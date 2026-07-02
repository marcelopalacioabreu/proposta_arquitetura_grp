@echo off
REM Inicia a aplicação em modo de desenvolvimento: aplica migrations, executa seed e sobe backend + frontend em janelas separadas.
SETLOCAL

echo ==================================================
echo Iniciando em modo desenvolvimento
echo ==================================================

echo 1) Verificando pré-requisitos (dotnet, npm)
dotnet --version >nul 2>&1 || (echo Dotnet SDK não encontrado. Instale .NET 8 SDK e tente novamente & pause & exit /b 1)
node -v >nul 2>&1 || (echo Node.js não encontrado. Instale Node.js (18+) e tente novamente & pause & exit /b 1)
npm -v >nul 2>&1 || (echo npm não encontrado. Instale npm e tente novamente & pause & exit /b 1)

echo 2) Aplicando migrations (EF Core)
dotnet tool restore
n:: Atualize o caminho do projeto se necessário
dotnet ef database update --project src\retaguarda\Persistencia\Retaguarda.Persistencia.csproj --startup-project src\retaguarda\Api\Retaguarda.Api.csproj --context ApplicationDbContext
if errorlevel 1 (
  echo Falha ao aplicar migrations. Verifique a string de conexão e o servidor MySQL.
  pause
  exit /b 1
)

echo 3) Iniciando backend (nova janela)
start "Backend" cmd /k "cd %~dp0\src\retaguarda\Api && dotnet run --project Retaguarda.Api.csproj"

echo 4) Iniciando frontend (nova janela). Pode demorar no primeiro start (npm install)
start "Frontend" cmd /k "cd %~dp0\interface_grafica\web && if exist node_modules (npm run dev) else (npm install && npm run dev)"

echo Tudo iniciado. Verifique as janelas "Backend" e "Frontend".
ENDLOCAL
pause
