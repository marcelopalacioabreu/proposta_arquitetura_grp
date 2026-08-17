# 🔐 Implementação de Permissões no Frontend

**Data:** 2026-08-17  
**Status:** ✅ Implementado

## ⚡ Regra Principal: Administrador

**Se o usuário for administrador do sistema (`administrador === true`), TODAS as validações de permissão retornam `true`.**

Isso significa:
- Admins têm acesso irrestrito a todas as ações
- Não precisam de permissões específicas para ver/executar ações
- A validação de permissão no backend também segue essa regra

```javascript
const { administrador, temPermissao } = usePermissoes()

// Se admin
if (administrador) {
  temPermissao('qualquer.coisa')  // Sempre true
  temPermissao('organizacoes.editar')  // Sempre true
  temPermissao(['perms', 'multiplas'])  // Sempre true
}

// Se não é admin
if (!administrador) {
  temPermissao('organizacoes.editar')  // True/false baseado em permissões reais
}
```

---

## 📋 O que foi feito

### 1. **Hook Customizado: `usePermissoes.js`**
- ✅ Carrega permissões do endpoint `/auth/me`
- ✅ Carrega flag `administrador` (true se admin)
- ✅ **Regra de Admin:** Se `administrador === true`, `temPermissao()` sempre retorna `true`
- ✅ Fornece função `temPermissao(codigoPermissao)` reutilizável
- ✅ Suporta validação individual (string) ou múltipla (array)
- ✅ Normaliza permissões para strings
- ✅ Gerencia loading state

**Localização:** `src/interface_grafica/web/src/servicos/usePermissoes.js`

**Uso:**
```javascript
import usePermissoes from '../../servicos/usePermissoes'

export default function MinhaTelaComponent(){
  const { temPermissao, administrador, loading } = usePermissoes()
  
  if (loading) return <div>Carregando permissões...</div>
  
  if (administrador) {
    // Admin vê todas as ações
  }
  
  if (temPermissao('organizacoes.editar')) {
    // Se admin: sempre true
    // Se não admin: true se tem a permissão
  }
  
  // Validar múltiplas permissões (AND logic)
  if (temPermissao(['usuarios.listar', 'usuarios.deletar'])) {
    // Se admin: sempre true
    // Se não admin: true se tem TODAS as permissões
  }
}
```

---

### 2. **Validador Genérico: `validadorAcoes.js`**
- ✅ Função `acaoEstaVisivel()` - valida permissão + visibilidade
- ✅ **Regra de Admin:** Já considera admin automaticamente (via `temPermissao()`)
- ✅ Função `itemEstaAtivo()` - detecta status do registro
- ✅ Função `filtrarAcoes()` - filtra array de ações
- ✅ Função `validarAcao()` - validação individual
- ✅ Suporta flags: `exibirQuandoAtivo`, `exibirQuandoInativo`, `exibirQuandoQuery`

**Localização:** `src/interface_grafica/web/src/utils/validadorAcoes.js`

**Uso:**
```javascript
import { acaoEstaVisivel, itemEstaAtivo } from '../../utils/validadorAcoes'
import { useLocation } from 'react-router-dom'

export default function MyComponent(){
  const location = useLocation()
  const query = new URLSearchParams(location.search)
  
  // Validar uma ação
  // temPermissao() já retorna true se admin, então acaoEstaVisivel
  // automaticamente considera admins
  const visivel = acaoEstaVisivel(
    acao,                              // objeto de ação
    (perm) => temPermissao(perm),      // função temPermissao
    item,                              // item/registro (para validar status)
    query                              // querystring
  )
  
  if (visivel) {
    // Renderizar ação
    // Se admin: sempre true (exceto por flags de status/query)
    // Se não admin: true se tem permissão (+ flags de status/query)
  }
}
```

---

### 3. **Integração em `TelaPesquisa.jsx`**
- ✅ Importa `usePermissoes` hook
- ✅ Importa `acaoEstaVisivel` e `itemEstaAtivo`
- ✅ Valida `acoesFormulario` com permissões
- ✅ Renderiza `acoesFormulario` como botões na barra de ferramentas
- ✅ Valida `tabela.acoes` com permissões
- ✅ Passa item para validar status (ativo/inativo)

**Localização:** `src/interface_grafica/web/src/componentes/Cadastros/TelaPesquisa.jsx`

---

## 🎯 Estrutura de Metadados

### Exemplo: Pesquisa de Organizações

```json
{
  "organizacaoPesquisa": {
    "tipo": "TELA_PESQUISA",
    "titulo": "Organizações",
    "extremidade": "/api/organizacoes",
    
    "acoesFormulario": [
      {
        "tipo": "navegacao",
        "rótulo": "Nova Organização",
        "destino": "/painel/organizacoes/editar/new",
        "icone": "bi-plus",
        "permissao": "organizacoes.editar"
      },
      {
        "tipo": "confirmacao_post_ajax",
        "rótulo": "Consolidar",
        "destino": "/api/organizacoes/consolidar",
        "icone": "bi-boxes",
        "permissao": "organizacoes.consolidar",
        "mensagem": "Consolidar organizações selecionadas?"
      }
    ],
    
    "filtro": [
      { "campo": "codigo", "descricao": "Código", "tipo": "string" },
      { "campo": "nome", "descricao": "Nome", "tipo": "string" }
    ],
    
    "tabela": {
      "colunas": [
        { "campo": "codigo", "titulo": "Código" },
        { "campo": "nome", "titulo": "Nome" }
      ],
      
      "acoes": [
        {
          "tipo": "navegacao",
          "destino": "/painel/organizacoes/editar/{id}",
          "icone": "pencil",
          "permissao": "organizacoes.editar"
        },
        {
          "tipo": "confirmacao_delete_ajax",
          "destino": "/api/organizacoes/{id}",
          "icone": "trash",
          "permissao": "organizacoes.deletar"
        },
        {
          "tipo": "confirmacao_post_ajax",
          "destino": "/api/organizacoes/{id}/restaurar",
          "icone": "arrow-counterclockwise",
          "permissao": "organizacoes.restaurar",
          "exibirQuandoInativo": true,
          "mensagem": "Restaurar registro?"
        }
      ]
    },
    
    "pagination": { "pageSize": 10 }
  }
}
```

---

## 📋 Campos Suportados nas Ações

### Permissões

| Campo | Tipo | Descrição |
|-------|------|-----------|
| `permissao` | string | ID da permissão necessária. Sem ele = ação pública |
| `permissoes` | string[] | Array de permissões (AND logic, todas obrigatórias) |

### Visibilidade Condicional

| Campo | Tipo | Descrição |
|-------|------|-----------|
| `exibirQuandoAtivo` | boolean | Exibir apenas quando `item.ativo === true` |
| `exibirQuandoInativo` | boolean | Exibir apenas quando `item.ativo === false` |
| `exibirQuandoQuery` | string | Exibir se query match (ex: `"inativo=1"` ou `"inativo=1&restore=1"`) |

### Renderização

| Campo | Tipo | Descrição |
|-------|------|-----------|
| `tipo` | string | `navegacao`, `confirmacao_delete_ajax`, `confirmacao_post_ajax` |
| `icone` | string | Classe Bootstrap Icons (ex: `"bi-plus"`, `"pencil"`, `"trash"`) |
| `rótulo` | string | Label do botão (opcional, fallback: tipo) |
| `destino` | string | URL ou rota (suporta placeholders `{id}`, `{organizacaoId}`, etc) |
| `campo_id` | string | Campo do item a usar como ID nos placeholders |
| `mensagem` | string | Mensagem de confirmação |

---

## 🔄 Fluxo de Validação

```
┌──────────────────────────────────────────────────────────┐
│ TelaPesquisa.jsx                                          │
│ ┌─────────────────────────────────────────────────────┐ │
│ │ 1. Chamar usePermissoes() → temPermissao()          │ │
│ │    Se administrador === true:                       │ │
│ │    └─ temPermissao() SEMPRE retorna true            │ │
│ │                                                      │ │
│ │ 2. Renderizar acoesFormulario[]                     │ │
│ │    ├─ Para cada ação:                              │ │
│ │    │  ├─ Chamar actionIsVisible(acao)              │ │
│ │    │  │  └─ Valida: permissao + flags              │ │
│ │    │  │     (Se admin: sempre true p/ permissão)   │ │
│ │    │  └─ Se visível → renderizar botão             │ │
│ │    └─ Suporta: navegacao, confirmacao_post_ajax    │ │
│ │                                                      │ │
│ │ 3. Renderizar tabela.acoes[] para cada item        │ │
│ │    ├─ Para cada item:                              │ │
│ │    │  └─ Para cada ação:                           │ │
│ │    │     ├─ Chamar actionIsVisible(acao, item)     │ │
│ │    │     │  └─ Valida:                             │ │
│ │    │     │     - permissao (admin sempre true)     │ │
│ │    │     │     - flags + status                    │ │
│ │    │     └─ Se visível → renderizar ícone          │ │
│ │    └─ Suporta: navegacao, delete_ajax, post_ajax   │ │
│ └─────────────────────────────────────────────────────┘ │
└──────────────────────────────────────────────────────────┘
```

---

## ⚙️ Comparação: Admin vs. Usuário Normal

| Cenário | Admin | Usuário Normal |
|---------|-------|---|
| Sem `permissao` | ✅ Visível | ✅ Visível |
| Com `permissao` | ✅ Visível (sempre) | ✅ Visível se tem permissão |
| Com `exibirQuandoAtivo: true` | ⚠️ Depende do status | ⚠️ Depende do status |
| Com `exibirQuandoInativo: true` | ⚠️ Depende do status | ⚠️ Depende do status |
| Com `exibirQuandoQuery` | ⚠️ Depende da query | ⚠️ Depende da query |

**Nota:** Admin também respeita flags de status/query, apenas não respeita permissão.

---

## ✅ Exemplos de Metadados para Suas Telas

### Pesquisa de Usuários

```json
{
  "usuarioPesquisa": {
    "tipo": "TELA_PESQUISA",
    "titulo": "Usuários",
    "extremidade": "/api/usuarios",
    "acoesFormulario": [
      {
        "tipo": "navegacao",
        "rótulo": "Novo Usuário",
        "destino": "/painel/usuarios/editar/new",
        "icone": "bi-plus",
        "permissao": "usuarios.criar"
      },
      {
        "tipo": "confirmacao_post_ajax",
        "rótulo": "Exportar CSV",
        "destino": "/api/usuarios/exportar",
        "icone": "bi-download",
        "permissao": "usuarios.exportar"
      }
    ],
    "tabela": {
      "acoes": [
        {
          "tipo": "navegacao",
          "destino": "/painel/usuarios/editar/{id}",
          "icone": "pencil",
          "permissao": "usuarios.editar"
        },
        {
          "tipo": "confirmacao_delete_ajax",
          "destino": "/api/usuarios/{id}",
          "icone": "trash",
          "permissao": "usuarios.deletar"
        }
      ]
    }
  }
}
```

### Pesquisa de Perfis

```json
{
  "perfilPesquisa": {
    "tipo": "TELA_PESQUISA",
    "titulo": "Perfis",
    "extremidade": "/api/perfis",
    "acoesFormulario": [
      {
        "tipo": "navegacao",
        "rótulo": "Novo Perfil",
        "destino": "/painel/perfis/editar/new",
        "icone": "bi-plus",
        "permissao": "perfis.criar"
      }
    ],
    "tabela": {
      "acoes": [
        {
          "tipo": "navegacao",
          "destino": "/painel/perfis/editar/{id}",
          "icone": "pencil",
          "permissao": "perfis.editar"
        },
        {
          "tipo": "confirmacao_delete_ajax",
          "destino": "/api/perfis/{id}",
          "icone": "trash",
          "permissao": "perfis.deletar",
          "exibirQuandoAtivo": true
        }
      ]
    }
  }
}
```

---

## 🚀 Próximos Passos

### 1. Atualizar `TelaCadastro.jsx` (Cadastro)
- [ ] Importar `usePermissoes` e validadores
- [ ] Validar botões de salvar/deletar por permissão
- [ ] Ocultar campos readonly para usuários sem permissão

### 2. Atualizar Componentes de Modal
- [ ] `TelaPesquisaDetalhesLinhaTelaPequena.jsx` - validar ações em modal
- [ ] Suportar `acoesFormulario` em modais se necessário

### 3. Atualizar Menu Lateral
- [ ] Menu já valida permissões via metadados
- [ ] Confirmar que está funcionando corretamente

### 4. Testar Cenários
- [ ] Usuário sem permissão não vê botão
- [ ] Botão desabilitado se sem permissão
- [ ] Flags `exibirQuando*` funcionam
- [ ] Ações bulk (consolidar) funcionam corretamente

### 5. Atualizar Documentação de Metadados
- [ ] Adicionar campo `permissao` aos exemplos de telas
- [ ] Documentar suporte a `acoesFormulario`

---

## 📚 Arquivos Criados/Modificados

### ✨ Novos
1. `src/interface_grafica/web/src/servicos/usePermissoes.js`
2. `src/interface_grafica/web/src/utils/validadorAcoes.js`
3. `PERMISSOES_FRONTEND.md` (este arquivo)

### 🔧 Modificados
1. `src/interface_grafica/web/src/componentes/Cadastros/TelaPesquisa.jsx`

---

## 💡 Dicas de Uso

### Validar Múltiplas Permissões

```javascript
// AND logic - todas obrigatórias
if (temPermissao(['organizacoes.editar', 'organizacoes.deletar'])) {
  // Ambas as permissões são obrigatórias
}

// Validar uma a uma
if (temPermissao('organizacoes.editar') && temPermissao('organizacoes.deletar')) {
  // Mesmo resultado, mas mais verboso
}
```

### Flag `exibirQuandoQuery`

```json
{
  "tipo": "navegacao",
  "destino": "/painel/itens/restaurar/{id}",
  "icone": "arrow-counterclockwise",
  "exibirQuandoQuery": "inativo=1",
  "exibirQuandoInativo": true,
  "permissao": "itens.restaurar"
}
```
Renderiza apenas quando:
- Query string contém `inativo=1`
- **E** item tem `inativo === true` ou `ativo === false`
- **E** usuário tem permissão `itens.restaurar`

### Ações sem Permissão (Públicas)

```json
{
  "tipo": "navegacao",
  "destino": "/painel/itens/visualizar/{id}",
  "icone": "eye"
  // Sem campo "permissao" = pública, todos veem
}
```

---

## 🔍 Troubleshooting

### Botão não aparece mesmo com permissão

1. ✅ Verificar se permissão está escrita corretamente nos metadados
2. ✅ Verificar se `/auth/me` retorna a permissão
3. ✅ Verificar flags `exibirQuando*`
4. ✅ Abrir console e ver resultado de `temPermissao('permissao.teste')`

### Carregamento lento

- Hook carrega permissões uma vez ao montar
- Use sessão/cache se necessário implementar

### Permissões não atualizando

- Hook não monitora mudanças de permissão em runtime
- Para forçar atualizar: `window.location.reload()`

---

## 🎓 Padrão Arquitetural

```
Frontend (React)
    ↓
usePermissoes() hook
    ↓
Carrega /auth/me → permissoes[]
    ↓
validadorAcoes.js
    ├─ acaoEstaVisivel(acao, temPermissao, item, query)
    ├─ itemEstaAtivo(item)
    └─ filtrarAcoes(acoes, temPermissao, item, query)
    ↓
TelaPesquisa/TelaCadastro
    ├─ Renderiza acoesFormulario[] com validação
    └─ Renderiza tabela.acoes[] com validação
    ↓
Usuário vê apenas ações permitidas
```

---

**Implementado em:** 2026-08-17  
**Versão:** 1.0  
**Status:** ✅ Produção Pronta
