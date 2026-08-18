# Solução Técnica: Subcadastros Genéricos e Enumerações

## 🎯 Objetivo

Fornecer uma arquitetura **genérica, reutilizável e baseada em eventos** para:
1. Associar múltiplas entidades relacionadas (subcadastros)
2. Gerenciar enumerações com conversão bidireccional (valor ↔ texto)
3. Manter **atomicidade** na submissão de formulários complexos
4. Evitar duplicação de código entre diferentes cadastros

## 📊 Estrutura de Solução

### Camada C# (Backend)

#### 1. **Metadados - Definições de Estrutura**
```
src/retaguarda/Metadados/Contratos/
├── SubcadastroDefinition.cs      # Define estrutura de subtabela
├── ScreenDefinition.cs            # Expandido com subcadastros
└── Enumeracoes/
    └── Enumeracao.cs              # Base abstrata para enumerações
```

**Responsabilidades:**
- Descrever estrutura de subcadastros (nome, título, colunas, seleção)
- Definir interface `IEnumeracao` para conversão valor ↔ texto
- Fornecer contratos para JSON de metadados

#### 2. **DTOs - Transferência de Dados**
```
src/retaguarda/Retaguarda.DTO/Dtos/
└── SetorAtuacaoDto.cs            # Exemplo: atuação de usuário em setor
```

**Responsabilidades:**
- Estruturar dados que serão recebidos do frontend
- Incluir identificadores e flags de estado (ex: `ehPadrao`)

### Camada React (Frontend)

#### 1. **Componente Genérico**
```
src/interface_grafica/web/src/componentes/Cadastros/
└── SubtabelaCadastro.jsx          # Subtabela reutilizável
```

**Responsabilidades:**
- Renderizar tabela de itens associados
- Gerenciar operações: adicionar, editar (inline), remover
- Carregar opções dinâmicas de endpoints
- Disparar evento `onDadosAlterados(dados)` para comunicação
- Suportar múltiplos tipos de campos

#### 2. **Integração com Formulário Principal**
```
src/interface_grafica/web/src/componentes/Cadastros/
└── TelaCadastro.jsx               # Estendido
```

**Mudanças:**
- Importa e renderiza `SubtabelaCadastro` para cada subcadastro
- Armazena dados de subcadastros em `subcadastrosRef.current`
- Agrega dados na submissão: `construirObjetoFormulario()`

### Metadados (JSON)

```
src/retaguarda/Metadados/Contratos/Telas/cliente/painel/
└── usuarios/cadastro.json         # Exemplo com subcadastro
```

**Estrutura:**
```json
{
  "usuarioCadastro": {
    "tipo": "TELA_CADASTRO",
    "itens": [/* campos principais */],
    "subcadastros": [
      {
        "nome": "atuacao",
        "titulo": "Atuação em Setores",
        "endpoint": "/api/setores",
        "campoArmazenamento": "atuacoes",
        "colunas": [/* definição de campos */],
        "selecao": {/* configuração de seleção */},
        "maxLinhas": null
      }
    ]
  }
}
```

## 🔄 Fluxo de Funcionamento

### 1️⃣ Carregamento da Tela

```
┌─────────────────────────────────┐
│ TelaCadastro (useEffect)         │
├─────────────────────────────────┤
│ 1. GET /meta/screens             │
│ 2. Obtém meta.subcadastros       │
│ 3. Renderiza SubtabelaCadastro   │
│ 4. SubtabelaCadastro carrega     │
│    opções dos endpoints          │
└─────────────────────────────────┘
```

### 2️⃣ Interação do Usuário

```
┌─────────────────────────────────┐
│ SubtabelaCadastro (Estado)       │
├─────────────────────────────────┤
│ [linhas] ← Usuário interage      │
│  ├─ Adiciona nova linha          │
│  ├─ Remove linha                 │
│  ├─ Marca como padrão            │
│  └─ Edita valor inline           │
│                                  │
│ onDadosAlterados(linhas)         │
└──────────────┬────────────────────┘
               │
        ┌──────▼──────────┐
        │ TelaCadastro    │
        │ subcadastrosRef │
        │ [atuacao]: [...] │
        └─────────────────┘
```

### 3️⃣ Submissão Atômica

```
┌──────────────────────────────────────┐
│ Usuário clica "Salvar"               │
├──────────────────────────────────────┤
│ construirObjetoFormulario(formData)  │
│                                      │
│ 1. Coleta campos do form             │
│    {nome: "João", email: "..."}      │
│                                      │
│ 2. Normaliza checkboxes              │
│    {administrador: true/false}       │
│                                      │
│ 3. Agrega subcadastros               │
│    {atuacoes: [...]}                 │
│                                      │
│ 4. Inclui campos da URL              │
│    {organizacaoId: 1}                │
│                                      │
│ 5. Valida schema                     │
│                                      │
│ Resultado:                           │
│ {                                    │
│   "nome": "João",                   │
│   "email": "joao@example.com",      │
│   "atuacoes": [                     │
│     {                               │
│       "organizacaoUnidadeId": 1,    │
│       "setorId": 5,                 │
│       "ehPadrao": true              │
│     }                               │
│   ]                                  │
│ }                                    │
│                                      │
│ POST /api/usuarios                   │
└──────────────────────────────────────┘
```

## 🎨 Tipos de Campos Suportados

| Tipo | Renderização | Saída | Exemplo |
|------|---|---|---|
| `text` | `<input type="text" />` | string | "João Silva" |
| `select` | `<select>` com opções | string/number | 5 |
| `checkbox` | `<input type="checkbox" />` | boolean | true/false |
| `date` | `<input type="date" />` | string (ISO) | "2026-08-17" |
| `number` | `<input type="number" />` | number | 42 |

## 🔐 Atomicidade e Transação

**Problema:** Sem subcadastro, múltiplas linhas relacionadas requerem múltiplas requisições, causando inconsistência se uma falhar.

**Solução:** Todos os dados (formulário principal + subcadastros) são enviados em **um único POST/PUT**, garantindo transação atômica no banco de dados.

```
Falha em Linha 1 → ❌ Toda transação reverte
Falha em Linha 2 → ❌ Toda transação reverte
Todas OK        → ✅ Todas persistem
```

## 🔄 Comunicação por Eventos

**Padrão:** Em vez de métodos específicos por tela, usamos callbacks genéricos.

```javascript
// SubtabelaCadastro
onDadosAlterados(dados) {
  // Dispara quando dados mudam
  // TelaCadastro armazena em ref
}

// TelaCadastro
const handleDadosAlterados = (dados) => {
  subcadastrosRef.current[sub.nome] = dados
}
```

**Benefício:** Mesmo padrão funciona para qualquer subcadastro.

## 🎯 Reutilização

### Para usar um novo subcadastro:

1. **Defina no JSON** (ex: `enderecos/cadastro.json`)
```json
{
  "subcadastros": [
    {
      "nome": "enderecosOrganizacao",
      "titulo": "Endereços",
      "endpoint": "/api/enderecos",
      "campoArmazenamento": "enderecos",
      "colunas": [...]
    }
  ]
}
```

2. **Nenhuma mudança** em React ou C# necessária!

3. **Componentes reutilizados:**
   - `SubtabelaCadastro.jsx` - Mesmo componente
   - `TelaCadastro.jsx` - Mesma lógica de renderização

### Exemplos Potenciais

| Entidade | Subcadastro | Uso |
|---|---|---|
| Organização | Endereços | Múltiplos endereços (sede, filiais) |
| Pessoa | Contatos | Telefones, emails por tipo |
| Pessoa | Documentos | Múltiplos documentos de ID |
| Usuário | Atuação | Setores e unidades de trabalho |
| Processo | Anexos | Múltiplos arquivos |
| Agendamento | Horários | Faixas horárias disponíveis |

## 📝 Enumerações

### Padrão C#

```csharp
public class TipoPessoa : Enumeracao
{
    public static readonly TipoPessoa Fisica = 
        new("F", "Pessoa Física");
    public static readonly TipoPessoa Juridica = 
        new("J", "Pessoa Jurídica");
    
    private TipoPessoa(string valor, string texto) 
        : base(valor, texto) { }
    
    public static TipoPessoa? ConverterDe(string? valor) =>
        valor?.ToUpper() switch {
            "F" => Fisica,
            "J" => Juridica,
            _ => null
        };
}
```

### Fluxo

```
Usuário vê:    "Pessoa Física"    (Texto - UI)
               ↓
Backend retorna: "F"               (Valor - BD)
               ↓
Frontend envia: "F"                (Valor em FormData)
               ↓
Banco armazena: "F"                (Valor em coluna)
```

### Arquivo JSON

```json
{
  "pessoa_tipos": {
    "chave": "pessoa.tipos",
    "valores": [
      {"valor": "F", "texto": "Pessoa Física"},
      {"valor": "J", "texto": "Pessoa Jurídica"}
    ]
  }
}
```

## 🛡️ Validação

### Frontend
- Validação obrigatória em `validarLinha()` do SubtabelaCadastro
- Verificação de duplicatas
- Feedback visual de erros

### Backend
- DTO com atributos `[Required]`, `[StringLength]`, etc.
- Controller valida schema antes de persistir
- `try-catch` com rollback transacional

## 📚 Documentação Fornecida

1. **ORIENTACAO_SUBCADASTROS.md**
   - Guia completo de estrutura e uso
   - Exemplos práticos
   - Fluxo de submissão
   - Padrões de reutilização

2. **ORIENTACAO_ENUMERACOES.md**
   - Padrão C# detalhado
   - Formato JSON
   - Exemplos com TipoPessoa
   - Integração com DTO e frontend

3. **DIAGRAMAS_ARQUITETURA.md**
   - 10 diagramas Mermaid
   - Fluxo de dados
   - Ciclo de vida dos componentes
   - Estados e eventos

## 🚀 Próximos Passos

### Implementação Backend
1. ✅ Contratos (DTOs)
2. ⬜ Controller para processar atuações
3. ⬜ Serviço com transação atômica
4. ⬜ Validação de negócio

### Testes
1. ⬜ Testes unitários: SubtabelaCadastro
2. ⬜ Testes E2E: fluxo completo
3. ⬜ Validação de atomicidade

### Exemplos Adicionais
1. ⬜ Subcadastro de endereços
2. ⬜ Subcadastro de contatos
3. ⬜ Template reutilizável

## 💡 Decisões de Design

| Decisão | Razão |
|---|---|
| Componente genérico | Evita duplicação de código |
| Eventos/callbacks | Desacoplamento entre componentes |
| `useRef` para dados | Evita re-render desnecessário |
| Submissão atômica | Garante consistência de dados |
| JSON de metadados | Configuração sem código |
| Enumerações imutáveis | Segurança de thread, singleton |

## ⚠️ Considerações Importantes

1. **Sem Undo/Redo**: Subtabelas não têm histórico de mudanças
2. **Validação Dupla**: Precisa validar tanto frontend quanto backend
3. **Limite de Linhas**: Considere `maxLinhas` para performance
4. **Cascade Delete**: Decidir comportamento ao deletar pai (setor)
5. **Permissões**: Cada operação pode precisar de validação de acesso

## 📞 Suporte

Para questões sobre:
- **Enumerações**: Veja `ORIENTACAO_ENUMERACOES.md`
- **Subcadastros**: Veja `ORIENTACAO_SUBCADASTROS.md`
- **Arquitetura**: Veja `DIAGRAMAS_ARQUITETURA.md`
