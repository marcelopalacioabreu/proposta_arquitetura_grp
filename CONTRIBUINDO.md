# 🤝 CONTRIBUTING.md - Guia de Contribuição

**Bem-vindo!** Este guia descreve como contribuir para o projeto **Proposta Arquitetura GRP**.

---

## 📋 Índice Rápido

- [Código de Conduta](#código-de-conduta)
- [Como Começar](#como-começar)
- [Branch Strategy](#branch-strategy)
- [Workflow de PR](#workflow-de-pr)
- [Padrões de Código](#padrões-de-código)
- [Commits e Mensagens](#commits-e-mensagens)
- [Code Review](#code-review)
- [Testes](#testes)
- [Documentação](#documentação)

---

## 📖 Código de Conduta

Esperamos profissionalismo de todos os contribuidores. Comportamentos inaceitáveis incluem:

- Assédio de qualquer tipo (gênero, orientação, religião, etc)
- Ataques pessoais ou insultos
- Spam ou self-promotion excessiva
- Divulgação de informações privadas de terceiros

**Violações:** Reportar para `arquitetura@dominio.com`

---

## 🚀 Como Começar

### **1. Fork o Repositório**

```bash
# Clone seu fork
git clone https://github.com/seu-usuario/proposta_arquitetura_grp.git
cd proposta_arquitetura_grp

# Adicionar remote upstream (repo oficial)
git remote add upstream https://github.com/seu-org/proposta_arquitetura_grp.git
```

### **2. Setup Ambiente Local**

```bash
# Ver DESENVOLVIMENTO_INSTRUCOES.md para setup completo
# Resumo:

# Windows (PowerShell 5.1+):
.\iniciar-em-modo-deselvolvimento.ps1

# Linux/Mac:
dotnet restore
npm install --prefix src/interface_grafica/web
dotnet ef database update -p src/retaguarda/Persistencia
```

### **3. Criar Branch para Sua Feature**

```bash
# Atualizar develop local
git fetch upstream
git checkout develop
git pull upstream develop

# Criar branch de feature
git checkout -b feature/descricao-da-feature

# ou para bugfix
git checkout -b bugfix/descricao-do-bug

# ou para docs
git checkout -b docs/descricao-da-doc
```

---

## 🌿 Branch Strategy

Usamos **Git Flow Modificado**:

```
main (produção)
├── v1.0.0, v1.1.0 (releases)
│
develop (próxima release)
├── feature/nova-funcionalidade
├── bugfix/correcao-critica
├── docs/atualizacao-docs
└── refactor/melhorias-codigo
```

### **Branches**

| Branch | Propósito | Vem de | Merge para |
|--------|-----------|--------|-----------|
| `main` | Produção (releases) | `release/x.y.z` | Nunca direto |
| `develop` | Próxima versão | Features + Bugfixes | `release/x.y.z` → `main` |
| `feature/*` | Nova feature | `develop` | `develop` (via PR) |
| `bugfix/*` | Bug crítico | `develop` | `develop` (via PR) |
| `docs/*` | Documentação | `develop` | `develop` (via PR) |
| `refactor/*` | Refatoração | `develop` | `develop` (via PR) |
| `hotfix/*` | Fix urgente em prod | `main` | `main` + `develop` |

### **Exemplo: Hotfix (Bug Crítico em Produção)**

```bash
# Bug encontrado em main v1.0.0
git checkout main
git pull origin main

# Criar hotfix
git checkout -b hotfix/correcao-critica-2024-01

# Corrigir, testar, commit
git commit -m "fix: correção crítica na autenticação"

# Fazer PR para main
# (after review and merge)

# Depois fazer PR de main para develop
git checkout develop
git pull origin develop
git merge main
git push origin develop
```

---

## 🔀 Workflow de PR

### **1. Antes de Começar**

```bash
# Verificar se há issues relacionadas
# Procurar em: https://github.com/seu-org/proposta_arquitetura_grp/issues

# Se nenhuma existe e é feature significativa, criar issue primeiro:
# "Como fazer: [Descrição]" (discussion)
# "Implementar: [Feature]" (feature request)
```

### **2. Implementar**

```bash
# Fazer commits pequenos e lógicos
git add src/retaguarda/Api/Controllers/NewController.cs
git commit -m "feat: novo controller para X"

# Testar localmente
dotnet test

# Push para seu fork
git push origin feature/minha-feature
```

### **3. Criar PR**

Ir para GitHub e criar PR com:

**Título (Obrigatório):**
```
feat: descrição curta em 50 caracteres
```

**Descrição (Obrigatório):**
```markdown
## Descrição
Explicar o que foi implementado e por quê.

## Tipo
- [ ] Bug fix
- [ ] Nova feature
- [ ] Breaking change
- [ ] Documentação
- [ ] Refatoração

## Como Testar?
1. Passo 1
2. Passo 2
3. Verificar que funciona

## Checklist
- [ ] Código segue estilo do projeto
- [ ] Testes adicionados/atualizados
- [ ] Documentação atualizada
- [ ] Sem breaking changes ou descrito em "Breaking Changes"

## Screenshots (se aplicável)
[Adicionar screenshots]

## Issues Relacionadas
Closes #123
```

### **4. Code Review**

Esperar revisão de **pelo menos 1 reviewer**:

- ✅ Aprovação: Merge permitido
- 📝 Comentários: Responder e fazer ajustes
- ❌ Mudanças Solicitadas: Fazer ajustes e re-requisitar review

### **5. Merge**

```bash
# Após aprovações, maintainer faz merge
# (Não fazer merge você mesmo em main/develop!)

# Se seu branch ficar com conflitos:
git fetch origin develop
git rebase origin/develop
# Resolver conflitos
git push origin feature/minha-feature --force-with-lease
```

---

## 📝 Padrões de Código

### **C# / .NET**

```csharp
// Naming Conventions
public class PessoaServico { }           // PascalCase classes
public IList<Pessoa> PessoasAtivas { }   // PascalCase properties
private string _descricao;               // _camelCase private fields
public void ProcessarDados() { }         // PascalCase methods
var minhaVariavel = "";                  // camelCase local variables

// Async/Await
public async Task<IEnumerable<Pessoa>> ObterAsync()
{
    return await _pessoaRepositorio.ListarAsync();
}

// Null checks
public void Processar(Pessoa pessoa)
{
    ArgumentNullException.ThrowIfNull(pessoa);
    // ...
}

// Try-catch (específico)
try
{
    // ...
}
catch (DbUpdateException ex)
{
    _logger.LogError(ex, "Erro ao salvar BD");
    throw;
}

// Logging
_logger.LogInformation("Processando organização: {OrganizacaoId}", org.Id);
_logger.LogError("Falha crítica: {Message}", ex.Message);
```

### **JavaScript / TypeScript (Frontend)**

```typescript
// Use const por padrão
const API_URL = "https://api.dominio.com";

// Functions
function minhaFuncao(parametro: string): void { }
const minhaFuncao = async (id: number) => { };

// React components
function MinhaComponente({ prop }: Props) {
  return <div>{prop}</div>;
}

// Error handling
try {
  const data = await fetch(url);
} catch (error) {
  console.error("Erro ao buscar dados:", error);
}
```

### **SQL Migrations**

```sql
-- Migrations devem ser versionadas: 20260814_AddPessoaTable.sql

-- PostgreSQL
BEGIN;

CREATE TABLE pessoa (
    id BIGSERIAL PRIMARY KEY,
    nome VARCHAR(255) NOT NULL,
    email VARCHAR(255) UNIQUE NOT NULL,
    created_at TIMESTAMP DEFAULT NOW()
);

CREATE INDEX idx_pessoa_email ON pessoa(email);

COMMIT;

-- Rollback:
-- DROP TABLE pessoa CASCADE;
```

---

## 💬 Commits e Mensagens

Usamos **Conventional Commits**:

```
<tipo>(<escopo>): <assunto>

<corpo>

<rodapé>
```

### **Tipos**

- `feat`: Nova feature
- `fix`: Correção de bug
- `docs`: Mudanças em documentação
- `style`: Formatação, sem alteração lógica
- `refactor`: Refatoração de código
- `perf`: Melhoria de performance
- `test`: Adição/atualização de testes
- `chore`: Atualizações de deps, build, etc

### **Exemplos**

```bash
feat(auth): implementar refresh token

fix(api): corrigir validação de CPF

docs(deployment): atualizar instruções de produção

refactor(repositories): melhorar query performance

test(auth): adicionar testes de login

perf(api): cachear respostas de lookup

chore(deps): atualizar dotnet para 9.0.1
```

### **Commits Ruins ❌ vs Bons ✅**

| Ruim | Bom |
|------|-----|
| `fix typo` | `fix(docs): corrigir typo em README` |
| `update stuff` | `refactor(api): melhorar estrutura de pastas` |
| `big changes` | `feat(auth): implementar 2FA via TOTP` |
| `wip` | `feat(api): draft de novo endpoint (WIP)` |

---

## 🔍 Code Review

### **Checklist para Reviewers**

- [ ] Código segue padrões do projeto?
- [ ] Testes foram adicionados/atualizados?
- [ ] Não há lógica duplicada?
- [ ] Performance é adequada?
- [ ] Mensagens de erro são claras?
- [ ] Documentação está atualizada?
- [ ] Sem console.log / Debug.WriteLine?
- [ ] Sem secrets/passwords no código?

### **Como Revisar**

```markdown
# Comentário Positivo
```suggestion
// Melhor usar async/await aqui
var resultado = await repository.ObterAsync();
```
Isso fica mais consistente com o resto do código.

# Comentário Crítico
🔴 **Requer Mudança:** Esta função tem complexity muito alta. 
Sugerir quebrar em funções menores.

# Sugestão Menor
💡 **Sugestão:** Considerar usar `ArgumentNullException.ThrowIfNull()` para validação.
```

---

## ✅ Testes

### **Escrever Testes Para Suas Changes**

```csharp
// xUnit example
public class PessoaServicoTests
{
    [Fact]
    public async Task ObterAsync_ComIdValido_RetornaPessoa()
    {
        // Arrange
        var pessoa = new Pessoa { Id = 1, Nome = "João" };
        var mockRepo = new Mock<IPessoaRepositorio>();
        mockRepo.Setup(r => r.ObterAsync(1)).ReturnsAsync(pessoa);
        
        var servico = new PessoaServico(mockRepo.Object);
        
        // Act
        var resultado = await servico.ObterAsync(1);
        
        // Assert
        Assert.NotNull(resultado);
        Assert.Equal("João", resultado.Nome);
    }
}
```

### **Rodar Testes Localmente**

```bash
# Todos os testes
dotnet test

# Apenas um projeto
dotnet test src/retaguarda/Retaguarda.Tests/Retaguarda.Tests.csproj

# Com cobertura
dotnet test --collect:"XPlat Code Coverage"

# Modo watch
dotnet watch test
```

### **Coverage Mínimo**

- **Target:** 80% de cobertura em lógica crítica
- **Crítica:** Autenticação, autorização, processamento de dados
- **Aceitável:** UI, controllers básicos, mappers

---

## 📚 Documentação

### **Quando Documentar**

- [ ] Toda feature tem descrita sua função em README
- [ ] APIs têm XML comments (`/// <summary>`)
- [ ] Configurações têm exemplo em `.env.example`
- [ ] Migrations têm descrição do que fazem
- [ ] Fluxos complexos têm diagrama em Mermaid

### **Exemplo: XML Comments**

```csharp
/// <summary>
/// Obtém uma pessoa pelo ID.
/// </summary>
/// <param name="id">ID da pessoa</param>
/// <returns>Dados da pessoa ou null se não encontrada</returns>
/// <exception cref="ArgumentException">Se ID é inválido</exception>
public async Task<PessoaDto?> ObterAsync(long id)
{
    ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id);
    return await _repositorio.ObterAsync(id);
}
```

### **Documentação de Features**

Se adicionar feature significativa, atualizar:

```markdown
- [DESENVOLVIMENTO_INSTRUCOES.md](DESENVOLVIMENTO_INSTRUCOES.md) - Setup
- [AUTENTICACAO_AUTORIZACAO_CONTEXTO.md](AUTENTICACAO_AUTORIZACAO_CONTEXTO.md) - Auth/security
- [DEPLOYMENT.md](DEPLOYMENT.md) - Configs de produção
- [README.md](README.md) - Overview
```

---

## 🐛 Reportando Bugs

Usar GitHub Issues com template:

```markdown
## Descrição
Descrição clara do problema.

## Passos para Reproduzir
1. Fazer X
2. Fazer Y
3. Problema acontece

## Comportamento Esperado
O que deveria acontecer.

## Comportamento Atual
O que realmente acontece.

## Ambiente
- OS: Windows 11 / Ubuntu 22.04 / macOS 14
- .NET: 9.0
- Node: 18+
- Postgres: 14
- Browser: Chrome 120

## Logs
[Adicionar logs relevantes]

## Screenshots
[Se aplicável]
```

---

## 💡 Sugestões de Features

Usar GitHub Discussions:

```markdown
## Descrição
Qual problema resolve?

## Solução Proposta
Como implementar?

## Alternativas
Outras formas de resolver?

## Contexto Adicional
Mais informações?
```

---

## 🚀 Release Process

Apenas maintainers fazem releases:

```bash
# 1. Criar release branch
git checkout -b release/v1.1.0

# 2. Atualizar versão
# - Atualizar version em .csproj files
# - Atualizar CHANGELOG.md
# - Atualizar docs se necessário

# 3. Testar
dotnet build
dotnet test

# 4. Commit final
git commit -m "chore: versão v1.1.0"

# 5. Tag
git tag -a v1.1.0 -m "Release v1.1.0"

# 6. Merge em main e develop
git checkout main
git merge release/v1.1.0
git checkout develop
git merge release/v1.1.0

# 7. Push
git push origin main develop --tags
```

---

## ❓ Dúvidas?

- 💬 Abrir discussion no GitHub
- 📧 Email para `arquitetura@dominio.com`
- 🗣️ Chat no Slack #desenvolvimento

---

**Obrigado por contribuir! 🎉**

