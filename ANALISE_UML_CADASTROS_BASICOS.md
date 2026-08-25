# Análise UML - Cadastros Básicos (CadastrosBasicosGZ)

**Data da Análise:** 2026-08-25  
**Arquivo Analisado:** `CadastrosBasicosGZ.dia`  
**Diretório:** `C:\Users\sann\Desktop\GRP\Modelagem\000001 - CadastrosBasicos\`

---

## 📋 Resumo Executivo

Este documento descreve a modelagem UML completa do sistema de **Cadastros Básicos** (CadastrosBasicosGZ), contendo **14 classes principais** organizadas em **4 pacotes temáticos**:

1. **Entidades Padrão** - Definição da classe base multilocatário
2. **Cadastros e Registros Básicos** - Organizações, Unidades, Setores, Perfis e Permissões
3. **Usuários e Pessoas** - Usuários, Pessoas Físicas e Jurídicas
4. **Tipificações e Contextos** - Situações e Tipos

---

## 🔄 Hierarquia de Herança

```
MultiLocatarioEntidade (CLASSE BASE)
├── Organizacao
├── OrganizacaoUnidade
├── OrganizacaoSetor
├── Usuario
├── Perfil
├── PerfilPermissao
├── Pessoa
│   ├── PessoaFisica
│   └── PessoaJuridica
├── SituacaoContexto
├── Situacao
├── TipoContexto
└── Tipo
```

---

## 📊 Pacote 1: Entidades Padrão

### Classe: `MultiLocatarioEntidade`

**Tipo:** Classe Base Abstrata  
**Descrição:** Tipo genérico de absolutamente todas as entidades. Considerar que existe um vínculo de extensão em todas as entidades.

**Atributos:**

| Nome | Tipo | Descrição |
|------|------|-----------|
| `id` | `long` | Identificador único numérico |
| `identificadorUnico` | `GUID` | Identificador único global |
| `identificadorUnicoAmigavel` | `string` | Identificador único legível para o usuário |
| `dataInsercao` | `Timestamp` | Data e hora de criação do registro |
| `dataAlteracao` | `TimeStamp` | Data e hora da última alteração |
| `organizacaoId` | `long` | Referência para a Organização |
| `organizacaoUnidadeId` | `long` | Referência para a Unidade Organizacional |
| `setorId` | `long` | Referência para o Setor |
| `ativo` | `bool` | Indica se o registro está ativo |
| `usuarioInsercaoId` | `long` | ID do usuário que criou o registro |
| `usuarioAlteracaoId` | `long` | ID do usuário que fez a última alteração |
| `versao` | `long` | Número de versão para controle de concorrência |

---

## 📊 Pacote 2: Cadastros e Registros Básicos

### Classe: `Organizacao`

**Herança:** Estende `MultiLocatarioEntidade`  
**Descrição:** Representa uma organização no sistema

**Atributos Específicos:**

| Nome | Tipo | Descrição |
|------|------|-----------|
| `nome` | `string` | Nome da organização |
| `codigo` | `string` | Código único da organização |
| `sigla` | `string` | Sigla da organização |
| `organizacaoPaiId` | `long` | Referência para organização pai (hierarquia) |
| `organizacaoRaizId` | `long` | Referência para organização raiz da árvore |
| `hierarquiaCodigo` | `string` | Código hierárquico (e.g., "001.002.003") |

**Relacionamentos:**
- **1 para 1..* com OrganizacaoUnidade** - Uma organização pode ter múltiplas unidades

---

### Classe: `OrganizacaoUnidade`

**Herança:** Estende `MultiLocatarioEntidade`  
**Descrição:** Representa uma unidade dentro de uma organização

**Atributos Específicos:**

| Nome | Tipo | Descrição |
|------|------|-----------|
| `nome` | `string` | Nome da unidade organizacional |
| `codigo` | `string` | Código único da unidade |
| `sigla` | `string` | Sigla da unidade |
| `unidadePaiId` | `long` | Referência para unidade pai (hierarquia) |
| `hierarquiaCodigo` | `string` | Código hierárquico |
| `hierarquiaNome` | `string` | Nome hierárquico (caminho completo) |
| `nivel` | `long` | Nível hierárquico (profundidade na árvore) |

**Relacionamentos:**
- **1 para 1..* com OrganizacaoSetor** - Uma unidade pode ter múltiplos setores
- **1 para 1 com Situacao** - Cada unidade pode estar em uma situação

---

### Classe: `OrganizacaoSetor`

**Herança:** Estende `MultiLocatarioEntidade`  
**Descrição:** Representa um setor dentro de uma unidade organizacional

**Atributos Específicos:**

| Nome | Tipo | Descrição |
|------|------|-----------|
| `codigoHierarquico` | `string` | Código hierárquico do setor |
| `nome` | `string` | Nome do setor |

**Relacionamentos:**
- Parte de `OrganizacaoUnidade` (referência pela foreign key em `setorId`)

---

### Classe: `Perfil`

**Herança:** Estende `MultiLocatarioEntidade`  
**Descrição:** Define um perfil de acesso/permissões no sistema

**Atributos Específicos:**

| Nome | Tipo | Descrição |
|------|------|-----------|
| `nome` | `string` | Nome do perfil |
| `administradorDoSistema` | `bool` | Indica se é perfil de administrador |

**Relacionamentos:**
- **1 para 1..* com PerfilPermissao** - Um perfil pode ter múltiplas permissões
- **1 para 1..* com Usuario** - Um perfil pode ser atribuído a vários usuários

---

### Classe: `PerfilPermissao`

**Herança:** Estende `MultiLocatarioEntidade`  
**Descrição:** Associação de permissões a um perfil

**Atributos Específicos:**

| Nome | Tipo | Descrição |
|------|------|-----------|
| `chave` | `string` | Identificador único da permissão |

**Relacionamentos:**
- Associado a `Perfil` (cardinalidade 1..*)

---

## 📊 Pacote 3: Usuários e Pessoas

### Classe: `Usuario`

**Herança:** Estende `MultiLocatarioEntidade`  
**Descrição:** Representa um usuário do sistema

**Atributos Específicos:**

| Nome | Tipo | Descrição |
|------|------|-----------|
| `nome` | `string` | Nome do usuário |
| `senhaHash` | `string` | Hash da senha (nunca armazenar em texto plano) |
| `email` | `string` | E-mail do usuário |
| `ultimoAcessoOrganizacaoId` | `long` | Organização do último acesso |
| `ultimoAcessoOrganizacaoUnidadeId` | `long` | Unidade do último acesso |
| `ultimoAcessoSetorId` | `long` | Setor do último acesso |

**Relacionamentos:**
- **1 para 1..* com Perfil** - Um usuário pode ter múltiplos perfis
- **1 para 1 com Pessoa** - Um usuário está vinculado a uma Pessoa

---

### Classe: `Pessoa`

**Herança:** Estende `MultiLocatarioEntidade`  
**Descrição:** Classe base para diferentes tipos de pessoas (física ou jurídica)

**Atributos Específicos:**

| Nome | Tipo | Valores |
|------|------|--------|
| `tipoPessoa` | `Enum` | `{FISICA, JURIDICA}` |

**Relacionamentos:**
- **1 para 1 com Usuario** - Cada pessoa tem um usuário associado
- Superclasse de `PessoaFisica` e `PessoaJuridica`

---

### Classe: `PessoaFisica`

**Herança:** Estende `Pessoa`  
**Descrição:** Dados específicos de uma pessoa física

**Atributos Específicos:**

| Nome | Tipo | Descrição |
|------|------|-----------|
| `nome` | `string` | Nome completo (tipo não especificado no UML) |
| `nomeSocial` | `string` | Nome social (tipo não especificado no UML) |
| `cpf` | `string` | CPF (tipo não especificado no UML) |
| `dataNascimento` | `Timestamp` | Data de nascimento (tipo não especificado) |
| `sexo` | `Enum` | Valores: `{FEMININO, MASCULINO}` |
| `estadoCivil` | `Enum` | Valores: `{SOLTEIRA, CASADA}` |
| `nomeMae` | `string` | Nome da mãe (tipo não especificado no UML) |
| `nomePai` | `string` | Nome do pai (tipo não especificado no UML) |
| `pcd` | `bool` | Indica se é Pessoa com Deficiência |
| `dataObito` | `Timestamp` | Data de óbito |

**Relacionamentos:**
- Estende `Pessoa`
- **1 para 1 com Situacao** - Cada pessoa física pode estar em uma situação

---

### Classe: `PessoaJuridica`

**Herança:** Estende `Pessoa`  
**Descrição:** Dados específicos de uma pessoa jurídica (empresa)

**Atributos Específicos:**

| Nome | Tipo | Descrição |
|------|------|-----------|
| `razaoSocial` | `string` | Razão social da empresa |
| `nomeFantasia` | `string` | Nome fantasia da empresa |
| `dataFundacao` | `Timestamp` | Data de fundação |
| `dataExtincao` | `Timestamp` | Data de extinção/encerramento |
| `cnpj` | `string` | CNPJ da empresa |
| `anotacoes` | `string` | Anotações gerais |
| `inscricaoEstadual` | `string` | Inscrição Estadual |
| `inscricaoMunicipal` | `string` | Inscrição Municipal |

**Relacionamentos:**
- Estende `Pessoa`
- **1 para 1 com Situacao** - Cada pessoa jurídica pode estar em uma situação

---

## 📊 Pacote 4: Tipificações e Contextos

### Classe: `SituacaoContexto`

**Herança:** Estende `MultiLocatarioEntidade`  
**Descrição:** Define contextos para situações (container de tipos de situação)

**Atributos Específicos:**

| Nome | Tipo | Descrição |
|------|------|-----------|
| `nome` | `string` | Nome do contexto de situação |
| `descricao` | `string` | Descrição detalhada |

**Relacionamentos:**
- **1 para 1..* com Situacao** - Um contexto pode ter múltiplas situações

---

### Classe: `Situacao`

**Herança:** Estende `MultiLocatarioEntidade`  
**Descrição:** Representa uma situação específica (estado/status de uma entidade)

**Atributos Específicos:**

| Nome | Tipo | Descrição |
|------|------|-----------|
| `codigo` | `string` | Código único da situação |
| `nome` | `string` | Nome da situação |
| `descricao` | `string` | Descrição detalhada |

**Relacionamentos:**
- Associado a `SituacaoContexto` (cardinalidade 1..*)
- Pode ser referenciado por:
  - `Organizacao` (1 para 1)
  - `OrganizacaoUnidade` (1 para 1)
  - `PessoaFisica` (1 para 1)
  - `PessoaJuridica` (1 para 1)

---

### Classe: `TipoContexto`

**Herança:** Estende `MultiLocatarioEntidade`  
**Descrição:** Define contextos para tipos de dados (container de tipos específicos)

**Atributos Específicos:**

| Nome | Tipo | Descrição |
|------|------|-----------|
| `codigo` | `string` | Código único do contexto |
| `nome` | `string` | Nome do contexto |

**Relacionamentos:**
- **1 para 1..* com Tipo** - Um contexto pode ter múltiplos tipos

---

### Classe: `Tipo`

**Herança:** Estende `MultiLocatarioEntidade`  
**Descrição:** Representa um tipo específico dentro de um contexto

**Atributos Específicos:**

| Nome | Tipo | Descrição |
|------|------|-----------|
| `codigo` | `string` | Código único do tipo |
| `nome` | `string` | Nome do tipo |

**Relacionamentos:**
- Associado a `TipoContexto` (cardinalidade 1..*)

---

## 🔗 Mapa de Relacionamentos Completo

| De | Para | Tipo | Cardinalidade | Descrição |
|----|----|------|---|-----------|
| Organizacao | OrganizacaoUnidade | Association | 1:1..* | Uma organização contém múltiplas unidades |
| OrganizacaoUnidade | OrganizacaoSetor | Association | 1:1..* | Uma unidade contém múltiplos setores |
| Usuario | Perfil | Association | 1:1..* | Um usuário tem múltiplos perfis |
| Perfil | PerfilPermissao | Association | 1:1..* | Um perfil contém múltiplas permissões |
| Usuario | Pessoa | Association | 1:1 | Um usuário é vinculado a uma pessoa |
| Pessoa | PessoaFisica | Generalization | 1:1 | Pessoa Física é tipo de Pessoa |
| Pessoa | PessoaJuridica | Generalization | 1:1 | Pessoa Jurídica é tipo de Pessoa |
| Organizacao | Situacao | Association | 1:1 | Organização tem uma situação |
| OrganizacaoUnidade | Situacao | Association | 1:1 | Unidade tem uma situação |
| PessoaFisica | Situacao | Association | 1:1 | Pessoa Física tem uma situação |
| PessoaJuridica | Situacao | Association | 1:1 | Pessoa Jurídica tem uma situação |
| SituacaoContexto | Situacao | Association | 1:1..* | Contexto contém múltiplas situações |
| TipoContexto | Tipo | Association | 1:1..* | Contexto contém múltiplos tipos |
| MultiLocatarioEntidade | * | Generalization | 1:1 | Todas as entidades estendem esta base |

---

## 📝 Observações Importantes

### 1. **Tipos Sem Especificação**
As seguintes propriedades em `PessoaFisica` tiveram seus tipos registrados como vazios no UML:
- `nome` - Inferir como `string`
- `nomeSocial` - Inferir como `string`
- `cpf` - Inferir como `string`
- `dataNascimento` - Inferir como `Timestamp` ou `DateTime`
- `nomeMae` - Inferir como `string`
- `nomePai` - Inferir como `string`

**Recomendação:** Verificar a implementação C# atual para confirmar os tipos corretos.

### 2. **Enumerações Identificadas**
- **Sexo:** `{FEMININO, MASCULINO}`
- **EstadoCivil:** `{SOLTEIRA, CASADA}` (considerar expandir: DIVORCIADA, VIÚVA, UNIÃO_ESTÁVEL)
- **TipoPessoa:** `{FISICA, JURIDICA}`

### 3. **Herança Profunda**
- `PessoaFisica` e `PessoaJuridica` herdam de `Pessoa` que herda de `MultiLocatarioEntidade`
- Isso cria uma cadeia de 3 níveis de herança
- Todas as propriedades da base devem estar presentes nas subclasses

### 4. **Cardinalidades de Associação**
- Associações usando `assoc_type=2` são agregações normais (1:1..*)
- Associações usando `assoc_type=1` indicam relacionamento de navegação simples (1:1)

### 5. **Multilocatarismo**
O sistema é explicitamente multilocatário, com referências a:
- `organizacaoId` - Segregação por organização
- `organizacaoUnidadeId` - Segregação por unidade
- `setorId` - Segregação por setor

---

## 🔄 Comparação com Código C# Atual

Este diagrama UML serve como referência para validar a implementação C# em:

1. **Estrutura de Classes**
   - Verificar se todas as 14 classes estão implementadas
   - Confirmar herança de `MultiLocatarioEntidade`

2. **Atributos e Propriedades**
   - Validar tipos de dados (especialmente em `PessoaFisica`)
   - Confirmar existência de todos os atributos listados

3. **Enumerações**
   - Verificar se as enums estão correctamente definidas
   - Considerar expansão de `EstadoCivil`

4. **Relacionamentos**
   - Confirmar foreign keys correspondentes
   - Validar navegação bidirecional onde apropriado

5. **Padrões de Multilocatarismo**
   - Verificar se os filtros de multi-tenant estão implementados
   - Confirmar constraints de chave estrangeira

---

## 📌 Próximas Ações Recomendadas

1. ✅ **Revisão de Tipos:** Confirmar tipos de dados em `PessoaFisica`
2. ✅ **Expansão de Enums:** Considerar adicionar valores a `EstadoCivil`
3. ✅ **Validação de Cardinalidades:** Verificar se as multiplicidades em banco estão corretas
4. ✅ **Índices de Banco:** Adicionar índices nas foreign keys identificadas
5. ✅ **Documentação C#:** Manter comentários XML alinhados com este diagrama

---

**Documento gerado em:** 2026-08-25  
**Fonte:** Arquivo UML Dia - CadastrosBasicosGZ.dia
