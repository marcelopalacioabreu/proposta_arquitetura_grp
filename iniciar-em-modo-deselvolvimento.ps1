# Inicia a aplicacao em modo de desenvolvimento via PowerShell
Write-Host '================================================='
Write-Host 'Iniciando em modo desenvolvimento'
Write-Host '================================================='

Write-Host '1) Verificando pre-requisitos (dotnet, node, npm)'
if (-not (Get-Command dotnet -ErrorAction SilentlyContinue)) { Write-Error 'Dotnet SDK nao encontrado. Instale .NET 8 SDK e tente novamente'; exit 1 }
if (-not (Get-Command node -ErrorAction SilentlyContinue)) { Write-Warning 'Node.js nao encontrado. Frontend pode nao iniciar.' }
if (-not (Get-Command npm -ErrorAction SilentlyContinue)) { Write-Warning 'npm nao encontrado. Frontend pode nao iniciar.' }

Write-Host '2) Aplicando migrations (EF Core)'
# Restore local tool manifest if present, otherwise ensure dotnet-ef is available globally
$toolsManifestPaths = @()
$toolsManifestPaths += Join-Path $PSScriptRoot '.config\dotnet-tools.json'
$toolsManifestPaths += Join-Path $PSScriptRoot 'dotnet-tools.json'
$manifestExists = $false
foreach ($p in $toolsManifestPaths) { if (Test-Path $p) { $manifestExists = $true; break } }
if ($manifestExists) {
	dotnet tool restore
} else {
	Write-Host 'No tools manifest found; ensuring dotnet-ef (9.0.13) is installed globally'
	$efCmd = Get-Command dotnet-ef -ErrorAction SilentlyContinue
	if ($efCmd) {
		dotnet tool update --global dotnet-ef --version 9.0.13 | Out-Null
	} else {
		dotnet tool install --global dotnet-ef --version 9.0.13 | Out-Null
	}
}
if (-not $Env:Persistence__Provider) { $Env:Persistence__Provider = 'Postgres' }
$provider = $Env:Persistence__Provider
Write-Host "Using persistence provider: $provider"

# Ensure development environment is set so appsettings.Development.json is used
if (-not $Env:ASPNETCORE_ENVIRONMENT) { $Env:ASPNETCORE_ENVIRONMENT = 'Development' }
Write-Host "ASPNETCORE_ENVIRONMENT=$Env:ASPNETCORE_ENVIRONMENT"

# Select the fully-qualified DbContext type for EF tools to avoid ambiguity
if ($provider -eq 'MySql' -or $provider -eq 'mysql' -or $provider -eq 'MYSQL') {
	$contextType = 'Retaguarda.Persistencia.MYSQL.ApplicationDbContext'
} else {
	$contextType = 'Retaguarda.Persistencia.POSTGRESQL.ApplicationDbContext'
}

Write-Host "Using EF DbContext: $contextType"

$efArgs = @( 'database', 'update', '--project', 'src\retaguarda\Persistencia\Retaguarda.Persistencia.csproj', '--startup-project', 'src\retaguarda\Api\Retaguarda.Api.csproj', '--context', $contextType )
dotnet ef @efArgs
if ($LASTEXITCODE -ne 0) { Write-Error 'Falha ao aplicar migrations para a API. Verifique a string de conexao e o servidor PostgreSQL.'; exit 1 }

# Apply Elsa/PlanejadorFluxo migrations (if any)
Write-Host '2.1) Aplicando migrations do PlanejadorFluxo (Elsa)'
# Check whether the startup/project assembly exposes any DbContext before attempting update
$planejadorProj = 'src\retaguarda\Retaguarda.PlanejadorFluxo\Retaguarda.PlanejadorFluxo.csproj'
$startupProj = $planejadorProj
Write-Host "Checking for DbContexts in $planejadorProj..."
$dbctxList = dotnet ef dbcontext list --project $planejadorProj --startup-project $startupProj 2>&1
if ($dbctxList -match 'No DbContext was found') {
	Write-Host 'No DbContext found in PlanejadorFluxo project; skipping Elsa migrations.'
} else {
	$efArgsElsa = @( 'database', 'update', '--project', $planejadorProj, '--startup-project', $startupProj )
	dotnet ef @efArgsElsa
	if ($LASTEXITCODE -ne 0) { Write-Warning 'Falha ao aplicar migrations do PlanejadorFluxo (verifique a configuração ou gere as migrations manualmente).' }
}

Write-Host '3) Iniciando backend (nova janela)'
Start-Process cmd -ArgumentList '/k', "cd /d `"$PSScriptRoot\src\retaguarda\Api`" & dotnet run --project Retaguarda.Api.csproj"

Write-Host '3.1) Iniciando PlanejadorFluxo (Elsa) (nova janela)'
Start-Process cmd -ArgumentList '/k', "cd /d `"$PSScriptRoot\src\retaguarda\Retaguarda.PlanejadorFluxo`" & dotnet run --project Retaguarda.PlanejadorFluxo.csproj"

Write-Host '4) Iniciando frontend (nova janela)'
Start-Process cmd -ArgumentList '/k', "cd /d `"$PSScriptRoot\src\interface_grafica\web`" & if exist node_modules (npm run dev) else (npm install & npm run dev)"

Write-Host 'Tudo iniciado. Verifique as janelas Backend e Frontend.'
