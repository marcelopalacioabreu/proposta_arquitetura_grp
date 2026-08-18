# 📚 Índice de Documentação - Subcadastros e Enumerações

## 🎯 Começar Aqui

1. **Novo no assunto?** → [SOLUCAO_SUBCADASTROS.md](./SOLUCAO_SUBCADASTROS.md)
2. **Implementar novo subcadastro?** → [CHECKLIST_NOVO_SUBCADASTRO.md](./CHECKLIST_NOVO_SUBCADASTRO.md)
3. **Entender a arquitetura?** → [DIAGRAMAS_ARQUITETURA.md](./DIAGRAMAS_ARQUITETURA.md)

---

## 📖 Documentação Detalhada

### 🏗️ Arquitetura e Visão Geral

| Arquivo | Descrição | Quando Usar |
|---------|-----------|-------------|
| [SOLUCAO_SUBCADASTROS.md](./SOLUCAO_SUBCADASTROS.md) | **README técnico completo** - Estrutura, fluxo, componentes, decisões de design | Entender a solução em detalhes |
| [DIAGRAMAS_ARQUITETURA.md](./DIAGRAMAS_ARQUITETURA.md) | **10 diagramas Mermaid** - Fluxo de dados, ciclo de vida, integração | Visualizar como tudo conecta |

### 👨‍💻 Guias de Implementação

| Arquivo | Descrição | Quando Usar |
|---------|-----------|-------------|
| [ORIENTACAO_SUBCADASTROS.md](./ORIENTACAO_SUBCADASTROS.md) | **Guia prático de subcadastros** - Metadados, componentes, padrões | Implementar novo subcadastro |
| [ORIENTACAO_ENUMERACOES.md](./ORIENTACAO_ENUMERACOES.md) | **Padrão de enumerações** - C#, JSON, validação | Criar enumeração com conversão |
| [CHECKLIST_NOVO_SUBCADASTRO.md](./CHECKLIST_NOVO_SUBCADASTRO.md) | **Checklist passo a passo** - 11 fases, testes, troubleshooting | Guia de implementação completa |

---

## 📁 Estrutura de Arquivos

### Backend (C#)

```
src/retaguarda/
├── Metadados/Contratos/
│   ├── SubcadastroDefinition.cs         ← Define estrutura de subcadastro
│   ├── ScreenDefinition.cs              ← Expandido com subcadastros
│   └── Enumeracoes/
│       └── Enumeracao.cs                ← Base abstrata IEnumeracao
│
├── Dominio/Entidades/
│   ├── SetorUsuario.cs                  ← Exemplo de entidade relacionada
│   └── Enumeracoes/
│       ├── IEnumeracao.cs               ← Interface
│       ├── EnumeracaoHelper.cs          ← Métodos auxiliares
│       └── PessoaTipo.cs                ← Exemplo: F/J
│
├── Metadados/Contratos/Telas/cliente/painel/usuarios/
│   └── cadastro.json                    ← Exemplo com subcadastro
│
└── Retaguarda.DTO/Dtos/
    └── SetorAtuacaoDto.cs               ← DTO para subcadastro
```

### Frontend (React)

```
src/interface_grafica/web/src/componentes/Cadastros/
├── SubtabelaCadastro.jsx                ← Componente genérico ⭐
├── TelaCadastro.jsx                     ← Estendido para usar subcadastros
└── PermissoesModulos.jsx                ← Componente relacionado
```

---

## 🔑 Conceitos Principais

### 1. Subcadastro (Subtabela)
**Definição:** Tabela associada a um formulário principal para gerenciar múltiplas entidades relacionadas.

**Exemplo:** Um usuário pode ter múltiplas atuações (setor + unidade).

**Arquivo de referência:** [ORIENTACAO_SUBCADASTROS.md](./ORIENTACAO_SUBCADASTROS.md)

### 2. Enumeração
**Definição:** Valores conhecidos do sistema com conversão bidireccional (Valor em BD ↔ Texto em UI).

**Exemplo:** Tipo de Pessoa = "F" (BD) ↔ "Pessoa Física" (UI)

**Arquivo de referência:** [ORIENTACAO_ENUMERACOES.md](./ORIENTACAO_ENUMERACOES.md)

### 3. Atomicidade
**Definição:** Submissão de formulário principal + subcadastros em uma única transação.

**Benefício:** Garante consistência - ou tudo é salvo ou nada é salvo.

**Arquivo de referência:** [SOLUCAO_SUBCADASTROS.md](./SOLUCAO_SUBCADASTROS.md#🔐-atomicidade-e-transação)

### 4. Padrão Genérico
**Definição:** Componentes e estruturas reutilizáveis sem código específico por cadastro.

**Benefício:** Adicionar novo subcadastro = apenas JSON, sem mudança de React/C#

**Arquivo de referência:** [SOLUCAO_SUBCADASTROS.md](./SOLUCAO_SUBCADASTROS.md#🔄-reutilização)

---

## 🚀 Quick Start

### Entender a Arquitetura (5 min)
1. Leia [SOLUCAO_SUBCADASTROS.md](./SOLUCAO_SUBCADASTROS.md) seção "Estrutura de Solução"
2. Veja diagrama #1 em [DIAGRAMAS_ARQUITETURA.md](./DIAGRAMAS_ARQUITETURA.md)

### Implementar Novo Subcadastro (30 min)
1. Siga [CHECKLIST_NOVO_SUBCADASTRO.md](./CHECKLIST_NOVO_SUBCADASTRO.md)
2. Use [ORIENTACAO_SUBCADASTROS.md](./ORIENTACAO_SUBCADASTROS.md) como referência
3. Copie JSON de exemplo do `usuarios/cadastro.json` e adapte

### Criar Enumeração (15 min)
1. Leia [ORIENTACAO_ENUMERACOES.md](./ORIENTACAO_ENUMERACOES.md) seção "Exemplo Prático"
2. Copie classe `TipoPessoa.cs` como template
3. Atualize arquivo JSON de enumeração

---

## 📊 Matriz de Decisão

**O que preciso?** → **Onde está?**

| Necessidade | Arquivo | Seção |
|---|---|---|
| Entender fluxo completo | SOLUCAO_SUBCADASTROS.md | Fluxo de Funcionamento |
| Ver exemplos de JSON | ORIENTACAO_SUBCADASTROS.md | Exemplo Prático |
| Implementar C# | ORIENTACAO_ENUMERACOES.md | Classe C# Base |
| Testar componente React | CHECKLIST_NOVO_SUBCADASTRO.md | Phase 7: Testes Frontend |
| Processar no backend | SOLUCAO_SUBCADASTROS.md | Implementação Backend |
| Resolver erro | CHECKLIST_NOVO_SUBCADASTRO.md | Troubleshooting |
| Visualizar arquitetura | DIAGRAMAS_ARQUITETURA.md | Qualquer diagrama |

---

## 🔗 Referências Cruzadas

### SubcadastroDefinition.cs
- Definido em: `src/retaguarda/Metadados/Contratos/`
- Documentado em: [ORIENTACAO_SUBCADASTROS.md](./ORIENTACAO_SUBCADASTROS.md#estrutura-de-metadados)
- Diagramado em: [DIAGRAMAS_ARQUITETURA.md](./DIAGRAMAS_ARQUITETURA.md) #2 e #9

### Enumeracao.cs
- Definido em: `src/retaguarda/Metadados/Contratos/Enumeracoes/`
- Documentado em: [ORIENTACAO_ENUMERACOES.md](./ORIENTACAO_ENUMERACOES.md#classe-c-base)
- Diagramado em: [DIAGRAMAS_ARQUITETURA.md](./DIAGRAMAS_ARQUITETURA.md) #4

### SubtabelaCadastro.jsx
- Definido em: `src/interface_grafica/web/src/componentes/Cadastros/`
- Documentado em: [ORIENTACAO_SUBCADASTROS.md](./ORIENTACAO_SUBCADASTROS.md#componente-react)
- Diagramado em: [DIAGRAMAS_ARQUITETURA.md](./DIAGRAMAS_ARQUITETURA.md) #5 e #7

---

## ✨ Destaques

### Padrão Reutilizável
```json
{
  "nome": "NOVO_SUBCADASTRO",
  "titulo": "Título para UI",
  "endpoint": "/api/dados",
  "campoArmazenamento": "dados",
  "colunas": [/* definição */],
  "selecao": {/* opcional */}
}
```
👉 Nenhuma mudança de código React/C# necessária!

### Atomicidade Garantida
```
Submissão = Formulário Principal + Subcadastros
           = Um POST/PUT único
           = Uma transação no BD
           = Tudo ou nada
```

### Componente Genérico
- Suporta: text, select, checkbox, date, number
- Carrega opções dinamicamente
- Gerencia validação interna
- Comunica via eventos

---

## 🎓 Leitura Recomendada

### Para Entender
1. [SOLUCAO_SUBCADASTROS.md](./SOLUCAO_SUBCADASTROS.md) - 30 min
2. [DIAGRAMAS_ARQUITETURA.md](./DIAGRAMAS_ARQUITETURA.md) - 15 min

### Para Implementar
1. [CHECKLIST_NOVO_SUBCADASTRO.md](./CHECKLIST_NOVO_SUBCADASTRO.md) - 60 min
2. [ORIENTACAO_SUBCADASTROS.md](./ORIENTACAO_SUBCADASTROS.md) - Referência contínua
3. [ORIENTACAO_ENUMERACOES.md](./ORIENTACAO_ENUMERACOES.md) - Se enumerações

### Para Troubleshoot
1. [CHECKLIST_NOVO_SUBCADASTRO.md](./CHECKLIST_NOVO_SUBCADASTRO.md#troubleshooting)
2. [SOLUCAO_SUBCADASTROS.md](./SOLUCAO_SUBCADASTROS.md#⚠️-considerações-importantes)

---

## 🆘 Precisa de Ajuda?

| Dúvida | Procure em |
|--------|-----------|
| "Como estruturar meu subcadastro?" | ORIENTACAO_SUBCADASTROS.md |
| "Como implementar enumeração?" | ORIENTACAO_ENUMERACOES.md |
| "Como faz um novo subcadastro?" | CHECKLIST_NOVO_SUBCADASTRO.md |
| "Por que usar referência?" | SOLUCAO_SUBCADASTROS.md #Decisões de Design |
| "Qual é o fluxo de dados?" | DIAGRAMAS_ARQUITETURA.md #1 |
| "Meu subcadastro não aparece" | CHECKLIST_NOVO_SUBCADASTRO.md #Troubleshooting |

---

## 📝 Arquivos do Projeto

### Documentação (este diretório)
- ✅ SOLUCAO_SUBCADASTROS.md
- ✅ DIAGRAMAS_ARQUITETURA.md
- ✅ ORIENTACAO_SUBCADASTROS.md
- ✅ ORIENTACAO_ENUMERACOES.md
- ✅ CHECKLIST_NOVO_SUBCADASTRO.md
- ✅ INDEX.md (este arquivo)

### Código (repositório)
- ✅ src/retaguarda/Metadados/Contratos/SubcadastroDefinition.cs
- ✅ src/retaguarda/Metadados/Contratos/ScreenDefinition.cs
- ✅ src/retaguarda/Metadados/Contratos/Enumeracoes/Enumeracao.cs
- ✅ src/interface_grafica/web/src/componentes/Cadastros/SubtabelaCadastro.jsx
- ✅ src/interface_grafica/web/src/componentes/Cadastros/TelaCadastro.jsx (estendido)
- ✅ src/retaguarda/Metadados/Contratos/Telas/cliente/painel/usuarios/cadastro.json

---

**Última atualização:** 2026-08-17  
**Versão:** 1.0.0  
**Status:** ✅ Produção
