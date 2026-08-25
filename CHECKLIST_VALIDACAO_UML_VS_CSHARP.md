# Checklist de Validação UML vs Código C#

**Objetivo:** Validar se a implementação C# corresponde ao modelo UML exportado

---

## ✅ Classes Base

- [ ] `MultiLocatarioEntidade` implementada como classe abstrata
- [ ] Todos os 12 atributos herdados presentes
- [ ] Versioning implementado (`versao` : `long`)
- [ ] Identificador único (GUID) implementado
- [ ] Identificador amigável implementado

---

## ✅ Pacote: Cadastros Básicos

### Organizacao
- [ ] Herança de `MultiLocatarioEntidade` confirmada
- [ ] Atributo `nome` : `string` ✓
- [ ] Atributo `codigo` : `string` ✓
- [ ] Atributo `sigla` : `string` ✓
- [ ] Atributo `organizacaoPaiId` : `long` ✓
- [ ] Atributo `organizacaoRaizId` : `long` ✓
- [ ] Atributo `hierarquiaCodigo` : `string` ✓
- [ ] Foreign Key para `Situacao` (1:1)
- [ ] Foreign Key de auto-referência para `organizacaoPaiId`

### OrganizacaoUnidade
- [ ] Herança de `MultiLocatarioEntidade` confirmada
- [ ] Atributo `nome` : `string` ✓
- [ ] Atributo `codigo` : `string` ✓
- [ ] Atributo `sigla` : `string` ✓
- [ ] Atributo `unidadePaiId` : `long` ✓
- [ ] Atributo `hierarquiaCodigo` : `string` ✓
- [ ] Atributo `hierarquiaNome` : `string` ✓
- [ ] Atributo `nivel` : `long` ✓
- [ ] Foreign Key para `Situacao` (1:1)
- [ ] Foreign Key de auto-referência para `unidadePaiId`

### OrganizacaoSetor
- [ ] Herança de `MultiLocatarioEntidade` confirmada
- [ ] Atributo `codigoHierarquico` : `string` ✓
- [ ] Atributo `nome` : `string` ✓

### Perfil
- [ ] Herança de `MultiLocatarioEntidade` confirmada
- [ ] Atributo `nome` : `string` ✓
- [ ] Atributo `administradorDoSistema` : `bool` ✓
- [ ] Relacionamento 1:N com `PerfilPermissao`
- [ ] Relacionamento 1:N com `Usuario`

### PerfilPermissao
- [ ] Herança de `MultiLocatarioEntidade` confirmada
- [ ] Atributo `chave` : `string` ✓
- [ ] Foreign Key para `Perfil`

---

## ✅ Pacote: Usuários e Pessoas

### Usuario
- [ ] Herança de `MultiLocatarioEntidade` confirmada
- [ ] Atributo `nome` : `string` ✓
- [ ] Atributo `senhaHash` : `string` ✓
- [ ] Atributo `email` : `string` ✓
- [ ] Atributo `ultimoAcessoOrganizacaoId` : `long` ✓
- [ ] Atributo `ultimoAcessoOrganizacaoUnidadeId` : `long` ✓
- [ ] Atributo `ultimoAcessoSetorId` : `long` ✓
- [ ] Foreign Key para `Pessoa` (1:1)
- [ ] Foreign Key para `Perfil` (1:N - múltiplos perfis?)

### Pessoa
- [ ] Herança de `MultiLocatarioEntidade` confirmada
- [ ] Atributo `tipoPessoa` : `Enum` {FISICA, JURIDICA} ✓
- [ ] Foreign Key para `Usuario` (1:1)
- [ ] Superclasse de `PessoaFisica` e `PessoaJuridica`

### PessoaFisica
- [ ] Herança de `Pessoa` confirmada (2 níveis)
- [ ] Atributo `nome` : `string` (VERIFICAR TIPO NO C#)
- [ ] Atributo `nomeSocial` : `string` (VERIFICAR TIPO NO C#)
- [ ] Atributo `cpf` : `string` (VERIFICAR TIPO NO C#)
- [ ] Atributo `dataNascimento` : `Timestamp` ou `DateTime` (VERIFICAR TIPO NO C#)
- [ ] Atributo `sexo` : `Enum` {FEMININO, MASCULINO} ✓
- [ ] Atributo `estadoCivil` : `Enum` {SOLTEIRA, CASADA} (CONSIDERAR EXPANSÃO)
- [ ] Atributo `nomeMae` : `string` (VERIFICAR TIPO NO C#)
- [ ] Atributo `nomePai` : `string` (VERIFICAR TIPO NO C#)
- [ ] Atributo `pcd` : `bool` ✓
- [ ] Atributo `dataObito` : `Timestamp` ✓
- [ ] Foreign Key para `Situacao` (1:1)

### PessoaJuridica
- [ ] Herança de `Pessoa` confirmada (2 níveis)
- [ ] Atributo `razaoSocial` : `string` ✓
- [ ] Atributo `nomeFantasia` : `string` ✓
- [ ] Atributo `dataFundacao` : `Timestamp` ✓
- [ ] Atributo `dataExtincao` : `Timestamp` ✓
- [ ] Atributo `cnpj` : `string` ✓
- [ ] Atributo `anotacoes` : `string` ✓
- [ ] Atributo `inscricaoEstadual` : `string` ✓
- [ ] Atributo `inscricaoMunicipal` : `string` ✓
- [ ] Foreign Key para `Situacao` (1:1)

---

## ✅ Pacote: Tipificações e Contextos

### SituacaoContexto
- [ ] Herança de `MultiLocatarioEntidade` confirmada
- [ ] Atributo `nome` : `string` ✓
- [ ] Atributo `descricao` : `string` ✓
- [ ] Relacionamento 1:N com `Situacao`

### Situacao
- [ ] Herança de `MultiLocatarioEntidade` confirmada
- [ ] Atributo `codigo` : `string` ✓
- [ ] Atributo `nome` : `string` ✓
- [ ] Atributo `descricao` : `string` ✓
- [ ] Foreign Key para `SituacaoContexto`
- [ ] Foreign Key reversa de `Organizacao`
- [ ] Foreign Key reversa de `OrganizacaoUnidade`
- [ ] Foreign Key reversa de `PessoaFisica`
- [ ] Foreign Key reversa de `PessoaJuridica`

### TipoContexto
- [ ] Herança de `MultiLocatarioEntidade` confirmada
- [ ] Atributo `codigo` : `string` ✓
- [ ] Atributo `nome` : `string` ✓
- [ ] Relacionamento 1:N com `Tipo`

### Tipo
- [ ] Herança de `MultiLocatarioEntidade` confirmada
- [ ] Atributo `codigo` : `string` ✓
- [ ] Atributo `nome` : `string` ✓
- [ ] Foreign Key para `TipoContexto`

---

## 🔍 Pontos Críticos para Validação

### TIPO: Itens não especificados no UML (PessoaFisica)
```csharp
// Verificar tipos reais no código C#:
public string Nome { get; set; }  // ✓ ou ? (nulo)
public string NomeSocial { get; set; }  // ✓ ou ? (nulo)
public string CPF { get; set; }  // ✓ ou ? (nulo)
public DateTime DataNascimento { get; set; }  // Timestamp ou DateTime?
public string NomeMae { get; set; }  // ✓ ou ? (nulo)
public string NomePai { get; set; }  // ✓ ou ? (nulo)
```

### ENUM: EstadoCivil - Valores Incompletos?
```csharp
public enum EstadoCivil
{
    SOLTEIRA = 0,
    CASADA = 1,
    // Faltam: DIVORCIADA, VIÚVA, UNIÃO_ESTÁVEL?
}
```

### MULTILOCATÁRIO: Validar Filtros de Segurança
- [ ] Todos os repositórios filtram por `organizacaoId`?
- [ ] Constraints de chave estrangeira definidas?
- [ ] Índices em `organizacaoId` criados?
- [ ] Índices em `organizacaoUnidadeId` criados?
- [ ] Índices em `setorId` criados?

### HERANÇA: Validar Estratégia
- [ ] TPH (Table Per Hierarchy) ou TPT (Table Per Type)?
- [ ] Discriminador implementado em `Pessoa`?
- [ ] Migrations do EF Core alinhadas?

---

## 📋 Resultado da Validação

| Item | Status | Observações |
|------|--------|-----------|
| Total de Classes | ⚠️ | Verificar se 14 classes estão presentes |
| Atributos | ⚠️ | Tipos em PessoaFisica precisam validação |
| Enumerações | ⚠️ | EstadoCivil pode precisar expansão |
| Relacionamentos | ⚠️ | Verificar cardinalidades no banco |
| Herança | ⚠️ | Estratégia de herança deve ser confirmada |

---

## 🎯 Próximos Passos

1. **Código C# Atual:** Revisar cada entidade em `Retaguarda.Dominio` e `Retaguarda.Persistencia`
2. **Banco de Dados:** Verificar schema do banco via migrations
3. **Correligiosas:** Listar todas as diferenças encontradas
4. **Documentação:** Atualizar diagramas se necessário

---

**Checklist gerado em:** 2026-08-25  
**Referência:** ANALISE_UML_CADASTROS_BASICOS.md
