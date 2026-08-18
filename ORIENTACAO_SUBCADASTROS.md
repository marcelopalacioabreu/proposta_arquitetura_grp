# Guia de Subcadastros em Metadados

## Visão Geral

Subcadastros (subtabelas) são componentes genéricos e reutilizáveis que permitem a associação de múltiplas entidades relacionadas dentro de um formulário principal. Exemplo: associar setores e unidades de atuação a um usuário.

## Características

- **Reutilizável**: O mesmo componente pode ser usado em diferentes telas
- **Baseado em Eventos**: Comunica com o formulário principal através de callbacks
- **Atômico**: Os dados dos subcadastros são agregados no payload final de submissão
- **Genérico**: Suporta múltiplos tipos de campos (text, select, checkbox, date, number)
- **Flexível**: Suporte a seleção simples (radio) ou múltipla (checkbox)

## Estrutura de Metadados

### Campo `subcadastros` na ScreenDefinition

```json
{
  "usuarioCadastro": {
    "tipo": "TELA_CADASTRO",
    "titulo": "Cadastro de Usuário",
    "extremidade": "/api/usuarios",
    "itens": [
      // ... campos principais ...
    ],
    "subcadastros": [
      {
        "nome": "atuacao",
        "titulo": "Atuação em Setores e Unidades",
        "endpoint": "/api/organizacao_unidade_setores",
        "chaveLocal": "id",
        "campoArmazenamento": "atuacoes",
        "colunas": [
          // ... definições de colunas ...
        ],
        "selecao": {
          // ... configuração de seleção ...
        },
        "maxLinhas": null
      }
    ]
  }
}
```

### Propriedades do Subcadastro

| Propriedade | Tipo | Descrição |
|---|---|---|
| `nome` | string | Identificador único para rastreamento de eventos |
| `titulo` | string | Título exibido na UI |
| `endpoint` | string | URL para listar opções disponíveis |
| `chaveLocal` | string | Campo que armazena o ID da linha (padrão: "id") |
| `campoArmazenamento` | string | Propriedade que armazena os dados no objeto enviado |
| `colunas` | array | Definições de colunas da subtabela |
| `selecao` | object | Configuração de seleção/marcação de padrão |
| `maxLinhas` | number \| null | Máximo de linhas permitidas (null = ilimitado) |

### Definição de Colunas

```json
{
  "campo": "setorId",
  "label": "Setor",
  "tipo": "select",
  "endpoint": "/api/setores",
  "col": 5,
  "readonly": false,
  "placeholder": "Selecione um setor",
  "enumeracao": "setores"
}
```

#### Propriedades de Coluna

| Propriedade | Tipo | Descrição |
|---|---|---|
| `campo` | string | Nome do campo na entidade |
| `label` | string | Rótulo exibido no cabeçalho |
| `tipo` | string | Tipo de controle: `text`, `select`, `checkbox`, `date`, `number` |
| `col` | number | Largura em colunas (1-12 em grid 12 colunas) |
| `enumeracao` | string | Chave de enumeração ou endpoint para opções |
| `endpoint` | string | URL para carregar opções (select) |
| `readonly` | boolean | Se true, apenas exibição (sem edição) |
| `placeholder` | string | Dica de ajuda ou placeholder |

### Configuração de Seleção

```json
{
  "selecao": {
    "campo": "ehPadrao",
    "label": "Definir como padrão",
    "singleSelecao": true,
    "mergeNoPrincipal": false
  }
}
```

#### Propriedades de Seleção

| Propriedade | Tipo | Descrição |
|---|---|---|
| `campo` | string | Campo que armazena o estado de seleção (boolean) |
| `label` | string | Rótulo do checkbox/radio |
| `singleSelecao` | boolean | Se true, usa radio (uma seleção); false usa checkbox (múltiplo) |
| `mergeNoPrincipal` | boolean | Se true, dados da linha selecionada são mergeados no formulário principal |

## Fluxo de Funcionamento

### 1. Carregamento de Dados

```
┌─────────────────────────────┐
│  TelaCadastro carrega meta  │
└──────────────┬──────────────┘
               │
               ├─> Verifica subcadastros
               │
               └─> Passa definição para SubtabelaCadastro
                   │
                   └─> Carrega opções dos endpoints
```

### 2. Edição de Dados

```
┌────────────────────────────────────┐
│  Usuário interage com subtabela    │
├────────────────────────────────────┤
│ • Adiciona nova linha              │
│ • Remove linha existente           │
│ • Marca como padrão (radio/chk)    │
│ • Edita valores em linha           │
└────────────────────────────────────┘
       │
       └─> onDadosAlterados() dispara evento
           │
           └─> TelaCadastro armazena em subcadastrosRef
```

### 3. Submissão Atômica

```
┌─────────────────────────────────┐
│  Usuário clica em "Salvar"      │
└──────────────┬──────────────────┘
               │
               ├─> construirObjetoFormulario()
               │
               ├─> Coleta dados de campos principais
               │
               ├─> Agrega dados de subcadastros
               │   └─> subcadastrosRef.current[nome]
               │
               └─> Envia objeto completo em uma request
                   {
                     "nome": "João",
                     "email": "joao@example.com",
                     "atuacoes": [
                       {
                         "organizacaoUnidadeId": 1,
                         "setorId": 5,
                         "ehPadrao": true
                       },
                       ...
                     ]
                   }
```

## Exemplo Prático: Atuação de Usuário

### Cenário

Um usuário pode atuar em múltiplos setores dentro de múltiplas unidades da organização. Cada atuação pode ser marcada como padrão.

### Metadados

```json
{
  "subcadastros": [
    {
      "nome": "atuacao",
      "titulo": "Atuação em Setores e Unidades",
      "endpoint": "/api/organizacao_unidade_setores",
      "chaveLocal": "id",
      "campoArmazenamento": "atuacoes",
      "colunas": [
        {
          "campo": "organizacaoUnidadeId",
          "label": "Unidade",
          "tipo": "select",
          "endpoint": "/api/organizacao_unidades",
          "col": 5
        },
        {
          "campo": "setorId",
          "label": "Setor",
          "tipo": "select",
          "endpoint": "/api/setores",
          "col": 5
        },
        {
          "campo": "ehPadrao",
          "label": "Padrão",
          "tipo": "checkbox",
          "col": 2
        }
      ],
      "selecao": {
        "campo": "ehPadrao",
        "label": "Definir como padrão",
        "singleSelecao": true
      },
      "maxLinhas": null
    }
  ]
}
```

### Dados Carregados na Edição

```json
{
  "nome": "João Silva",
  "email": "joao@example.com",
  "atuacoes": [
    {
      "id": 1,
      "organizacaoUnidadeId": 10,
      "setorId": 50,
      "ehPadrao": true
    },
    {
      "id": 2,
      "organizacaoUnidadeId": 10,
      "setorId": 51,
      "ehPadrao": false
    }
  ]
}
```

## Padrões de Reutilização

### Para Qualquer Subcadastro Genérico

1. **Defina a estrutura no JSON** dos metadados
2. **Use nomes descritivos** para `nome` e `campoArmazenamento`
3. **Aproveite a configuração de seleção** conforme necessário
4. **O componente React não precisa de alterações**

### Exemplos Potenciais

- **Endereços de Organização**: Múltiplos endereços (residencial, comercial, etc.)
- **Contatos**: Múltiplos telefones/emails por tipo
- **Documentos**: Múltiplos documentos de identificação
- **Autorizações**: Permissões específicas por módulo
- **Agendamentos**: Múltiplas faixas horárias de disponibilidade

## Validação e Erros

Cada subcadastro pode implementar validação customizada na função `validarLinha()`:

```javascript
const validarLinha = (linha) => {
  const errosLocal = {}
  
  // Exemplo: Validar campos obrigatórios
  if (!linha.setorId) {
    errosLocal.setorId = "Setor é obrigatório"
  }
  
  // Exemplo: Validar duplicatas
  if (linhas.some(l => l.setorId === linha.setorId)) {
    errosLocal.setorId = "Este setor já foi adicionado"
  }
  
  return errosLocal
}
```

## Integração com Enumerações

Se usar enumerações em vez de endpoints dinâmicos:

```json
{
  "campo": "tipoPessoaChave",
  "label": "Tipo",
  "tipo": "select",
  "enumeracao": "pessoa.tipos",
  "col": 4
}
```

O componente buscará as opções em `/api/meta/enumeracoes/pessoa.tipos`.

## Observações Importantes

1. **Atomicidade**: Todos os subcadastros são salvos no mesmo request que o formulário principal
2. **Sem Componentes Específicos**: O padrão genérico evita código duplicado em múltiplas telas
3. **Estado Isolado**: Cada SubtabelaCadastro gerencia seu próprio estado
4. **Eventos Padronizados**: Uso de callbacks `onDadosAlterados` para comunicação
5. **Chaves Locais**: IDs temporários (timestamp) para linhas novas, IDs do banco para existentes
