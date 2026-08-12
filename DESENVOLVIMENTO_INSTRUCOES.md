# Instruções de desenvolvimento

Essas instruções cobrem pré-requisitos, criação do banco, execução de migrations/seed e como rodar frontend e backend em modo desenvolvimento.

## Pré-requisitos

- **.NET SDK 9.0** instalado (verifique com `dotnet --version`)
- **PostgreSQL 14+** (padrão) rodando em `localhost:5432` com usuário `postgres` (senha: `postgres`)
- **MySQL 8.0+** (opcional) rodando em `localhost:3306` com usuário `root` (senha vazia)
- **Node.js 18+** e **npm**
- **PowerShell 5.1+** (para executar scripts de desenvolvimento)
- (Opcional) `dotnet-ef` se desejar rodar migrations manualmente: `dotnet tool install --global dotnet-ef --version 9.0.13`

## Configurar banco de dados

### 1. Criar bancos de dados

**PostgreSQL (padrão):**
```sql
CREATE DATABASE grp_banco_01;
```

**MySQL (opcional, se quiser testar em ambos):**
```sql
CREATE DATABASE grp_banco_01;
```

### 2. Configurar connection strings

As connection strings estão definidas em **`src/retaguarda/Api/appsettings.json`**:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=grp_banco_01;Username=postgres;Password=postgres",
    "MySql": "Server=localhost;Port=3306;Database=grp_banco_01;User=root;Password="
  },
  "Persistence": {
    "Provider": "Postgres"
  }
}
```

### 3. Selecionar provedor de banco

**Para usar PostgreSQL (padrão):**
```powershell
# Sem necessidade de fazer nada — PostgreSQL é o padrão
$env:Persistence__Provider = 'Postgres'
```

**Para usar MySQL:**
```powershell
$env:Persistence__Provider = 'MySql'
```

> **Nota:** O arquivo `Persistencia/Configuracao.cs` registra os `DbContext`s apropriados baseado na variável de ambiente `Persistence__Provider`.

## Aplicar migrations e seed

### Forma Recomendada: Executar Script Automatizado

O repositório fornece um script PowerShell que faz tudo automaticamente:

```powershell
cd c:\PROJETOS\proposta_arquitetura_grp
.\iniciar-em-modo-deselvolvimento.ps1
```

**O que o script faz:**
- ✅ Verifica pré-requisitos (.NET, Node.js, PostgreSQL/MySQL)
- ✅ Restaura/instala ferramentas globais (`dotnet-ef`)
- ✅ Aplica migrations do seu banco (PostgreSQL ou MySQL)
- ✅ Inicia **3 serviços em janelas separadas:**
  - Backend (API): `http://localhost:5000`
  - PlanejadorFluxo (Elsa): `http://localhost:6001`
  - Frontend (Vite): `http://localhost:5173`

> **Nota:** O backend executa automaticamente um seeder que cria usuário `admin` com senha `admin` em modo desenvolvimento.

### Recriar Migrations do Zero (Desenvolvimento)

Se precisar deletar todas as migrations e recriar do zero:

```powershell
cd c:\PROJETOS\proposta_arquitetura_grp\scripts
.\criar_migracoes.ps1
```

**O script:**
1. Pergunta qual provedor (POSTGRESQL ou MYSQL)
2. Deleta todas as migrations antigas (com confirmação)
3. Deleta o banco de dados
4. Cria novo banco
5. Gera nova migration `InitialCreate`
6. Aplica ao banco

⚠️ **Aviso:** Este script é destrutivo. Use apenas em desenvolvimento.

## Rodando manualmente (sem scripts)

### 1. Aplicar migrations

**PostgreSQL:**
```powershell
cd src\retaguarda\Api
dotnet ef database update `
  -p ..\Persistencia `
  -s . `
  --context Retaguarda.Persistencia.POSTGRESQL.ApplicationDbContext
```

**MySQL:**
```powershell
cd src\retaguarda\Api
$env:Persistence__Provider = 'MySql'
dotnet ef database update `
  -p ..\Persistencia `
  -s . `
  --context Retaguarda.Persistencia.MYSQL.ApplicationDbContext
```

### 2. Criar nova migration

**PostgreSQL:**
```powershell
cd src\retaguarda\Api
$timestamp = Get-Date -Format "yyyyMMdd_HHmmss"
dotnet ef migrations add "$timestamp`_DescricaoDaMigracao" `
  -p ..\Persistencia `
  -s . `
  --context Retaguarda.Persistencia.POSTGRESQL.ApplicationDbContext `
  -o POSTGRESQL\Migracoes
```

**MySQL:**
```powershell
cd src\retaguarda\Api
$env:Persistence__Provider = 'MySql'
$timestamp = Get-Date -Format "yyyyMMdd_HHmmss"
dotnet ef migrations add "$timestamp`_DescricaoDaMigracao" `
  -p ..\Persistencia `
  -s . `
  --context Retaguarda.Persistencia.MYSQL.ApplicationDbContext `
  -o MYSQL\Migracoes
```

### 3. Iniciar backend

```powershell
cd src\retaguarda\Api
dotnet run
```

Escuta em: `http://localhost:5000` (HTTP) e `https://localhost:5001` (HTTPS)

### 4. Iniciar frontend

```powershell
cd src\interface_grafica\web
npm install  # Apenas na primeira vez
npm run dev
```

Dev server: `http://localhost:5173`

### 5. Iniciar PlanejadorFluxo/Elsa (opcional)

```powershell
cd src\retaguarda\Retaguarda.PlanejadorFluxo
dotnet build -c Debug /p:DefineConstants=ENABLE_ELSA
dotnet run --urls "http://localhost:6001"
```

Elsa Server: `http://localhost:6001/elsa`
Elsa Studio: `http://localhost:6001/studio`

## Padrão de nomenclatura de migrations

Todas as migrations devem seguir o padrão:

```
{YYYYMMDD_HHMMSS}_{DescricaoCamelCase}
```

**Exemplos:**
- `20260812_120000_CreateUsersTable`
- `20260812_120500_AddEmailToUsers`
- `20260812_121000_ApplyMultilocatarioTraits`

O timestamp garante:
- ✅ Unicidade global
- ✅ Ordem de execução determinística
- ✅ Fácil identificação de quando foi criada

## Endpoints importantes

- Metadados: `GET /meta/screens`, `GET /meta/modulos`, `GET /meta/components`
- API Organizações: `GET /api/organizacoes`, `POST /api/organizacoes`, `PUT /api/organizacoes/{id}`, `DELETE /api/organizacoes/{id}`
- API Setores: `GET /api/setores`, `POST /api/setores`, `PUT /api/setores/{id}`, `DELETE /api/setores/{id}`

## Notas de segurança

- As credenciais padrão `admin/admin` são apenas para desenvolvimento. Altere em produção.
- Configure `Jwt:Key` e outras secrets via `appsettings` ou gerenciador de segredos.
- Nunca commite secrets (senhas, tokens) no repositório — use variáveis de ambiente.

## Problemas comuns

| Problema | Solução |
|----------|---------|
| `Connection refused` | Verifique se PostgreSQL/MySQL está rodando em localhost:5432/3306 |
| `DbContext not found` | Certifique-se de usar `--context` completo: `Retaguarda.Persistencia.POSTGRESQL.ApplicationDbContext` |
| `Port already in use` | Mude porta: `$env:PLANEJADOR_PORT = '7001'` antes de executar script |
| `npm not found` | Instale Node.js de https://nodejs.org |
| `dotnet ef not found` | Execute: `dotnet tool install --global dotnet-ef --version 9.0.13` |

---

## Notas importantes

### Arquitetura Multi-Tenant com Traços (Traits)

Este projeto implementa um padrão de **Traços** para gerenciar campos multi-tenant:

- **V1 Traço:** 11 campos base de MultilocatarioEntidade (IdentificadorUnico, DataInsercao, OrganizacaoId, etc.)
- **Idempotência:** Todas as migrations tipo `20_*` usam `IF NOT EXISTS` para serem reaplicáveis
- **SGBD Agnóstico:** Suporta PostgreSQL, MySQL e MongoDB

Veja `src/retaguarda/Persistencia/TracosPadrao/` para referência.

### Configuração por Ambiente

- **Desenvolvimento:** `ASPNETCORE_ENVIRONMENT=Development`
  - Usuário padrão: `admin` / `admin` (criado automaticamente)
  - Logs detalhados habilitados
  - Seeder de dados executado

- **Produção:** Usar variáveis de ambiente para secrets e connection strings
  - Configure `Jwt:Key` via `appsettings.Production.json` ou gerenciador de segredos
  - Mude credenciais padrão
  - Desabilite seeder

### Documentação Completa

Para estratégias de migração em produção, troubleshooting avançado e procedimentos detalhados, consulte:

📖 **[DOCUMENTACAO/INSTRUCOES.md](DOCUMENTACAO/INSTRUCOES.md)**

Essa documentação inclui:
- ✅ Estratégia 1: Produção com Banco Existente (não-destrutiva)
- ✅ Estratégia 2: Produção com Traços (adicionar campos multi-tenant)
- ✅ Estratégia 3: Migração Blue-Green (downtime mínimo)
- ✅ Procedimento completo para recriar migrations do zero
- ✅ Troubleshooting detalhado
- ✅ Comandos específicos para PostgreSQL e MySQL

---
