@echo off
REM Start the application in development mode: apply migrations, run seed and open backend + frontend in separate windows.
SETLOCAL ENABLEDELAYEDEXPANSION

echo ==================================================
echo Iniciando em modo desenvolvimento
echo ==================================================

echo 1) Verificando pre-requisitos (dotnet, npm)
dotnet --version >nul 2>&1
if %errorlevel% neq 0 (
  echo Dotnet SDK nao encontrado. Instale .NET 8 SDK e tente novamente
  pause
  exit /b 1
)

node -v >nul 2>&1
if %errorlevel% neq 0 (
  echo Node.js nao encontrado. Instale Node.js (18+) e tente novamente
  pause
  exit /b 1
)

npm -v >nul 2>&1
if %errorlevel% neq 0 (
  echo npm nao encontrado. Instale npm e tente novamente
  pause
  exit /b 1
)

echo 2) Aplicando migrations (EF Core)
dotnet tool restore
dotnet ef database update --project "%~dp0src\retaguarda\Persistencia\Retaguarda.Persistencia.csproj" --startup-project "%~dp0src\retaguarda\Api\Retaguarda.Api.csproj" --context ApplicationDbContext
if %errorlevel% neq 0 (
  echo Falha ao aplicar migrations. Verifique a string de conexao e o servidor MySQL.
  pause
  exit /b 1
)

echo 3) Iniciando backend (nova janela)
start "Backend" cmd /k "cd /d "%~dp0src\retaguarda\Api" && dotnet run --project Retaguarda.Api.csproj"

echo 4) Iniciando frontend (nova janela)
start "Frontend" cmd /k "cd /d "%~dp0interface_grafica\web" && if exist node_modules (npm run dev) else (npm install && npm run dev)"

echo Tudo iniciado. Verifique as janelas "Backend" e "Frontend".
ENDLOCAL
pause

