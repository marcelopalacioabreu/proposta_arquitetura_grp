# Inicia a aplicacao em modo de desenvolvimento via PowerShell
Write-Host '================================================='
Write-Host 'Iniciando em modo desenvolvimento'
Write-Host '================================================='

Write-Host '1) Verificando pre-requisitos (dotnet, node, npm)'
if (-not (Get-Command dotnet -ErrorAction SilentlyContinue)) { Write-Error 'Dotnet SDK nao encontrado. Instale .NET 8 SDK e tente novamente'; exit 1 }
if (-not (Get-Command node -ErrorAction SilentlyContinue)) { Write-Warning 'Node.js nao encontrado. Frontend pode nao iniciar.' }
if (-not (Get-Command npm -ErrorAction SilentlyContinue)) { Write-Warning 'npm nao encontrado. Frontend pode nao iniciar.' }

Write-Host '2) Aplicando migrations (EF Core)'
dotnet tool restore
$efArgs = @( 'database', 'update', '--project', 'src\retaguarda\Persistencia\Retaguarda.Persistencia.csproj', '--startup-project', 'src\retaguarda\Api\Retaguarda.Api.csproj', '--context', 'ApplicationDbContext' )
dotnet ef @efArgs
if ($LASTEXITCODE -ne 0) { Write-Error 'Falha ao aplicar migrations. Verifique a string de conexao e o servidor MySQL.'; exit 1 }

Write-Host '3) Iniciando backend (nova janela)'
Start-Process cmd -ArgumentList '/k', "cd /d `"$PSScriptRoot\src\retaguarda\Api`" & dotnet run --project Retaguarda.Api.csproj"

Write-Host '4) Iniciando frontend (nova janela)'
Start-Process cmd -ArgumentList '/k', "cd /d `"$PSScriptRoot\src\interface_grafica\web`" & if exist node_modules (npm run dev) else (npm install & npm run dev)"

Write-Host 'Tudo iniciado. Verifique as janelas Backend e Frontend.'
