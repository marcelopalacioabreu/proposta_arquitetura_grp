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

## Integração com Elsa (Orquestração de Processos)

O projeto utiliza **Elsa Workflows** (via projeto `Retaguarda.PlanejadorFluxo`) para orquestrar processos de negócio. As atividades Elsa têm acesso completo à camada de domínio do Retaguarda (contexto, repositórios, serviços).

### Arquitetura de Integração

```
┌──────────────────────────────────────┐
│  Elsa Studio (Designer de Workflows) │
│  localhost:6001/studio               │
└──────────────────────────────────────┘
              ↓
┌──────────────────────────────────────┐
│  Servidor Elsa (PlanejadorFluxo)     │
│  localhost:6001                      │
│                                      │
│  • Container DI com dependências:    │
│    - IApplicationDbContext (Scoped)  │
│    - Repositórios (Scoped)           │
│    - Serviços de Domínio (Scoped)    │
│    - Middleware de Tenant-Aware      │
└──────────────────────────────────────┘
              ↓
┌──────────────────────────────────────┐
│  Camada Retaguarda                   │
│  • Persistencia                      │
│  • Repositorios                      │
│  • Servicos                          │
│  • DbContext (PostgreSQL/MySQL)      │
└──────────────────────────────────────┘
```

### Exemplo Prático: CriarOrganizacaoAtividade

A atividade [src/retaguarda/Retaguarda.PlanejadorFluxo/Atividades/CriarOrganizacaoAtividade.cs](src/retaguarda/Retaguarda.PlanejadorFluxo/Atividades/CriarOrganizacaoAtividade.cs) demonstra como:

1. **Declarar uma atividade Elsa:**

```csharp
[Activity("Retaguarda", "Organização", "Cria uma nova organização usando o serviço existente.")]
public class CriarOrganizacaoAtividade : TenantAwareActivity
{
    // Define portas de entrada (Input) e saída (Output)
    [Input(Description = "Nome da organização a ser criada.")]
    public Input<string> Nome { get; set; } = default!;

    [Output(Description = "Id da organização criada.")]
    public Output<long> OrganizacaoId { get; set; } = default!;

    // Método executado pelo Elsa durante o workflow
    protected override async ValueTask ExecuteAsync(ActivityExecutionContext context)
    {
        // ... implementação
    }
}
```

2. **Resolver contexto multilocatário (tenant):**

```csharp
// Herda de TenantAwareActivity que fornece este método:
var (orgId, unidadeId, setorId) = ResolveTenant(context);

// ResolveTenant() busca em ordem:
// 1. Variáveis do workflow (OrganizacaoId, etc)
// 2. Claims HTTP do usuário (organizacaoId, etc)
// 3. Valores nulos se não encontrados

logger.LogInformation("Executando em contexto OrgId={OrgId}", orgId);
```

3. **Injetar e usar dependências:**

```csharp
// Obter serviço obrigatório (lança exceção se não registrado)
var logger = context.GetRequiredService<ILogger<CriarOrganizacaoAtividade>>();
var orgService = context.GetRequiredService<IOrganizacaoServico>();

// Chamar serviço (que automaticamente tem acesso ao IApplicationDbContext)
var organizacaoDto = new OrganizacaoDto { Nome = "Nova Org" };
var criada = await orgService.CriarAsync(organizacaoDto);

// Retornar resultado ao workflow
context.Set(OrganizacaoId, criada.Id);
```

### Estratégia de Injeção de Dependências

As dependências são registradas em **3 camadas** no [src/retaguarda/Retaguarda.PlanejadorFluxo/Program.cs](src/retaguarda/Retaguarda.PlanejadorFluxo/Program.cs):

#### 1. Registrar DbContext e Repositórios

```powershell
# Arquivo: src/retaguarda/Retaguarda.PlanejadorFluxo/Program.cs (linhas ~32-34)

Retaguarda.Persistencia.Configuracao.RegistrarServices(builder.Services, builder.Configuration);
Retaguarda.Repositorios.Configuracao.RegistrarServices(builder.Services, builder.Configuration);
Retaguarda.Servicos.Configuracao.RegistrarServices(builder.Services, builder.Configuration);
```

#### 2. Configuração de Persistência

[src/retaguarda/Persistencia/Configuracao.cs](src/retaguarda/Persistencia/Configuracao.cs) registra:

```csharp
// DbContext concreto (escolhe PostgreSQL ou MySQL por variável de ambiente)
if (provider.Equals("Postgres", StringComparison.OrdinalIgnoreCase))
{
    services.AddDbContext<Retaguarda.Persistencia.POSTGRESQL.ApplicationDbContext>(options =>
        options.UseNpgsql(connectionString, ...));
    
    // Interface de acesso agnóstica
    services.AddScoped<IApplicationDbContext>(sp =>
        sp.GetRequiredService<Retaguarda.Persistencia.POSTGRESQL.ApplicationDbContext>());
}
```

#### 3. Interface IApplicationDbContext

[src/retaguarda/Persistencia/IApplicationDbContext.cs](src/retaguarda/Persistencia/IApplicationDbContext.cs) fornece:

```csharp
public interface IApplicationDbContext
{
    DbSet<Organizacao> Organizacoes { get; set; }
    DbSet<OrganizacaoUnidade> OrganizacaoUnidades { get; set; }
    DbSet<OrganizacaoUnidadeSetor> OrganizacaoUnidadeSetores { get; set; }
    DbSet<Usuario> Usuarios { get; set; }
    // ... 40+ DbSets para todas as entidades
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
```

#### 4. Registro de Repositórios

[src/retaguarda/Repositorios/Configuracao.cs](src/retaguarda/Repositorios/Configuracao.cs) registra todos como **Scoped**:

```csharp
services.AddScoped<IOrganizacaoRepositorio, OrganizacaoRepositorio>();
services.AddScoped<IOrquestracaoFluxoProcessoRepositorio, OrquestracaoFluxoProcessoRepositorio>();
services.AddScoped<IPaisRepositorio, PaisRepositorio>();
// ... 20+ repositórios
```

#### 5. Registro de Serviços de Domínio

[src/retaguarda/Servicos/Configuracao.cs](src/retaguarda/Servicos/Configuracao.cs) registra:

```csharp
services.AddScoped<IOrganizacaoServico, OrganizacaoServico>();
services.AddScoped<IUsuarioServico, UsuarioServico>();
services.AddScoped<RequisicaoUsuario>();      // Contexto do usuário
services.AddScoped<EscopoEmExecucao>();       // Contexto de execução
// ... 20+ serviços
```

### Como o Contexto do Retaguarda é Compartilhado

#### Classe Base: TenantAwareActivity

[src/retaguarda/Retaguarda.PlanejadorFluxo/Atividades/TenantAwareActivity.cs](src/retaguarda/Retaguarda.PlanejadorFluxo/Atividades/TenantAwareActivity.cs) implementa:

```csharp
public abstract class TenantAwareActivity : Activity
{
    /// Resolve (OrganizacaoId, OrganizacaoUnidadeId, SetorId) por:
    /// 1. Variáveis do workflow
    /// 2. Claims do HttpContext.User
    /// 3. Valores nulos se não encontrados
    protected (long? OrganizacaoId, long? OrganizacaoUnidadeId, long? SetorId) 
        ResolveTenant(ActivityExecutionContext context)
    {
        // Implementação que suporta múltiplas versões do Elsa
        // via reflection para WorkflowInstance.Variables
        
        // Se não encontrar em variáveis, busca em claims HTTP
        var httpAccessor = context.GetService<IHttpContextAccessor>();
        var orgId = long.TryParse(
            httpAccessor?.HttpContext?.User?.FindFirst("organizacaoId")?.Value, 
            out var id) ? id : null;
        
        return (orgId, unidadeId, setorId);
    }
}
```

#### Injeção Automática em Serviços

Quando uma atividade injeta um serviço:

```csharp
var orgService = context.GetRequiredService<IOrganizacaoServico>();
```

O `IOrganizacaoServico` já possui `IApplicationDbContext` registrado como **Scoped**:

```csharp
public class OrganizacaoServico : ServicoBase<Organizacao, OrganizacaoDto>, IOrganizacaoServico
{
    public OrganizacaoServico(IOrganizacaoRepositorio repositorio) : base(repositorio)
    {
        // repositorio já tem IApplicationDbContext injetado
    }

    public async Task<OrganizacaoDto> CriarAsync(OrganizacaoDto dto)
    {
        // Usa repositorio.DbContext para operações de BD
        var org = new Organizacao { Nome = dto.Nome, ... };
        await repositorio.AdicionarAsync(org);
        await repositorio.SalvarAlteracoes();
        return EntityToDto(org);
    }
}
```

#### Ciclo de Vida Escoped

- **Por Requisição HTTP:** Cada requisição à API cria novo scope
- **Por Execução de Atividade:** Cada atividade Elsa cria novo scope
- **Disposição Automática:** Ao final, DbContext é descartado e conexão retorna ao pool

Isso garante:
- ✅ Isolamento entre execuções
- ✅ Sem vazamento de estado
- ✅ Eficiência de conexões

### Descoberta Automática de Atividades

No [src/retaguarda/Retaguarda.PlanejadorFluxo/Program.cs](src/retaguarda/Retaguarda.PlanejadorFluxo/Program.cs) (~linha 78):

```csharp
services.AddElsa(elsa => elsa
    // ... configuração de WorkflowManagement, Scheduling, etc
    .AddActivitiesFrom<Program>()  // ← ESCANEIA assembly para atividades
    .AddWorkflowsFrom<Program>()   // ← CARREGA workflows definidos
);
```

Este comando:
1. Escaneia o assembly `Retaguarda.PlanejadorFluxo`
2. Localiza classes que herdam de `Elsa.Workflows.Activity`
3. Lê atributos `[Activity("categoria", "grupo", "descrição")]`
4. Registra automaticamente no container DI do Elsa
5. Torna disponível no Elsa Studio para designer de workflows

### Criando Novas Atividades

Para adicionar uma nova atividade personalizada:

1. **Criar classe em `Atividades/`:**

```csharp
// Arquivo: src/retaguarda/Retaguarda.PlanejadorFluxo/Atividades/MeuProcessoAtividade.cs

[Activity("Retaguarda", "Processos", "Processa dados customizados")]
public class MeuProcessoAtividade : TenantAwareActivity
{
    [Input(Description = "Dados de entrada")]
    public Input<string> Entrada { get; set; } = default!;

    [Output(Description = "Resultado do processamento")]
    public Output<string> Resultado { get; set; } = default!;

    protected override async ValueTask ExecuteAsync(ActivityExecutionContext context)
    {
        var logger = context.GetRequiredService<ILogger<MeuProcessoAtividade>>();
        var (orgId, _, _) = ResolveTenant(context);
        
        var entrada = context.Get(Entrada);
        
        // Sua lógica aqui
        var resultado = ProcessarLogica(entrada, orgId);
        
        context.Set(Resultado, resultado);
        await context.CompleteActivityAsync();
    }

    private string ProcessarLogica(string entrada, long? orgId) { /* ... */ }
}
```

2. **Atividade será descoberta automaticamente** na próxima reinicialização do PlanejadorFluxo
3. **Aparecerá no Elsa Studio** sob a categoria "Retaguarda" > "Processos"

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
