#Requires -Version 5.0
<#
.SYNOPSIS
    Script para criar/regenerar migrations para diferentes bancos de dados.

.DESCRIPTION
    Este script permite:
    1. Selecionar banco de dados (POSTGRESQL, MYSQL)
    2. Validar e preparar pasta de migracoes
    3. Deletar migracoes antigas
    4. Criar novo ApplicationDbContext para o provider selecionado
    5. Gerar e aplicar novas migrations
#>

param(
    [switch]$SkipDatabaseDrop = $false,
    [switch]$DryRun = $false
)

$Colors = @{
    Info    = "Cyan"
    Success = "Green"
    Warning = "Yellow"
    Error   = "Red"
    Header  = "Magenta"
}

function Write-Info([string]$Message) { Write-Host $Message -ForegroundColor $Colors.Info }
function Write-Success([string]$Message) { Write-Host "OK $Message" -ForegroundColor $Colors.Success }
function Write-Warning([string]$Message) { Write-Host "! $Message" -ForegroundColor $Colors.Warning }
function Write-Error([string]$Message) { Write-Host "ERROR $Message" -ForegroundColor $Colors.Error }
function Write-Header([string]$Message) { Write-Host "`n========== $Message ==========" -ForegroundColor $Colors.Header }

$ProjectRoot = Split-Path $PSScriptRoot -Parent
$PersistenciaPath = Join-Path $ProjectRoot "src\retaguarda\Persistencia"
$ApiPath = Join-Path $ProjectRoot "src\retaguarda\Api"

Write-Header "Gerenciador de Migrations"

# PASSO 1: Selecionar banco
Write-Header "Passo 1: Selecionar Banco de Dados"
Write-Info "Opcoes: POSTGRESQL, MYSQL"

$BancoSelecionado = ""
$ValidBancos = @("POSTGRESQL", "MYSQL")

do {
    $input = Read-Host "Digite o banco (POSTGRESQL/MYSQL)" 
    $input = $input.ToUpper().Trim()
    
    if ($input -in $ValidBancos) {
        $BancoSelecionado = $input
        Write-Success "Banco selecionado: $BancoSelecionado"
        break
    } else {
        Write-Error "Banco invalido! Use: POSTGRESQL ou MYSQL"
    }
} while ($true)

# PASSO 2: Validar estrutura
Write-Header "Passo 2: Validando estrutura"

$MigracoesPath = Join-Path $PersistenciaPath "$BancoSelecionado\Migracoes"

if (-not (Test-Path $MigracoesPath)) {
    Write-Info "Criando diretorio: $MigracoesPath"
    if (-not $DryRun) {
        New-Item -ItemType Directory -Path $MigracoesPath -Force | Out-Null
        Write-Success "Diretorio criado"
    }
}

# PASSO 3: Confirmacao
Write-Header "AVISO IMPORTANTE!"
Write-Warning "Este script vai:"
Write-Warning "  1. Deletar TODOS os arquivos de migration em: $MigracoesPath"
Write-Warning "  2. Deletar o banco de dados $BancoSelecionado"
Write-Warning "  3. Gerar novas migrations"
Write-Warning "  4. Aplicar migrations ao banco"

if (-not $DryRun) {
    $response = Read-Host "Digite 'SIM' para continuar"
    if ($response -ne "SIM") {
        Write-Warning "Cancelado"
        exit 0
    }
}

# PASSO 4: Deletar migracoes antigas
Write-Header "Passo 3: Deletando migracoes antigas"

if (Test-Path $MigracoesPath) {
    $DeletedCount = 0
    Get-ChildItem -Path $MigracoesPath -Filter "*.cs" | ForEach-Object {
        if ($DryRun) {
            Write-Info "  [DELETE] $($_.Name)"
        } else {
            Remove-Item $_.FullName -Force -ErrorAction SilentlyContinue
            Write-Info "  OK $($_.Name)"
            $DeletedCount++
        }
    }
    if (-not $DryRun) {
        Write-Success "$DeletedCount arquivos deletados"
    }
}

# PASSO 5: Deletar database
if (-not $SkipDatabaseDrop) {
    Write-Header "Passo 4: Deletando database"
    
    if ($DryRun) {
        Write-Info "  [EXEC] dotnet ef database drop --force"
    } else {
        Push-Location $ApiPath
        try {
            dotnet ef database drop --force -p $PersistenciaPath -s . 2>&1 | Out-Null
            Write-Success "Database deletado"
        } catch {
            Write-Warning "Aviso ao deletar database (pode ser ignorado)"
        }
        Pop-Location
    }
}

# PASSO 6: Gerar migrations
Write-Header "Passo 5: Gerando novas migrations"

$ContextName = "Retaguarda.Persistencia.$BancoSelecionado.ApplicationDbContext"
$OutputDir = "$BancoSelecionado\Migracoes"

if ($DryRun) {
    Write-Info "  [EXEC] dotnet ef migrations add InitialCreate --context $ContextName --output-dir $OutputDir"
} else {
    Push-Location $ApiPath
    try {
        dotnet ef migrations add InitialCreate -p $PersistenciaPath -s . --context $ContextName -o $OutputDir 2>&1 | Out-Null
        Write-Success "Migrations criadas em $OutputDir"
    } catch {
        Write-Error "Erro ao criar migrations: $_"
        Pop-Location
        exit 1
    }
    Pop-Location
}

# PASSO 7: Aplicar migrations
Write-Header "Passo 6: Aplicando migrations"

if ($DryRun) {
    Write-Info "  [EXEC] dotnet ef database update --context $ContextName"
} else {
    Push-Location $ApiPath
    try {
        dotnet ef database update -p $PersistenciaPath -s . --context $ContextName 2>&1 | Out-Null
        Write-Success "Migrations aplicadas"
    } catch {
        Write-Error "Erro ao aplicar migrations: $_"
        Pop-Location
        exit 1
    }
    Pop-Location
}

# Conclusao
Write-Header "Pronto!"

if ($DryRun) {
    Write-Warning "DRY-RUN concluido. Nenhuma alteracao foi feita."
} else {
    Write-Success "Migrations regeneradas para $BancoSelecionado!"
    Write-Info ""
    Write-Info "Banco: $BancoSelecionado"
    Write-Info "Pasta: $MigracoesPath"
    Write-Info ""
    Write-Info "Proximos passos:"
    Write-Info "  1. dotnet build"
    Write-Info "  2. dotnet ef migrations list"
    Write-Info "  3. Testar a aplicacao"
}

exit 0
