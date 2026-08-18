# Diagramas de Arquitetura - Subcadastros Genéricos

## 1. Fluxo Geral de Dados

```mermaid
graph TD
    A["📋 JSON de Metadados<br/>(usuarios/cadastro.json)"] 
    B["🎯 ScreenDefinition<br/>(itens + subcadastros)"]
    C["📱 TelaCadastro<br/>(React)"]
    D["📊 SubtabelaCadastro<br/>(Genérico)"]
    E["💾 subcadastrosRef<br/>(armazena dados)"]
    F["🚀 Submissão Atômica<br/>(POST/PUT)"]
    G["🛢️ Banco de Dados"]
    
    A -->|carrega| B
    B -->|passa config| C
    C -->|renderiza N| D
    D -->|evento| E
    E -->|agrega| F
    F -->|persiste| G
```

## 2. Estrutura de Subcadastro (JSON)

```mermaid
graph LR
    A["subcadastro"]
    B["nome: atuacao"]
    C["titulo: 'Atuação...'"]
    D["endpoint: /api/setores"]
    E["colunas: []"]
    F["selecao: {campo, label}"]
    G["maxLinhas: null"]
    
    A --> B & C & D & E & F & G
    E --> E1["organizacaoUnidadeId<br/>select"]
    E --> E2["setorId<br/>select"]
    E --> E3["ehPadrao<br/>checkbox"]
```

## 3. Ciclo de Vida de SubtabelaCadastro

```mermaid
sequenceDiagram
    participant User as 👤 Usuário
    participant React as ⚛️ React
    participant API as 🌐 API
    participant Ref as 🔖 Ref
    
    User->>React: Abre formulário
    React->>API: GET /api/setores
    API-->>React: [setor1, setor2, ...]
    React->>React: Renderiza subtabela
    
    User->>React: Adiciona linha
    React->>React: Valida
    React->>Ref: Armazena em ref
    React->>React: Re-renderiza tabela
    
    User->>React: Clica Salvar
    React->>Ref: Lê subcadastrosRef.current
    Ref-->>React: {atuacoes: [...]}
    React->>API: POST {..., atuacoes: [...]}
    API-->>React: ✅ 201 Created
```

## 4. Arquitetura de Enumerações

```mermaid
graph TB
    A["📁 pessoa_tipos.json<br/>(Valor + Texto)"]
    B["🔧 Enumeracao.cs<br/>(Base Abstrata)"]
    C["🎭 TipoPessoa.cs<br/>(Instância Concreta)"]
    D["📤 DTO"]
    E["🎨 Frontend"]
    
    A -->|define| B
    B -->|implementa| C
    C -->|usa| D
    D -->|retorna| E
    
    E -->|exibe| E1["Pessoa Física<br/>(texto)"]
    E -->|envia| E2["F<br/>(valor)"]
```

## 5. Componentes React

```mermaid
graph LR
    A["TelaCadastro"]
    B["renderCampo()"]
    C["renderSubcadastro()"]
    D["SubtabelaCadastro"]
    E["Linha"]
    F["Campo"]
    
    A -->|para cada| B
    A -->|para cada| C
    C -->|passa config| D
    D -->|renderiza| E
    E -->|renderiza| F
    F -->|texto, select,<br/>checkbox, date, number| F
```

## 6. Fluxo de Validação e Submissão

```mermaid
graph TD
    A["construirObjetoFormulario()"]
    B["Coleta FormData"]
    C["Normaliza checkboxes"]
    D["Busca subcadastrosRef"]
    E["Agrega atuacoes"]
    F["Inclui campos da URL"]
    G["Valida schema"]
    H{Válido?}
    I["POST/PUT"]
    J["Erro"]
    
    A --> B --> C --> D --> E --> F --> G --> H
    H -->|sim| I
    H -->|não| J
    J -->|exibe| J1["Mensagem de erro<br/>(setErrors)"]
```

## 7. Estados e Eventos SubtabelaCadastro

```mermaid
graph LR
    A["linhas: []"]
    B["linhaEmEdicao"]
    C["novaLinha: {}"]
    D["opcoes: {}"]
    E["erros: {}"]
    
    A -->|gerencia| A1["Adicionar"]
    A -->|gerencia| A2["Remover"]
    A -->|gerencia| A3["Marcar padrão"]
    
    B -->|rastreia| B1["Edição inline"]
    C -->|buffer| C1["Validar entrada"]
    D -->|cache| D1["Opções de select"]
    E -->|exibe| E1["Feedback de erro"]
    
    A -->|dispara| A4["onDadosAlterados()"]
```

## 8. Padrão de Reutilização

```mermaid
graph TD
    A["Novo Subcadastro<br/>Necessário?"]
    B["Defina no JSON<br/>de Metadados"]
    C["Configure colunas,<br/>selecao, endpoint"]
    D["Nenhuma mudança<br/>de código React/C#"]
    E["Pronto para usar!"]
    
    A -->|sim| B --> C --> D --> E
```

## 9. Integração com Metadados

```mermaid
graph TB
    META["📋 Meta (carregado)"]
    CAMPOS["itens: [campos principais]"]
    SUBCAD["subcadastros: [subtabelas]"]
    ENUM["enumeracoes: [definições]"]
    
    META --> CAMPOS
    META --> SUBCAD
    META --> ENUM
    
    CAMPOS -->|renderiza| TELA["TelaCadastro"]
    SUBCAD -->|renderiza| SUB["SubtabelaCadastro x N"]
    ENUM -->|carrega| API["GET /meta/enumeracoes"]
    
    TELA -->|agrega| PAYLOAD["Objeto Final"]
    SUB -->|agrega| PAYLOAD
```

## 10. Atomicidade de Transação

```mermaid
graph LR
    A["FormData<br/>Complete"]
    B["Construir<br/>Objeto"]
    C["Incluir<br/>Subcadastros"]
    D["Validar<br/>Schema"]
    E["POST/PUT<br/>Único"]
    F["BD:<br/>Transação"]
    G["✅ Sucesso<br/>ou ❌ Falha<br/>Completa"]
    
    A --> B --> C --> D --> E --> F --> G
```

---

## Legenda de Componentes

| Símbolo | Significado |
|---------|------------|
| 📋 | Arquivo de Configuração / JSON |
| 🎯 | Classe / Contrato |
| 📱 | Componente React |
| 💾 | Armazenamento / Referência |
| 🚀 | Operação / Ação |
| 🛢️ | Banco de Dados |
| 🔧 | Base / Interface |
| 🎭 | Implementação Concreta |
| 📤 | Transferência de Dados |
| 🎨 | Interface Visual |
| 📊 | Dados / Estado |
| 🌐 | API / Serviço |
| 👤 | Ator / Usuário |
| ⚛️ | React |
| 🔖 | Referência em Memória |
