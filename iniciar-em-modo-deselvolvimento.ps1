# Inicia a aplicacao em modo de desenvolvimento via PowerShell
Write-Host '============================================='
Write-Host 'Iniciando em modo desenvolvimento'
Write-Host '============================================='

Write-Host '1) Verificando prerequisitos (dotnet, node, npm)'
if (-not (Get-Command dotnet -ErrorAction SilentlyContinue)) { Write-Error 'Dotnet SDK nao encontrado. Instale .NET 8 SDK e tente novamente'; exit 1 }
if (-not (Get-Command node -ErrorAction SilentlyContinue)) { Write-Warning 'Node.js nao encontrado. Frontend pode nao iniciar.' }
if (-not (Get-Command npm -ErrorAction SilentlyContinue)) { Write-Warning 'npm nao encontrado. Frontend pode nao iniciar.' }

Write-Host '2) Aplicando migrations (EF Core)'
# If a local tools manifest exists, restore; otherwise ensure dotnet-ef is installed globally
$toolsManifestPaths = @()
$toolsManifestPaths += Join-Path $PSScriptRoot '.config\dotnet-tools.json'
$toolsManifestPaths += Join-Path $PSScriptRoot 'dotnet-tools.json'
$manifestExists = $false
foreach ($p in $toolsManifestPaths) { if (Test-Path $p) { $manifestExists = $true; break } }
if ($manifestExists) { dotnet tool restore } else { 
    Write-Host 'No tools manifest found; ensuring dotnet-ef (9.0.13) is installed globally'
    $efCmd = Get-Command dotnet-ef -ErrorAction SilentlyContinue
    if ($efCmd) { dotnet tool update --global dotnet-ef --version 9.0.13 | Out-Null } else { dotnet tool install --global dotnet-ef --version 9.0.13 | Out-Null }
}

# Default persistence provider
if (-not $Env:Persistence__Provider) { $Env:Persistence__Provider = 'Postgres' }
$provider = $Env:Persistence__Provider
Write-Host "Using persistence provider: $provider"

# Dev environment
if (-not $Env:ASPNETCORE_ENVIRONMENT) { $Env:ASPNETCORE_ENVIRONMENT = 'Development' }
Write-Host "ASPNETCORE_ENVIRONMENT=$Env:ASPNETCORE_ENVIRONMENT"

# Allow overriding Elsa base URL and Planejador port via environment variables
if (-not $Env:Elsa__BaseUrl) { $Env:Elsa__BaseUrl = 'http://localhost:4500' }
if (-not $Env:PLANEJADOR_PORT) { $Env:PLANEJADOR_PORT = '6000' }
$elsaUrl = $Env:Elsa__BaseUrl
$planejadorPort = $Env:PLANEJADOR_PORT
$planejadorUrl = "http://localhost:$planejadorPort"
Write-Host "Elsa BaseUrl: $elsaUrl"
Write-Host "Planejador will run on: $planejadorUrl"

# Quick reachability check for Elsa (best-effort)
try { $resp = Invoke-WebRequest -Uri $elsaUrl -UseBasicParsing -TimeoutSec 3 -ErrorAction Stop; Write-Host "Elsa reachable: $($resp.StatusCode) at $elsaUrl" } catch { Write-Warning "Elsa not reachable at $elsaUrl - planner may fail to contact Elsa." }

# EF context selection
if ($provider -eq 'MySql' -or $provider -eq 'mysql' -or $provider -eq 'MYSQL') { $contextType = 'Retaguarda.Persistencia.MYSQL.ApplicationDbContext' } else { $contextType = 'Retaguarda.Persistencia.POSTGRESQL.ApplicationDbContext' }
Write-Host "Using EF DbContext: $contextType"

# Apply API migrations
$efArgs = @( 'database', 'update', '--project', 'src\retaguarda\Persistencia\Retaguarda.Persistencia.csproj', '--startup-project', 'src\retaguarda\Api\Retaguarda.Api.csproj', '--context', $contextType )
dotnet ef @efArgs
if ($LASTEXITCODE -ne 0) { Write-Error 'Falha ao aplicar migrations para a API. Verifique a string de conexao e o servidor PostgreSQL.'; exit 1 }

# Apply PlanejadorFluxo migrations if any
Write-Host '2.1) Aplicando migrations do PlanejadorFluxo (Elsa)'
$planejadorProj = 'src\retaguarda\Retaguarda.PlanejadorFluxo\Retaguarda.PlanejadorFluxo.csproj'
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

# Start backend, planejador and frontend in new cmd windows
Write-Host '3) Iniciando backend (nova janela)'
$apiCmd = 'cd /d "' + $PSScriptRoot + '\src\retaguarda\Api" & dotnet run --project Retaguarda.Api.csproj'
Start-Process cmd -ArgumentList '/k', $apiCmd

Write-Host '3.1) Iniciando PlanejadorFluxo (Elsa) (nova janela)'
$planeCmd = 'set Elsa__BaseUrl=' + $elsaUrl + ' & set ASPNETCORE_URLS=' + $planejadorUrl + ' & cd /d "' + $PSScriptRoot + '\src\retaguarda\Retaguarda.PlanejadorFluxo" & dotnet run --project Retaguarda.PlanejadorFluxo.csproj'
Start-Process cmd -ArgumentList '/k', $planeCmd

Write-Host '4) Iniciando frontend (nova janela)'
$frontCmd = 'cd /d "' + $PSScriptRoot + '\src\interface_grafica\web"'
Start-Process cmd -ArgumentList '/k', $frontCmd

Write-Host 'Tudo iniciado. Verifique as janelas Backend e Frontend.'
