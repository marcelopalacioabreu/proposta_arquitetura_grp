# 🚀 SelectPesquisavel - Guia Rápido de Uso

## O Componente Resolve

✅ Selects vazios no cadastro  
✅ Suporte a contexto (filtros dependentes)  
✅ Componente reutilizável genérico  
✅ Funciona com qualquer formato de API  

---

## 📍 Onde Está

```
src/interface_grafica/web/src/componentes/Cadastros/SelectPesquisavel.jsx
```

---

## 🎯 Como Usar no JSON de Metadados

### Variação 1: URL Simples
```json
{
  "campo": "organizacaoId",
  "label": "Organização",
  "tipo": "select",
  "url": "/api/organizacoes"
}
```
✓ Carrega de `/api/organizacoes?pageSize=1000`

---

### Variação 2: Endpoint com Filtros Dependentes
```json
{
  "campo": "setorId",
  "label": "Setor",
  "tipo": "select",
  "endpoint": "/api/setores?organizacaoId={organizacaoId}"
}
```
✓ Se organizacaoId = 5, carrega de `/api/setores?organizacaoId=5&pageSize=1000`

---

### Variação 3: Enumeração
```json
{
  "campo": "tipoPessoaChave",
  "label": "Tipo",
  "tipo": "select",
  "enumeracao": "pessoa.tipos"
}
```
✓ Carrega de `/api/enumeracoes/pessoa.tipos?pageSize=1000`

---

### Variação 4: Label e ID Customizados
```json
{
  "campo": "municipioId",
  "label": "Município",
  "tipo": "select",
  "url": "/api/municipios",
  "optionLabel": "descricao",
  "optionId": "codigo"
}
```
✓ Usa campo `descricao` para exibir  
✓ Usa campo `codigo` como value

---

## 🔗 Placeholders Suportados

O componente substitui placeholders com valores do contexto:

| Placeholder | Fonte |
|-------------|-------|
| `{organizacaoId}` | Query param ou path param |
| `{organizacaoUnidadeId}` | Query param ou path param |
| `{setorId}` | Query param ou path param |
| `{id}` | Qualquer outro parâmetro |

**Exemplo:**
```json
{
  "campo": "setorId",
  "label": "Setor",
  "tipo": "select",
  "url": "/api/organizacao_unidades/{organizacaoId}/setores"
}
```

---

## 📝 Exemplo Completo: Cadastro de Usuário

```json
{
  "usuarioCadastro": {
    "tipo": "TELA_CADASTRO",
    "titulo": "Cadastro de Usuário",
    "itens": [
      {
        "titulo": "Alocação",
        "campos": [
          {
            "campo": "organizacaoId",
            "label": "Organização",
            "tipo": "select",
            "url": "/api/organizacoes",
            "col": 4
          },
          {
            "campo": "organizacaoUnidadeId",
            "label": "Unidade",
            "tipo": "select",
            "url": "/api/organizacao_unidades?organizacaoId={organizacaoId}",
            "col": 4
          },
          {
            "campo": "setorId",
            "label": "Setor",
            "tipo": "select",
            "url": "/api/setores?organizacaoId={organizacaoId}&unidadeId={organizacaoUnidadeId}",
            "col": 4
          }
        ]
      }
    ]
  }
}
```

**Fluxo:**
1. Seleciona Organização → Carrega de `/api/organizacoes`
2. Seleciona Unidade → Carrega de `/api/organizacao_unidades?organizacaoId=X`
3. Seleciona Setor → Carrega de `/api/setores?organizacaoId=X&unidadeId=Y`

---

## 📋 Exemplo: Subcadastro com Contexto

```json
{
  "nome": "atuacao",
  "titulo": "Atuação em Setores",
  "colunas": [
    {
      "campo": "organizacaoUnidadeId",
      "label": "Unidade",
      "tipo": "select",
      "url": "/api/organizacao_unidades?ativo=true"
    },
    {
      "campo": "setorId",
      "label": "Setor",
      "tipo": "select",
      "url": "/api/setores?organizacaoUnidadeId={organizacaoUnidadeId}"
    }
  ]
}
```

**Fluxo no subcadastro:**
1. Seleciona Unidade → Carrega de `/api/organizacao_unidades?ativo=true`
2. Seleciona Setor → Carrega de `/api/setores?organizacaoUnidadeId=Z` (usa valor selecionado em Unidade)

---

## 🎨 Formatação Automática

O componente tenta extrair labels dessas propriedades (na ordem):

1. `fieldConfig.optionLabel` (se customizado)
2. `nome`
3. `Nome`
4. `name`
5. `Name`
6. `titulo`
7. `Titulo`
8. `label`
9. `Label`
10. `descricao`
11. `Descricao`
12. `texto`
13. `Texto`
14. `id` (último recurso)

**Exemplo:**
API retorna: `{ id: 1, descricao: "São Paulo" }`  
→ Renderiza como: `<option value="1">São Paulo</option>`

---

## 🔍 Debug / Teste

### Abrir Console (F12)
Procurar por mensagens de carregamento:
- ✅ `Carregando...` = Requisição em progresso
- ❌ Mensagem de erro = Problema no endpoint
- ⚠️ `Nenhuma opção disponível` = Retornou array vazio

### Verificar Requisição
Em DevTools > Network:
1. Verificar se a URL está correta
2. Verificar se os placeholders foram substituídos
3. Verificar resposta da API (deve ter items ou array)

---

## ✨ Features

| Feature | Descrição |
|---------|-----------|
| **Cache** | Evita requisições repetidas da mesma URL |
| **Contexto** | Substitui placeholders com valores reais |
| **Robusto** | Trata diferentes formatos de resposta |
| **Flexível** | Reconhece múltiplas variações de propriedades |
| **Feedback** | Mostra estado (carregando, erro, vazio) |
| **Acessível** | Disabled state, placeholder, etc. |

---

## 🚨 Troubleshooting

### Problema: "Nenhuma opção disponível"

**Causas possíveis:**
1. Endpoint retorna array vazio
   → Verificar se endpoint está correto
   
2. Placeholder não foi substituído
   → Verificar se valor de contexto existe
   
3. API retorna formato inesperado
   → Ver propriedades de `optionLabel` e `optionId`

**Solução:**
```json
{
  "campo": "exemplo",
  "tipo": "select",
  "url": "/api/exemplo",
  "optionLabel": "nomeCampo",
  "optionId": "idCampo"
}
```

---

### Problema: Select carrega, mas valores não aparecem

**Causa:**
Valor salvo não existe no array de opções

**Solução:**
1. Verificar se o valor vem da API
2. Verificar se há filtros aplicados (ex: `ativo=true`)

---

## 📚 Referência Completa

Arquivo de documentação completa: `PROBLEMA_SELECTS_VAZIOS.md`

---

## 🎯 TL;DR

```json
{
  "campo": "organizacaoId",
  "label": "Organização",
  "tipo": "select",
  "url": "/api/organizacoes"
}
```

✨ E pronto! SelectPesquisavel cuida do resto.
