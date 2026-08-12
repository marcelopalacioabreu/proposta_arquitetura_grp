# INSTRUÇÕES para restaurar, criar migrations e executar a API

Pré-requisitos
- .NET SDK 8 instalado (verifique com `dotnet --version`).
- MySQL rodando em `localhost:3306` com usuário `root` (senha vazia conforme configuração atual).
- PostgreSQL rodando em `localhost:5432` com usuário `postgres` (senha postgres conforme configuração atual).

Arquivos importantes
- `src/retaguarda/Api/appsettings.json` — connection string usada pela aplicação.
- `src/retaguarda/Persistencia` (ou `src/retaguarda/Persistencia/MYSQL`) — projeto onde ficam `ApplicationDbContext` e as migrations.

Resumo: o EF carrega migrations a partir da assembly compilada e do `DbContext` que você indicar. A organização em pastas (`Migracoes/MYSQL`) é apenas física — informe o projeto/assembly e o `--context` ao rodar os comandos para aplicar a migração correta.

Passos (PowerShell)

1. Ir para o projeto de API (startup project):

```powershell
cd src\retaguarda\Api
```

2. Restaurar dependências:

```powershell
dotnet restore
```

3. Instalar a ferramenta `dotnet-ef` (se ainda não estiver instalada):

```powershell
dotnet tool install --global dotnet-ef
```

4. Criar uma migration (ex.: numerada `000002_descricao`) — especifique o projeto que contém as migrations com `-p`, o startup project com `-s`, o `--context` e a pasta de saída `--output-dir` se quiser que a migration vá para `Migracoes/MYSQL`:

```powershell
dotnet ef migrations add 000002_descricao -p ..\Persistencia\ -s . --context ApplicationDbContext --output-dir Migracoes/MYSQL
```

Notas:
- `-p ..\Persistencia\` = projeto que contém o `DbContext` e onde as migrations serão adicionadas.
- `-s .` = projeto de startup (neste caso `Api`) usado para construir e executar a aplicação durante o processo de design-time.
- `--context ApplicationDbContext` = especifica qual `DbContext` usar (importante quando há vários).

5. Aplicar as migrations ao banco (executa todas as migrations pendentes para o `DbContext` especificado):

```powershell
dotnet ef database update -p ..\Persistencia\ -s . --context ApplicationDbContext
```

Para aplicar até uma migration específica:

```powershell
dotnet ef database update 000001_InitialCreate -p ..\Persistencia\ -s . --context ApplicationDbContext
```

Considerações para múltiplos bancos / contexts
- Se você tem dois bancos (por exemplo MySQL e SQL Server) prefira ter um `DbContext` por banco e/ou um projeto `Persistencia` por banco (`Persistencia.MYSQL`, `Persistencia.SqlServer`).
- Alternativamente, mantenha `DbContext`s no mesmo projeto mas configure `MigrationsAssembly` e `MigrationsHistoryTable` ao configurar o provider para separar os históricos:

```csharp
options.UseMySql(connString, serverVersion, o =>
	o.MigrationsAssembly("Retaguarda.Persistencia")
	 .MigrationsHistoryTable("__EFMigrationsHistory_MYSQL")
);
```

Sobre os arquivos `.Designer.cs` e o snapshot
- O EF gera para cada migration um par de arquivos: `0000XX_Nome.cs` (a classe `Migration`) e `0000XX_Nome.Designer.cs` (contém `BuildTargetModel` / o snapshot parcial). Há também o `ApplicationDbContextModelSnapshot.cs` que representa o modelo atual do projeto.
- Normalmente você NÃO precisa editar os `.Designer.cs` — use `dotnet ef migrations add` para que o EF gere corretamente a migration e o designer.
- Se você **criar migrations manualmente**, deve garantir que a classe `Migration`, o `.Designer.cs` (ou o snapshot) e o `ApplicationDbContextModelSnapshot.cs` reflitam o estado do modelo para que futuras migrations funcionem corretamente.

Boas práticas rápidas
- Use `--output-dir` para manter migrations organizadas por banco (ex.: `Migracoes/MYSQL`).
- Use nomes numerados (`000001_...`, `000002_...`) se quiser um esquema de versionamento legível.
- Prefira `dotnet ef migrations add 00000X_nome` para que a ferramenta gere arquivos com o nome correto — evita editar atributos manualmente.

Problemas comuns
- Erro: "dotnet ef not found" → execute o passo 3 e reinicie o terminal.
- Erro de compilação ao rodar `dotnet ef` → verifique se o `startup project` compila e se o namespace/assembly onde `ApplicationDbContext` está definido é referenciado corretamente no `Program.cs`.
- Erro de conexão MySQL → verifique se o serviço MySQL está ativo e se as credenciais em `appsettings.json` estão corretas.

Exemplo: criar e aplicar migration numerada para MySQL

```powershell
cd src\retaguarda\Api
dotnet restore
dotnet ef migrations add 000002_adiciona_setor -p ..\Persistencia\ -s . --context ApplicationDbContext --output-dir Migracoes/MYSQL
dotnet ef database update -p ..\Persistencia\ -s . --context ApplicationDbContext
```

---

## Scripts de Desenvolvimento Automatizados

Este projeto fornece dois scripts PowerShell para facilitar o desenvolvimento:

### 1. `scripts/criar_migracoes.ps1` — Recriar Migrations do Zero

**Propósito:** Deletar todas as migrations, recriar o banco de dados e gerar novas migrations do zero.

**⚠️ Aviso:** Este script é **destrutivo**. Ele deleta o banco de dados e todas as migrations. Use apenas em desenvolvimento.

**Pré-requisitos:**
- PostgreSQL rodando em `localhost:5432`
- MySQL rodando em `localhost:3306`
- PowerShell 5.1 ou superior
- .NET 9 SDK instalado

**Como Executar:**

```powershell
cd c:\PROJETOS\proposta_arquitetura_grp\scripts
.\criar_migracoes.ps1
```

**O que o Script Faz:**

1. Pergunta qual SGBD usar: **POSTGRESQL** ou **MYSQL**
   ```
   Qual provedor? (POSTGRESQL/MYSQL): POSTGRESQL
   ```

2. Valida a pasta `POSTGRESQL/Migracoes` ou `MYSQL/Migracoes`

3. **Deleta todas as migrations antigas** (com confirmação)
   ```powershell
   # PostgreSQL
   Remove-Item "src\retaguarda\Persistencia\POSTGRESQL\Migracoes\*" -Recurse -Force
   
   # MySQL
   Remove-Item "src\retaguarda\Persistencia\MYSQL\Migracoes\*" -Recurse -Force
   ```

4. **Deleta o banco de dados** (com confirmação)
   ```powershell
   # PostgreSQL
   psql -U postgres -c "DROP DATABASE IF EXISTS grp_banco_01;"
   psql -U postgres -c "CREATE DATABASE grp_banco_01;"
   
   # MySQL
   mysql -u root -e "DROP DATABASE IF EXISTS grp_banco_01; CREATE DATABASE grp_banco_01;"
   ```

5. **Compila o projeto Persistencia**
   ```powershell
   dotnet build src\retaguarda\Persistencia\Retaguarda.Persistencia.csproj
   ```

6. **Gera nova migration `InitialCreate`**
   ```powershell
   # PostgreSQL
   dotnet ef migrations add InitialCreate `
     --project src\retaguarda\Persistencia `
     --startup-project src\retaguarda\Api `
     --context Retaguarda.Persistencia.POSTGRESQL.ApplicationDbContext `
     --output-dir POSTGRESQL\Migracoes
   
   # MySQL
   dotnet ef migrations add InitialCreate `
     --project src\retaguarda\Persistencia `
     --startup-project src\retaguarda\Api `
     --context Retaguarda.Persistencia.MYSQL.ApplicationDbContext `
     --output-dir MYSQL\Migracoes
   ```

7. **Aplica a migration ao banco**
   ```powershell
   # PostgreSQL
   dotnet ef database update `
     --project src\retaguarda\Persistencia `
     --startup-project src\retaguarda\Api `
     --context Retaguarda.Persistencia.POSTGRESQL.ApplicationDbContext
   
   # MySQL (com variável de ambiente)
   $env:Persistence__Provider = "MySql"
   dotnet ef database update `
     --project src\retaguarda\Persistencia `
     --startup-project src\retaguarda\Api `
     --context Retaguarda.Persistencia.MYSQL.ApplicationDbContext
   ```

**Confirmações Requeridas:**

O script exige digitação explícita de **"SIM"** (em maiúsculas) antes de deletar:

```
AVISO: Isto irá deletar TODAS as migrations e o banco de dados grp_banco_01!
Tem certeza? Digite 'SIM' para continuar:
```

**Saída Esperada:**

```
✓ Migrations criadas com sucesso em: src\retaguarda\Persistencia\POSTGRESQL\Migracoes
✓ Database grp_banco_01 atualizado com sucesso
✓ Migrations concluídas!
```

**Troubleshooting:**

| Erro | Solução |
|------|---------|
| `Connection refused` | Verifique se PostgreSQL/MySQL estão rodando |
| `Permission denied` | Execute PowerShell como Administrador |
| `dotnet ef not found` | Instale: `dotnet tool install --global dotnet-ef` |

---

### 2. `iniciar-em-modo-deselvolvimento.ps1` — Iniciar Aplicação Completa

**Propósito:** Iniciar automaticamente todos os 3 serviços em janelas separadas:
- **Backend** (API)
- **PlanejadorFluxo** (Elsa Workflow Server + Studio)
- **Frontend** (Vite dev server)

**Pré-requisitos:**
- PostgreSQL ou MySQL rodando
- Node.js e npm instalados
- .NET 9 SDK instalado
- PowerShell 5.1 ou superior

**Como Executar:**

```powershell
cd c:\PROJETOS\proposta_arquitetura_grp
.\iniciar-em-modo-deselvolvimento.ps1
```

**O que o Script Faz:**

1. **Verifica Pré-requisitos**
   - ✅ .NET SDK
   - ✅ Node.js e npm
   - ✅ PostgreSQL/MySQL

2. **Instala/Restaura Ferramentas**
   - Restaura `dotnet-ef` global ou local
   - Instala v9.0.13 automaticamente se necessário

3. **Configura Variáveis de Ambiente**
   ```powershell
   $env:Persistence__Provider = 'Postgres'  # ou 'MySql' se necessário
   $env:ASPNETCORE_ENVIRONMENT = 'Development'
   $env:Elsa__BaseUrl = 'http://localhost:6001'
   $env:PLANEJADOR_PORT = '6001'
   $env:ENABLE_ELSA = '1'
   ```

4. **Aplica Migrations**
   ```powershell
   # Detecta o provedor configurado e aplica migrations
   # Se usar MySQL, define $env:Persistence__Provider = "MySql"
   dotnet ef database update --context [provedor especificado]
   ```

5. **Inicia Backend (nova janela CMD)**
   ```powershell
   cd src\retaguarda\Api
   dotnet run
   ```
   - Escuta em: `http://localhost:5000` (HTTP) e `https://localhost:5001` (HTTPS)

6. **Inicia PlanejadorFluxo/Elsa (nova janela CMD)**
   ```powershell
   cd src\retaguarda\Retaguarda.PlanejadorFluxo
   dotnet build -c Debug /p:DefineConstants=ENABLE_ELSA
   dotnet run --urls "http://localhost:6001"
   ```
   - Elsa Server: `http://localhost:6001/elsa`
   - Elsa Studio: `http://localhost:6001/studio`

7. **Instala dependências Frontend (se necessário)**
   ```powershell
   cd src\interface_grafica\web
   npm install  # Executado apenas se node_modules não existir
   ```

8. **Inicia Frontend Vite (nova janela CMD)**
   ```powershell
   cd src\interface_grafica\web
   npm run dev
   ```
   - Dev server: `http://localhost:5173` (ou porta configurada no Vite)
   - Conecta ao backend: `http://localhost:5000`
   - Conecta ao PlanejadorFluxo: `http://localhost:6001`

**Variáveis de Ambiente (Customizáveis):**

Você pode personalizar o script antes de executar:

```powershell
# Usar MySQL em vez de PostgreSQL
$env:Persistence__Provider = 'MySql'

# Alterar porta do PlanejadorFluxo
$env:PLANEJADOR_PORT = '7001'

# Alterar base URL do Elsa
$env:Elsa__BaseUrl = 'http://localhost:7001'

# Desabilitar Elsa completamente
$env:ENABLE_ELSA = '0'

# Então execute o script
.\iniciar-em-modo-deselvolvimento.ps1
```

**Saída Esperada:**

```
=============================================
Iniciando em modo desenvolvimento
=============================================

1) Verificando prerequisitos (dotnet, node, npm)
2) Aplicando migrations (EF Core)
Using persistence provider: Postgres
ASPNETCORE_ENVIRONMENT=Development
Elsa BaseUrl: http://localhost:6001
Planejador will run on: http://localhost:6001
ENABLE_ELSA=1
Elsa reachable: 200 at http://localhost:6001
Using EF DbContext: Retaguarda.Persistencia.POSTGRESQL.ApplicationDbContext

3) Iniciando backend (nova janela)
3.1) Iniciando PlanejadorFluxo (Elsa) (nova janela)
4) Iniciando frontend (nova janela)
Tudo iniciado. Verifique as janelas Backend e Frontend.
```

**URLs em Desenvolvimento:**

| Serviço | URL | Função |
|---------|-----|--------|
| Backend API | `http://localhost:5000` | API REST |
| Frontend | `http://localhost:5173` | Interface web |
| Elsa Server | `http://localhost:6001/elsa` | Engine de workflows |
| Elsa Studio | `http://localhost:6001/studio` | Editor visual de workflows |

**Conectar Frontend ao Backend:**

Dentro do frontend (Vite), configure a URL do backend:

```javascript
// src/config.js ou .env.development
VITE_API_URL=http://localhost:5000
VITE_PLANEJADOR_URL=http://localhost:6001
```

**Troubleshooting:**

| Erro | Solução |
|------|---------|
| `npm not found` | Instale Node.js de https://nodejs.org |
| `Database connection refused` | Verifique se PostgreSQL/MySQL está rodando |
| `Port already in use` | Mude `$env:PLANEJADOR_PORT` antes de executar o script |
| `Elsa not reachable` | Aguarde 5-10s para o serviço iniciar, ou desabilite com `$env:ENABLE_ELSA=0` |
| `Frontend não conecta ao backend` | Verifique URLs em `VITE_API_URL` e CORS na API |

**Parar os Serviços:**

Feche as janelas CMD que foram abertas. Alternativamente, use:

```powershell
# PowerShell
Get-Process | Where-Object { $_.MainWindowTitle -like "*dotnet*" } | Stop-Process
Get-Process | Where-Object { $_.MainWindowTitle -like "*npm*" } | Stop-Process
Get-Process cmd | Stop-Process -Force  # Force close all cmd windows
```

---

## Estratégias de Migração em Produção

### Contexto do Projeto: Multi-Tenant com Traços (Traits)

Este projeto implementa um sistema **multi-tenant** com suporte a **múltiplos SGBDs** (PostgreSQL e MySQL) usando o padrão de **Traços (Traits)** para controle de versão de campos.

**Organização de Migrations:**
- Todas as migrations seguem o padrão: `{YYYYMMDD_HHMMSS}_{Descricao}`
- Subdivididas por banco: `POSTGRESQL/Migracoes` e `MYSQL/Migracoes`
- Gerenciadas por `DbContext` específico: `Retaguarda.Persistencia.POSTGRESQL.ApplicationDbContext` ou `Retaguarda.Persistencia.MYSQL.ApplicationDbContext`

### Configuração de Connection Strings

As connection strings são definidas em dois lugares:

#### 1. **Arquivo de Configuração: `src/retaguarda/Api/appsettings.json`**

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

- **DefaultConnection** → PostgreSQL (padrão)
  - Host: `localhost`
  - Port: `5432`
  - Database: `grp_banco_01`
  - Username: `postgres`
  - Password: `postgres`

- **MySql** → MySQL
  - Server: `localhost`
  - Port: `3306`
  - Database: `grp_banco_01`
  - User: `root`
  - Password: (vazio)

- **Persistence.Provider** → Define qual banco usar:
  - `"Postgres"` (padrão)
  - `"MySql"`

#### 2. **Registro de Dependency Injection: `src/retaguarda/Persistencia/Configuracao.cs`**

Este arquivo registra os `DbContext`s no contêiner de DI:

```csharp
// Exemplo para PostgreSQL
services.AddDbContext<Retaguarda.Persistencia.POSTGRESQL.ApplicationDbContext>(
    options => options.UseNpgsql(
        connectionString,
        b => b.MigrationsAssembly("Retaguarda.Persistencia")
    )
);

// Exemplo para MySQL
services.AddDbContext<Retaguarda.Persistencia.MYSQL.ApplicationDbContext>(
    options => options.UseMySql(
        connectionString,
        new MySqlServerVersion(new Version(8, 0, 0)),
        b => b.MigrationsAssembly("Retaguarda.Persistencia")
    )
);
```

#### 3. **Variável de Ambiente: `Persistence__Provider`**

Para alterar o provedor em runtime (sem editar `appsettings.json`), use:

```powershell
# PowerShell
$env:Persistence__Provider = "MySql"
dotnet run

# Ou CMD
set Persistence__Provider=MySql
dotnet run
```

#### Como Alternar Entre PostgreSQL e MySQL

**Desenvolvimento (PostgreSQL - padrão):**
```powershell
cd src\retaguarda\Api
dotnet run
# Conectará automaticamente a PostgreSQL via "DefaultConnection"
```

**Desenvolvimento (MySQL):**
```powershell
cd src\retaguarda\Api
$env:Persistence__Provider = "MySql"
dotnet run
# Conectará a MySQL via "MySql" connection string
```

**Migrations (PostgreSQL):**
```powershell
cd src\retaguarda\Api
dotnet ef migrations add "20260812_120000_DescricaoMigracao" `
  -p ..\Persistencia `
  -s . `
  --context Retaguarda.Persistencia.POSTGRESQL.ApplicationDbContext `
  -o POSTGRESQL\Migracoes
```

**Migrations (MySQL):**
```powershell
cd src\retaguarda\Api
$env:Persistence__Provider = "MySql"
dotnet ef migrations add "20260812_120000_DescricaoMigracao" `
  -p ..\Persistencia `
  -s . `
  --context Retaguarda.Persistencia.MYSQL.ApplicationDbContext `
  -o MYSQL\Migracoes
```

---

### Estratégia 1: Produção com Banco Existente (Sem recriação)

**Quando usar:** Sistema já está em produção, há dados críticos, e você precisa adicionar novas features.

**Passos (PostgreSQL):**

1. **Criar uma nova migration:**
   ```powershell
   cd src\retaguarda\Api
   $timestamp = Get-Date -Format "yyyyMMdd_HHmmss"
   dotnet ef migrations add "$timestamp`_DescritivoDaMudanca" `
     -p ..\Persistencia `
     -s . `
     --context Retaguarda.Persistencia.POSTGRESQL.ApplicationDbContext `
     -o POSTGRESQL\Migracoes
   ```

2. **Revisar o arquivo gerado:**
   ```powershell
   cat "src\retaguarda\Persistencia\POSTGRESQL\Migracoes\$timestamp`_*.cs"
   ```

3. **Testar em ambiente de staging:**
   ```powershell
   # Backup do banco de staging
   pg_dump -U postgres grp_banco_01_staging > backup_staging_$(Get-Date -Format "yyyyMMdd_HHmmss").sql
   
   # Aplicar migration
   dotnet ef database update `
     -p ..\Persistencia `
     -s . `
     --context Retaguarda.Persistencia.POSTGRESQL.ApplicationDbContext
   ```

4. **Fazer backup do banco de produção ANTES de aplicar:**
   ```powershell
   pg_dump -U postgres -h prod-server grp_banco_01 > backup_producao_$(Get-Date -Format "yyyyMMdd_HHmmss").sql
   ```

5. **Aplicar a migration em produção (PostgreSQL):**
   ```powershell
   dotnet ef database update `
     -p ..\Persistencia `
     -s . `
     --context Retaguarda.Persistencia.POSTGRESQL.ApplicationDbContext `
     --connection "Host=prod-server;Port=5432;Database=grp_banco_01;Username=admin;Password=***"
   ```

**Passos (MySQL):**

1. **Criar uma nova migration:**
   ```powershell
   cd src\retaguarda\Api
   $env:Persistence__Provider = "MySql"
   $timestamp = Get-Date -Format "yyyyMMdd_HHmmss"
   dotnet ef migrations add "$timestamp`_DescritivoDaMudanca" `
     -p ..\Persistencia `
     -s . `
     --context Retaguarda.Persistencia.MYSQL.ApplicationDbContext `
     -o MYSQL\Migracoes
   ```

2. **Revisar o arquivo gerado:**
   ```powershell
   cat "src\retaguarda\Persistencia\MYSQL\Migracoes\$timestamp`_*.cs"
   ```

3. **Testar em ambiente de staging:**
   ```powershell
   # Backup do banco de staging
   mysqldump -u root -h staging-server grp_banco_01_staging > backup_staging_$(Get-Date -Format "yyyyMMdd_HHmmss").sql
   
   # Aplicar migration
   $env:Persistence__Provider = "MySql"
   dotnet ef database update `
     -p ..\Persistencia `
     -s . `
     --context Retaguarda.Persistencia.MYSQL.ApplicationDbContext
   ```

4. **Fazer backup do banco de produção ANTES de aplicar:**
   ```powershell
   mysqldump -u root -h prod-server grp_banco_01 > backup_producao_$(Get-Date -Format "yyyyMMdd_HHmmss").sql
   ```

5. **Aplicar a migration em produção (MySQL):**
   ```powershell
   $env:Persistence__Provider = "MySql"
   dotnet ef database update `
     -p ..\Persistencia `
     -s . `
     --context Retaguarda.Persistencia.MYSQL.ApplicationDbContext `
     --connection "Server=prod-server;Port=3306;Database=grp_banco_01;User=admin;Password=***"
   ```

### Estratégia 2: Produção com Traços (Adicionar Campos Multi-Tenant)

**Quando usar:** Você precisa adicionar novos campos a TODAS as tabelas para suportar novos requisitos multi-tenant.

**Passos (PostgreSQL):**

1. **Criar uma nova versão do Traço** em `src/retaguarda/Persistencia/TracosPadrao/`:
   ```csharp
   public class MultilocatarioEntidadeTracosV2 : ITracoMigracao
   {
       public string Nome => "MultilocatarioV2";
       public string Versao => "2.0";
       public string Descricao => "Adiciona campos de auditoria estendida";

       public void AplicarColunas(MigrationBuilder migrationBuilder, string nomeTabela)
       {
           // Adicione ALTER TABLE com IF NOT EXISTS
           // Veja MultilocatarioEntidadeTracosV1.cs como exemplo
       }
   }
   ```

2. **Criar migration que aplica o novo Traço (PostgreSQL):**
   ```powershell
   cd src\retaguarda\Api
   $timestamp = Get-Date -Format "yyyyMMdd_HHmmss"
   dotnet ef migrations add "$timestamp`_AplicarTracosV2" `
     -p ..\Persistencia `
     -s . `
     --context Retaguarda.Persistencia.POSTGRESQL.ApplicationDbContext `
     -o POSTGRESQL\Migracoes
   ```

3. **Editar o arquivo da migration manualmente para aplicar o Traço:**
   ```csharp
   protected override void Up(MigrationBuilder migrationBuilder)
   {
       var traco = new MultilocatarioEntidadeTracosV2();
       traco.ObterSQLBrutoPorTabela(); // ou usar AplicarColunas()
       
       migrationBuilder.Sql(/* SQL gerado pelo traço */);
   }
   ```

4. **Aplicar em staging:**
   ```powershell
   dotnet ef database update `
     -p ..\Persistencia `
     -s . `
     --context Retaguarda.Persistencia.POSTGRESQL.ApplicationDbContext
   ```

**Passos (MySQL):**

1. **Criar migration que aplica o novo Traço (MySQL):**
   ```powershell
   cd src\retaguarda\Api
   $env:Persistence__Provider = "MySql"
   $timestamp = Get-Date -Format "yyyyMMdd_HHmmss"
   dotnet ef migrations add "$timestamp`_AplicarTracosV2" `
     -p ..\Persistencia `
     -s . `
     --context Retaguarda.Persistencia.MYSQL.ApplicationDbContext `
     -o MYSQL\Migracoes
   ```

2. **Editar o arquivo da migration manualmente com SQL específico do MySQL:**
   ```csharp
   protected override void Up(MigrationBuilder migrationBuilder)
   {
       var traco = new MultilocatarioEntidadeTracosV2();
       // Use ObterSQLBrutoPorTabelaMySQL() para MySQL específico
       migrationBuilder.Sql(/* SQL gerado pelo traço para MySQL */);
   }
   ```

3. **Aplicar em staging:**
   ```powershell
   $env:Persistence__Provider = "MySql"
   dotnet ef database update `
     -p ..\Persistencia `
     -s . `
     --context Retaguarda.Persistencia.MYSQL.ApplicationDbContext
   ```

4. **Aplicar em produção (mesmos passos da Estratégia 1, mas para ambos os bancos).**

### Estratégia 3: Migração com Downtime Mínimo (Blue-Green)

**Quando usar:** Sistema crítico, impossível ter downtime significativo.

**Passos (PostgreSQL):**

1. **Criar novo banco "verde" (cópia da produção atual):**
   ```powershell
   # Backup do banco azul (produção)
   pg_dump -U postgres -h prod-server grp_banco_01 > backup_blue.sql
   
   # Restaurar no servidor de staging
   psql -U postgres -h staging-server -c "DROP DATABASE IF EXISTS grp_banco_01_green;"
   psql -U postgres -h staging-server -c "CREATE DATABASE grp_banco_01_green;"
   psql -U postgres -h staging-server -d grp_banco_01_green < backup_blue.sql
   ```

2. **Aplicar migrations ao banco verde:**
   ```powershell
   dotnet ef database update `
     -p ..\Persistencia `
     -s . `
     --context Retaguarda.Persistencia.POSTGRESQL.ApplicationDbContext `
     --connection "Host=staging-server;Port=5432;Database=grp_banco_01_green;Username=admin;Password=***"
   ```

3. **Testar aplicação contra banco verde (mudar connection string temporariamente).**

4. **Trocar DNS/connection string apontando para banco verde:**
   ```json
   // appsettings.json (ambiente de produção)
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=staging-server;Port=5432;Database=grp_banco_01_green;Username=admin;Password=***"
     }
   }
   ```

5. **Manter banco azul (produção antiga) como fallback por 7+ dias.**

**Passos (MySQL):**

1. **Criar novo banco "verde" (cópia da produção atual):**
   ```powershell
   # Backup do banco azul (produção)
   mysqldump -u admin -h prod-server grp_banco_01 > backup_blue.sql
   
   # Restaurar no servidor de staging
   mysql -u admin -h staging-server -e "DROP DATABASE IF EXISTS grp_banco_01_green;"
   mysql -u admin -h staging-server -e "CREATE DATABASE grp_banco_01_green;"
   mysql -u admin -h staging-server grp_banco_01_green < backup_blue.sql
   ```

2. **Aplicar migrations ao banco verde:**
   ```powershell
   $env:Persistence__Provider = "MySql"
   dotnet ef database update `
     -p ..\Persistencia `
     -s . `
     --context Retaguarda.Persistencia.MYSQL.ApplicationDbContext `
     --connection "Server=staging-server;Port=3306;Database=grp_banco_01_green;User=admin;Password=***"
   ```

3. **Testar aplicação contra banco verde.**

4. **Trocar connection string apontando para banco verde:**
   ```json
   // appsettings.json (ambiente de produção)
   {
     "ConnectionStrings": {
       "MySql": "Server=staging-server;Port=3306;Database=grp_banco_01_green;User=admin;Password=***"
     },
     "Persistence": {
       "Provider": "MySql"
     }
   }
   ```

5. **Manter banco azul (produção antiga) como fallback por 7+ dias.**

---

## Procedimento para Recriar Migrations do Zero

### ⚠️ Aviso: Operação Destrutiva

Este procedimento apaga TODAS as migrations e recria do zero. **Use apenas em desenvolvimento ou após decisão arquitetônica.**

### Pré-requisitos

- Ter um backup ou não ligar para os dados atuais
- Estar em ambiente de desenvolvimento
- Ter acesso ao banco de dados

### Passos (PowerShell)

1. **Deletar todas as migrations existentes:**
   ```powershell
   cd "c:\PROJETOS\proposta_arquitetura_grp\src\retaguarda\Persistencia"
   
   # PostgreSQL
   Remove-Item "POSTGRESQL\Migracoes\*" -Recurse -Force
   
   # MySQL
   Remove-Item "MYSQL\Migracoes\*" -Recurse -Force
   
   # Snapshots
   Remove-Item "POSTGRESQL\ApplicationDbContextModelSnapshot.cs" -Force -ErrorAction SilentlyContinue
   Remove-Item "MYSQL\ApplicationDbContextModelSnapshot.cs" -Force -ErrorAction SilentlyContinue
   ```

2. **Deletar banco de dados:**
   ```powershell
   # PostgreSQL
   psql -U postgres -c "DROP DATABASE IF EXISTS grp_banco_01;"
   psql -U postgres -c "CREATE DATABASE grp_banco_01;"
   
   # MySQL
   mysql -u root -e "DROP DATABASE IF EXISTS grp_banco_01; CREATE DATABASE grp_banco_01;"
   ```

3. **Compilar o projeto Persistencia:**
   ```powershell
   cd "c:\PROJETOS\proposta_arquitetura_grp\src\retaguarda\Persistencia"
   dotnet build
   ```

4. **Recriar migrations do zero:**
   ```powershell
   cd "c:\PROJETOS\proposta_arquitetura_grp\src\retaguarda\Api"
   
   # PostgreSQL
   dotnet ef migrations add InitialCreate `
     -p ..\Persistencia `
     -s . `
     --context Retaguarda.Persistencia.POSTGRESQL.ApplicationDbContext `
     -o POSTGRESQL\Migracoes
   
   # MySQL
   dotnet ef migrations add InitialCreate `
     -p ..\Persistencia `
     -s . `
     --context Retaguarda.Persistencia.MYSQL.ApplicationDbContext `
     -o MYSQL\Migracoes
   ```

5. **Aplicar migrations aos bancos:**
   ```powershell
   # PostgreSQL
   dotnet ef database update `
     -p ..\Persistencia `
     -s . `
     --context Retaguarda.Persistencia.POSTGRESQL.ApplicationDbContext
   
   # MySQL
   $env:Persistence__Provider = "MySql"
   dotnet ef database update `
     -p ..\Persistencia `
     -s . `
     --context Retaguarda.Persistencia.MYSQL.ApplicationDbContext
   ```

6. **Verificar que as migrations foram criadas e aplicadas:**
   ```powershell
   # PostgreSQL
   dotnet ef migrations list `
     -p ..\Persistencia `
     -s . `
     --context Retaguarda.Persistencia.POSTGRESQL.ApplicationDbContext
   
   # MySQL
   $env:Persistence__Provider = "MySql"
   dotnet ef migrations list `
     -p ..\Persistencia `
     -s . `
     --context Retaguarda.Persistencia.MYSQL.ApplicationDbContext
   ```

7. **Compilar e testar:**
   ```powershell
   dotnet build
   cd "src\retaguarda\Api"
   dotnet run
   ```

### Verificação Pós-Recriação

Verifique se todos os arquivos e bancos de dados foram criados corretamente:

**Verificar arquivos de migration:**
```powershell
# PostgreSQL
Get-ChildItem "src\retaguarda\Persistencia\POSTGRESQL\Migracoes" -Filter "*.cs"

# MySQL
Get-ChildItem "src\retaguarda\Persistencia\MYSQL\Migracoes" -Filter "*.cs"

# Snapshots (deve haver 1 por contexto)
Test-Path "src\retaguarda\Persistencia\POSTGRESQL\ApplicationDbContextModelSnapshot.cs"
Test-Path "src\retaguarda\Persistencia\MYSQL\ApplicationDbContextModelSnapshot.cs"
```

**Verificar bancos de dados (PostgreSQL):**
```powershell
# Conectar e listar tabelas
psql -U postgres -d grp_banco_01 -c "\dt"

# Verificar se uma tabela tem as colunas esperadas (ex: Usuarios)
psql -U postgres -d grp_banco_01 -c "\d Usuarios"
```

**Verificar bancos de dados (MySQL):**
```powershell
# Conectar e listar tabelas
mysql -u root -e "USE grp_banco_01; SHOW TABLES;"

# Verificar se uma tabela tem as colunas esperadas (ex: Usuarios)
mysql -u root -e "USE grp_banco_01; DESCRIBE Usuarios;"
```

### Possíveis Problemas

| Problema | Causa | Solução |
|----------|-------|--------|
| `No migrations were found` | Arquivo `.csproj` excluindo migrations | Verificar regras `<Compile Remove>` no `.csproj` |
| `DbContext not found` | Namespace ou assembly incorreto | Usar `--context` completo: `Retaguarda.Persistencia.POSTGRESQL.ApplicationDbContext` |
| `Connection refused` | Banco não está rodando | Iniciar PostgreSQL/MySQL antes de aplicar migrations |
| `Permission denied` | Usuário sem privilégios | Use usuário com direitos de criar tabelas (`postgres`, `root`, etc.) |

---

## Padrão de Nomeclatura de Migrations

Use este padrão para manter consistência:

```
{YYYYMMDD_HHMMSS}_{DescricaoCamelCase}

Exemplos:
20260812_120000_CreateUsersTable
20260812_120500_AddEmailToUsers
20260812_121000_ApplyMultilocatarioTraits
```

**Observações:**
- O timestamp garante unicidade e ordem de execução
- A descrição deve ser clara e em PascalCase
- EF Core aplica as migrations em ordem alfabética (timestamp é ordenável) 
