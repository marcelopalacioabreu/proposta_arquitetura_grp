# Inicia a aplicacao em modo de desenvolvimento via PowerShell (VERSÃO MELHORADA)
Write-Host '============================================='
Write-Host 'Iniciando em modo desenvolvimento'
Write-Host '============================================='

Write-Host '1) Verificando prerequisitos (dotnet, node, npm)'
if (-not (Get-Command dotnet -ErrorAction SilentlyContinue)) { Write-Error 'Dotnet SDK nao encontrado. Instale .NET 8 SDK e tente novamente'; exit 1 }
if (-not (Get-Command node -ErrorAction SilentlyContinue)) { Write-Warning 'Node.js nao encontrado. Frontend pode nao iniciar.' }
if (-not (Get-Command npm -ErrorAction SilentlyContinue)) { Write-Warning 'npm nao encontrado. Frontend pode nao iniciar.' }

# Definir variáveis de ambiente ANTES de tudo
Write-Host '1.1) Configurando variáveis de ambiente...'
if (-not $Env:Persistence__Provider) { $Env:Persistence__Provider = 'Postgres' }
if (-not $Env:ASPNETCORE_ENVIRONMENT) { $Env:ASPNETCORE_ENVIRONMENT = 'Development' }
if (-not $Env:Elsa__BaseUrl) { $Env:Elsa__BaseUrl = 'http://localhost:4500' }
if (-not $Env:PLANEJADOR_PORT) { $Env:PLANEJADOR_PORT = '6001' }

$provider = $Env:Persistence__Provider
$planejadorPort = $Env:PLANEJADOR_PORT
$planejadorUrl = "http://localhost:$planejadorPort"
$elsaUrl = $planejadorUrl

Write-Host "  ASPNETCORE_ENVIRONMENT=$Env:ASPNETCORE_ENVIRONMENT"
Write-Host "  Persistence__Provider=$provider"
Write-Host "  ENABLE_ELSA=1"
Write-Host "  PlanejadorFluxo URL: $planejadorUrl"

# TESTE DE CONEXÃO POSTGRESQL (apenas se usar Postgres)
if ($provider -eq 'Postgres' -or $provider -eq 'postgres' -or $provider -eq 'POSTGRES') {
    Write-Host ''
    Write-Host '1.2) Verificando conectividade com PostgreSQL...'
    
    $psqlResponse = Test-NetConnection -ComputerName localhost -Port 5432 -WarningAction SilentlyContinue -InformationLevel Quiet
    if (-not $psqlResponse.TcpTestSucceeded) {
        Write-Error 'PostgreSQL nao esta respondendo em localhost:5432'
        Write-Host '  Verifique se:'
        Write-Host '    1. PostgreSQL esta instalado'
        Write-Host '    2. PostgreSQL esta rodando (Services > PostgreSQL)'
        Write-Host '    3. Porta 5432 nao esta bloqueada'
        exit 1
    }
    
    Write-Host '✓ PostgreSQL respondendo'
    
    # Testar com psql se disponível
    $psqlCmd = Get-Command psql -ErrorAction SilentlyContinue
    if ($psqlCmd) {
        Write-Host '  Testando autenticação com psql...'
        $env:PGPASSWORD = 'postgres'
        $testOutput = & psql -h localhost -U postgres -d postgres -t -c "SELECT 1;" 2>&1
        if ($LASTEXITCODE -eq 0) {
            Write-Host '✓ Autenticação bem-sucedida (postgres/postgres)'
        } else {
            Write-Error "Erro de autenticação PostgreSQL"
            Write-Host "  Verifique:"
            Write-Host "    1. Senha do usuário postgres esta 'postgres'"
            Write-Host "    2. Usuário postgres existe"
            exit 1
        }
    }
}

Write-Host ''
Write-Host '2) Aplicando migrations (EF Core)'

# If a local tools manifest exists, restore; otherwise ensure dotnet-ef is installed globally
$toolsManifestPaths = @()
$toolsManifestPaths += Join-Path $PSScriptRoot '.config\dotnet-tools.json'
$toolsManifestPaths += Join-Path $PSScriptRoot 'dotnet-tools.json'
$manifestExists = $false
foreach ($p in $toolsManifestPaths) { if (Test-Path $p) { $manifestExists = $true; break } }

if ($manifestExists) { 
    Write-Host 'Restaurando ferramentas locais...'
    dotnet tool restore 
} else { 
    Write-Host 'Instalando dotnet-ef 9.0.13 globalmente...'
    $efCmd = Get-Command dotnet-ef -ErrorAction SilentlyContinue
    if ($efCmd) { dotnet tool update --global dotnet-ef --version 9.0.13 | Out-Null } else { dotnet tool install --global dotnet-ef --version 9.0.13 | Out-Null }
}

# DEFINIR VARIÁVEIS DE AMBIENTE EXPLICITAMENTE PARA DOTNET EF
# Isso garante que sejam herdadas pelos processos filhos
$env:Persistence__Provider = $provider
$env:ASPNETCORE_ENVIRONMENT = 'Development'

Write-Host "Using persistence provider: $provider"

# EF context selection
if ($provider -eq 'MySql' -or $provider -eq 'mysql' -or $provider -eq 'MYSQL') { 
    $contextType = 'Retaguarda.Persistencia.MYSQL.ApplicationDbContext' 
} else { 
    $contextType = 'Retaguarda.Persistencia.POSTGRESQL.ApplicationDbContext' 
}
Write-Host "Using EF DbContext: $contextType"

# Apply API migrations
$apiProject = Join-Path $PSScriptRoot 'src\retaguarda\Persistencia\Retaguarda.Persistencia.csproj'
$apiStartup = Join-Path $PSScriptRoot 'src\retaguarda\Api\Retaguarda.Api.csproj'
$efArgs = @( 'database', 'update', '--project', $apiProject, '--startup-project', $apiStartup, '--context', $contextType, '--verbose' )

Write-Host ''
Write-Host 'Executando: dotnet ef' $efArgs
Write-Host ''
dotnet ef @efArgs

if ($LASTEXITCODE -ne 0) { 
    Write-Error 'Falha ao aplicar migrations para a API.'
    Write-Host ''
    Write-Host 'Possíveis causas:'
    Write-Host '  1. PostgreSQL nao esta rodando'
    Write-Host '  2. Banco de dados "grp_banco_01" nao existe'
    Write-Host '  3. Credenciais postgres/postgres incorretas'
    Write-Host '  4. String de conexao esta errada'
    Write-Host ''
    Write-Host 'Para diagnosticar, execute:'
    Write-Host '  .\diagnosticar-conexao-postgres.ps1'
    exit 1 
}

Write-Host ''
Write-Host '✓ Migrations aplicadas com sucesso'

# Apply PlanejadorFluxo migrations if any
Write-Host ''
Write-Host '2.1) Verificando migrations do PlanejadorFluxo (Elsa)...'
$planejadorProj = Join-Path $PSScriptRoot 'src\retaguarda\Retaguarda.PlanejadorFluxo\Retaguarda.PlanejadorFluxo.csproj'
$startupProj = $planejadorProj
$dbctxListRaw = dotnet ef dbcontext list --project $planejadorProj --startup-project $startupProj 2>&1

if ($dbctxListRaw -match 'No DbContext was found') {
    Write-Host 'No DbContext found in PlanejadorFluxo project; skipping Elsa migrations.'
} else {
    # Parse the output into a list of context names
    $dbctxs = $dbctxListRaw -split "`n" | ForEach-Object { $_.Trim() } | Where-Object { $_ -ne '' -and $_ -notmatch '^(Found|Using|No DbContext)' }

    if ($dbctxs.Count -eq 0) {
        Write-Host 'Could not parse DbContext list; running generic update (may prompt for --context)'
        dotnet ef database update --project $planejadorProj --startup-project $startupProj
    } elseif ($dbctxs.Count -eq 1) {
        $ctx = $dbctxs[0]
        Write-Host "One DbContext found: $ctx - applying migrations for this context"
        dotnet ef database update --project $planejadorProj --startup-project $startupProj --context $ctx
    } else {
        Write-Host "Multiple DbContexts found:`n$($dbctxs -join "`n")"
        # Prefer known Elsa contexts; otherwise apply to all discovered contexts
        $preferred = @('ManagementElsaDbContext','RuntimeElsaDbContext','Management','Runtime')
        $elsaMatches = $dbctxs | Where-Object { $preferred -contains $_ -or $_ -match 'Elsa' }
        if ($elsaMatches.Count -gt 0) {
            foreach ($c in $elsaMatches) {
                Write-Host "Applying migrations for context: $c"
                dotnet ef database update --project $planejadorProj --startup-project $startupProj --context $c
                if ($LASTEXITCODE -ne 0) { Write-Warning "Failed to apply migrations for $c" }
            }
        } else {
            Write-Host 'No preferred Elsa DbContext names found; applying migrations for all discovered contexts (may take time)'
            foreach ($c in $dbctxs) {
                Write-Host "Applying migrations for context: $c"
                dotnet ef database update --project $planejadorProj --startup-project $startupProj --context $c
                if ($LASTEXITCODE -ne 0) { Write-Warning "Failed to apply migrations for $c" }
            }
        }
    }
}

Write-Host ''
Write-Host '============================================='
Write-Host 'Iniciando serviços...'
Write-Host '============================================='

# Ensure environment variables are available to child processes
$env:ENABLE_ELSA = '1'
$env:Elsa__BaseUrl = $planejadorUrl
$env:PLANEJADOR_PORT = $planejadorPort

Write-Host '3) Iniciando backend (nova janela)'
$apiCmd = 'cd /d "' + $PSScriptRoot + '\src\retaguarda\Api" & dotnet run --project Retaguarda.Api.csproj'
Start-Process cmd -ArgumentList '/k', $apiCmd

Write-Host '3.1) Iniciando PlanejadorFluxo (Elsa) (nova janela)'
$planeDir = Join-Path $PSScriptRoot 'src\retaguarda\Retaguarda.PlanejadorFluxo'
$planeCmd = 'set Elsa__BaseUrl=' + $planejadorUrl + ' & set ENABLE_ELSA=1 & cd /d "' + $planeDir + '"'
if ($Env:ENABLE_ELSA -eq '1' -or $Env:ENABLE_ELSA.ToLower() -eq 'true') {
    $planeCmd += ' & dotnet build -c Debug /p:DefineConstants=ENABLE_ELSA'
} else {
    $planeCmd += ' & dotnet build -c Debug'
}
$planeCmd += ' & dotnet run --project Retaguarda.PlanejadorFluxo.csproj --no-launch-profile --urls "' + $planejadorUrl + '"'
Start-Process cmd -ArgumentList '/k', $planeCmd

Write-Host '4) Iniciando frontend (nova janela)'
$webPath = Join-Path $PSScriptRoot 'src\interface_grafica\web'
$frontCmd = 'set PLANEJADOR_URL=' + $planejadorUrl + ' & cd /d "' + $webPath + '"'
if (-not (Test-Path (Join-Path $webPath 'node_modules'))) {
    $frontCmd += ' & npm install'
}
$frontCmd += ' & npm run dev'
Start-Process cmd -ArgumentList '/k', $frontCmd

Write-Host ''
Write-Host '============================================='
Write-Host 'Serviços iniciados com sucesso!'
Write-Host '============================================='
Write-Host ''
Write-Host 'URLs disponíveis:'
Write-Host '  Backend (API):      http://localhost:5000'
Write-Host '  PlanejadorFluxo:    http://localhost:' + $planejadorPort
Write-Host '  Frontend (Vite):    http://localhost:5173'
Write-Host ''
Write-Host 'Verifique as janelas Backend e Frontend para logs.'
Write-Host ''
