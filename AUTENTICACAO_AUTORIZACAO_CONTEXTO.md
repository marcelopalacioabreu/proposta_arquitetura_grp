# Autenticação, Autorização e Contexto de Atuação

Este documento descreve como o projeto implementa **autenticação (authentication)**, **autorização (authorization)** e **contexto de atuação (tenant context)** em modo desenvolvimento e produção, com foco na integração com Elsa.

## Visão Geral da Arquitetura

```
┌─────────────────────────────────────────────────────────────┐
│  FRONTEND (localhost:5173)                                  │
│  • Login com credentials (admin/admin)                      │
│  • Recebe JWT token + contexto de atuação                   │
│  • Armazena token em cookie HttpOnly (access_token)         │
│  • Armazena contexto em cookie (atuacao)                    │
└──────────────────┬──────────────────────────────────────────┘
                   │
    GET / POST requests com cookies
                   │
                   ▼
┌─────────────────────────────────────────────────────────────┐
│  API (localhost:5000)                                       │
│  ┌─────────────────────────────────────────────────────────┤
│  │ MIDDLEWARE PIPELINE                                     │
│  │ 1. Authentication (JWT do cookie access_token)          │
│  │ 2. UsuarioMiddleware (extrai usuário autenticado)       │
│  │ 3. AtuacaoMiddleware (extrai OrganizacaoId/etc)         │
│  │    └─ Preenche EscopoEmExecucao                         │
│  │ 4. Authorization (verifica [Authorize])                 │
│  └─────────────────────────────────────────────────────────┤
│                                                             │
│  • Controllers filtram dados por OrganizacaoId (tenant)     │
│  • Reverse proxy para /elsa/* → PlanejadorFluxo            │
│  • Adiciona header X-Atuacao ao proxy (NOVO)               │
└──────────────────┬──────────────────────────────────────────┘
                   │ HTTP + Cookie + Header X-Atuacao
                   ▼
┌─────────────────────────────────────────────────────────────┐
│  PLANEJADOR FLUXO / ELSA (localhost:6001)                   │
│  ┌─────────────────────────────────────────────────────────┤
│  │ MIDDLEWARE PIPELINE                                     │
│  │ 1. Authentication (JWT validado)                        │
│  │ 2. UsuarioMiddleware (extrai usuário)                   │
│  │ 3. AtuacaoMiddleware (lê header X-Atuacao)             │
│  │    └─ Preenche EscopoEmExecucao                         │
│  │ 4. ElsaTenantFilterMiddleware (valida tenant)          │
│  │ 5. Authorization                                        │
│  └─────────────────────────────────────────────────────────┤
│                                                             │
│  • Atividades Elsa usam EscopoEmExecucao.OrganizacaoId     │
│  • TenantAwareActivity resolve tenant de variáveis/claims   │
│  • Workflows isolados por OrganizacaoId (novo)            │
│  • Banco compartilhado mas dados filtrados                  │
└─────────────────────────────────────────────────────────────┘
```

## Fluxo de Autenticação

### 1. Login (POST /api/auth/login)

```http
POST http://localhost:5000/api/auth/login
Content-Type: application/json

{
  "usuario": "admin",
  "senha": "admin"
}
```

**Resposta (201 Created):**
```json
{
  "token": "eyJhbGc...",
  "usuario": "admin",
  "organizacaoId": 1,
  "organizacaoUnidadeId": null,
  "setorId": null,
  "expiracao": "2026-08-14T23:00:00Z"
}
```

**Side Effects:**
- 🍪 Cookie `access_token` criado (HttpOnly, Secure em produção)
- 🍪 Cookie `atuacao` criado com contexto: `{"organizacaoId": 1, ...}`
- No frontend, token armazenado em memória para requisições

### 2. Fluxo de Requisições Autenticadas

**DESENVOLVIMENTO:**
```
Frontend (cookie access_token + atuacao)
    ↓
API.AtuacaoMiddleware
    ├─ Lê cookie "atuacao"
    ├─ Preenche EscopoEmExecucao { OrganizacaoId: 1 }
    └─ Armazena em HttpContext.Items["escopo.organizacaoId"]
    ↓
Controllers (filtram por EscopoEmExecucao.OrganizacaoId)
    ↓
Reverse Proxy para /elsa/*
    ├─ Copia cookies (access_token, atuacao)
    ├─ Adiciona header X-Atuacao: {"organizacaoId": 1}  (NOVO)
    └─ Envia para PlanejadorFluxo
    ↓
PlanejadorFluxo.AtuacaoMiddleware
    ├─ Encontra header X-Atuacao
    ├─ Preenche EscopoEmExecucao
    └─ Atividades Elsa usam este contexto
```

## Componentes Principais

### A. AtuacaoMiddleware

**Arquivo:** [src/retaguarda/Api/Middleware/AtuacaoMiddleware.cs](../src/retaguarda/Api/Middleware/AtuacaoMiddleware.cs)

**Responsabilidades:**
1. Ler contexto de tenant de:
   - Cookie `atuacao` (prioridade)
   - Header `X-Atuacao` (fallback)
2. Parse JSON: `{"organizacaoId": 1, "organizacaoUnidadeId": 2, "setorId": 3}`
3. Preencher `EscopoEmExecucao` (escoped service)
4. Armazenar em `HttpContext.Items` para acesso sem referências de projeto

**Estratégia de Parsing:**

```csharp
// Cookie/Header pode estar em 2 formatos:

// Formato 1: JSON
atuacao = {"organizacaoId": 1, "organizacaoUnidadeId": 2}

// Formato 2: Key-Value
atuacao = organizacaoId=1;organizacaoUnidadeId=2
```

### B. EscopoEmExecucao

**Arquivo:** [src/retaguarda/Servicos/EscopoEmExecucao.cs](../src/retaguarda/Servicos/EscopoEmExecucao.cs)

```csharp
public class EscopoEmExecucao
{
    public long? OrganizacaoId { get; set; }
    public long? OrganizacaoUnidadeId { get; set; }
    public long? SetorId { get; set; }
}
```

**Ciclo de Vida:** Scoped (criado por requisição, destruído ao final)

**Uso:**
```csharp
// Em um repositório ou serviço
public class OrganizacaoRepositorio
{
    private readonly IApplicationDbContext _context;
    private readonly EscopoEmExecucao _escopo;

    public OrganizacaoRepositorio(IApplicationDbContext context, EscopoEmExecucao escopo)
    {
        _context = context;
        _escopo = escopo;
    }

    public async Task<List<Organizacao>> ListarAsync()
    {
        // Filtrar automaticamente por OrganizacaoId do usuário
        return await _context.Organizacoes
            .Where(o => o.OrganizacaoId == _escopo.OrganizacaoId)
            .ToListAsync();
    }
}
```

### C. TenantAwareActivity (Elsa)

**Arquivo:** [src/retaguarda/Retaguarda.PlanejadorFluxo/Atividades/TenantAwareActivity.cs](../src/retaguarda/Retaguarda.PlanejadorFluxo/Atividades/TenantAwareActivity.cs)

Classe base para atividades Elsa que precisam resolver contexto de tenant:

```csharp
public abstract class TenantAwareActivity : Activity
{
    protected (long? OrganizacaoId, long? UnidadeId, long? SetorId) 
        ResolveTenant(ActivityExecutionContext context)
    {
        // 1. Tenta ler de variáveis do workflow
        // 2. Fallback para claims do usuário
        // 3. Fallback para HttpContext.Items (via header X-Atuacao)
        
        var httpAccessor = context.GetService<IHttpContextAccessor>();
        var orgId = long.TryParse(
            httpAccessor?.HttpContext?.Items["elsa.organizacaoId"]?.ToString(),
            out var id) ? id : null;
        
        return (orgId, unidadeId, setorId);
    }
}
```

## Desenvolvimento vs. Produção

### Desenvolvimento (localhost)

| Aspecto | Implementação |
|---------|---------------|
| **HTTPS** | ❌ Desabilitado (HTTP) |
| **Cookie Secure** | ❌ Falso (funciona em HTTP) |
| **JWT Key** | 🔑 Padrão: `"change_this_secret_for_prod"` |
| **CORS** | ✅ Permissivo: `AllowAnyOrigin()` |
| **Credenciais Padrão** | ✅ `admin/admin` (seeded no banco) |
| **Isolamento Elsa** | ⚠️ Novo (middleware implementado) |
| **Reverse Proxy** | ✅ API (5000) → PlanejadorFluxo (6001) |
| **Header X-Atuacao** | ✅ Adicionado no proxy (NOVO) |

**Como executar:**
```powershell
cd c:\PROJETOS\proposta_arquitetura_grp
.\iniciar-em-modo-deselvolvimento.ps1
```

**Endpoints:**
- API: `http://localhost:5000`
- Elsa Studio: `http://localhost:6001/studio`
- Frontend: `http://localhost:5173`

### Produção

| Aspecto | Recomendação |
|---------|---|
| **HTTPS** | ✅ **Obrigatório** - Use certificado válido (Let's Encrypt) |
| **Cookie Secure** | ✅ **Obrigatório** - `Secure; HttpOnly; SameSite=Strict` |
| **JWT Key** | ✅ **Obrigatório** - Use 256-bit key segura (não hardcoded) |
| **CORS** | ⚠️ Restritivo - Apenas domínios autorizados |
| **Credenciais Padrão** | ❌ **Desabilitar** - Mudar `admin/admin` |
| **Isolamento Elsa** | ✅ **Crítico** - OrganizacaoId obrigatório em todas as requests |
| **Autenticação Multi-Tenant** | ✅ Validar OrganizacaoId do usuário vs. dados |
| **Secret Management** | ✅ Usar Key Vault / Secrets Manager (não `appsettings.json`) |
| **Logging & Audit** | ✅ Log todas as operações Elsa com OrganizacaoId |

**Variáveis de Ambiente Essenciais:**

```bash
# Autenticação
ASPNETCORE_ENVIRONMENT=Production
Jwt__Key=<64-char-random-hex-key>
Jwt__Issuer=YourOrgName

# Banco de Dados
Persistence__Provider=Postgres  # ou MySql
ConnectionStrings__DefaultConnection=<connection-string>

# Segurança
ASPNETCORE_URLs=https://+:443
ASPNETCORE_HTTPS_PORT=443
ASPNETCORE_Kestrel__Certificates__Default__Path=/etc/certs/cert.pem
ASPNETCORE_Kestrel__Certificates__Default__KeyPath=/etc/certs/key.pem

# Elsa/PlanejadorFluxo
Elsa__ConnectionStrings__DefaultConnection=<connection-string>

# CORS
Cors__AllowedOrigins=https://yourdomain.com
```

## Isolamento Multilocatário (Tenant Isolation)

### Estratégia de Isolamento

```
┌─────────────────────────────────┐
│  NÍVEL 1: Autenticação          │
│  ✅ JWT Token com OrganizacaoId │
└─────────────┬───────────────────┘
              ▼
┌─────────────────────────────────┐
│  NÍVEL 2: Contexto de Execução  │
│  ✅ EscopoEmExecucao.OrgId      │
│  ✅ HttpContext.Items           │
└─────────────┬───────────────────┘
              ▼
┌─────────────────────────────────┐
│  NÍVEL 3: Filtro de Dados       │
│  ✅ Controllers filtram queries │
│  ✅ Repositórios filtram por OrgId
│  ✅ DbContext filtra automático │
└─────────────┬───────────────────┘
              ▼
┌─────────────────────────────────┐
│  NÍVEL 4: Isolamento Elsa       │
│  ✅ Header X-Atuacao propagado  │
│  ✅ ElsaTenantFilterMiddleware   │
│  ✅ OrganizacaoId em workflows  │
│  ✅ TenantAwareActivity resolve  │
└─────────────────────────────────┘
```

### Validações de Segurança

**Em Controllers:**
```csharp
[ApiController]
[Route("api/[controller]")]
[Authorize]  // ← JWT obrigatório
public class OrganizacoesController
{
    private readonly EscopoEmExecucao _escopo;

    [HttpGet("{id}")]
    public async Task<ActionResult<OrganizacaoDto>> GetById(long id)
    {
        var org = await _organizacaoRepository.GetByIdAsync(id);
        
        // ✅ Validar: usuário só vê dados da sua organização
        if (org.OrganizacaoId != _escopo.OrganizacaoId)
        {
            return Forbid("Acesso negado: organização diferente");
        }
        
        return Ok(org);
    }
}
```

**Em Atividades Elsa:**
```csharp
[Activity("Retaguarda", "Organização", "Cria nova organização")]
public class CriarOrganizacaoAtividade : TenantAwareActivity
{
    protected override async ValueTask ExecuteAsync(ActivityExecutionContext context)
    {
        var (orgId, _, _) = ResolveTenant(context);
        
        // ✅ Validar: atividade executa apenas no contexto de tenant
        if (!orgId.HasValue)
        {
            throw new InvalidOperationException("OrganizacaoId obrigatório para criar organização");
        }
        
        // Criar organização no contexto de orgId
        var dto = new OrganizacaoDto { Nome = "Nova Org" };
        var result = await _orgService.CriarAsync(dto);
        
        context.Set(OrganizacaoId, result.Id);
    }
}
```

## Múltiplos Frontends (Microfrontend & Microserviços)

Esta arquitetura de autenticação compartilhada viabiliza uso de **múltiplos frontends independentes** (Microfrontend/Micro Apps):

### Padrão: Contexto Centralizado + APIs Agnósticas

```
┌─────────────────────────────────────────────────────────────┐
│  FRONTEND 1: Web (Vite React)          FRONTEND 2: Mobile   │
│  • localhost:5173                      • Native/Flutter      │
│  • Cookie access_token + atuacao       • Header Auth        │
└────────────────┬────────────────────────────┬───────────────┘
                 │                            │
                 └────────────┬────────────────┘
                              │
                    ALL requests to API
                    + Cookie/Header Atuacao
                              │
                              ▼
┌─────────────────────────────────────────────────────────────┐
│  API GATEWAY (localhost:5000)                               │
│  ✅ Validação centralizada de OrganizacaoId                 │
│  ✅ Contexto compartilhado entre frontends                  │
│  ✅ Header X-Atuacao propagado para microsserviços         │
└────────────┬───────────────────────┬───────────────────────┘
             │                       │
    ┌────────▼────────┐   ┌────────▼────────┐
    │  Microserviço 1 │   │  Microserviço 2 │
    │  (Org Service)  │   │  (Elsa Flows)   │
    └────────────────┘   └────────────────┘
```

### Benefícios:

- ✅ **Separação de Responsabilidades:** Cada frontend gerencia sua UI
- ✅ **Reutilização de Contexto:** Todos compartilham `OrganizacaoId`
- ✅ **Escalabilidade:** Fácil adicionar novos frontends/microsserviços
- ✅ **Coexistência:** Web + Mobile + Desktop no mesmo tenant
- ✅ **Headers Agnósticos:** Qualquer cliente (web, mobile, IoT) passa `X-Atuacao`

### Exemplo: Frontend Mobile + Elsa

```
Mobile App (Flutter)
    │
    ├─ POST /api/auth/login
    │   ← Recebe: JWT token + organizacaoId
    │
    ├─ GET /api/organizacoes
    │   Header: Authorization: Bearer <token>
    │   ← Retorna: Lista de organizações (filtrada por tenant)
    │
    └─ POST /elsa/api/workflow-instances
        Header: Authorization: Bearer <token>
        Header: X-Atuacao: {"organizacaoId": 1}
        ← Executa: Workflow no contexto de tenant
```

O middleware de contexto valida que:
- Token é válido
- OrganizacaoId do token = OrganizacaoId da requisição
- Dados retornados = apenas da organização do usuário

---

## Próximos Passos

### Desenvolvimento:
1. ✅ Aplicar migrations para adicionar `OrganizacaoId` ao schema Elsa
   ```powershell
   cd src\retaguarda\Api
   dotnet ef database update -p ..\Persistencia -s . --context Retaguarda.Persistencia.POSTGRESQL.ApplicationDbContext
   ```

2. ✅ Testar isolamento entre organizações no Elsa Studio

3. ✅ Validar que workflows de ORG A não aparecem em ORG B

### Produção:
1. ✅ Configurar HTTPS com certificado
2. ✅ Definir JWT Key segura (via secrets manager)
3. ✅ Habilitar validação rigorosa de tenant
4. ✅ Implementar audit logging de acesso a workflows
5. ✅ Testar com múltiplas organizações

---

## Referências

- [DESENVOLVIMENTO_INSTRUCOES.md](../DESENVOLVIMENTO_INSTRUCOES.md) - Como rodar em desenvolvimento
- [Integração com Elsa](../DESENVOLVIMENTO_INSTRUCOES.md#integração-com-elsa-orquestração-de-processos) - Arquitetura de atividades Elsa
- `/memories/repo/elsa-tenant-isolation-analysis.md` - Análise técnica de isolamento (salva em memória)
