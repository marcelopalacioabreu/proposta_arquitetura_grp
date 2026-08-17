# 🎯 Guia de Implementação - Validação de Permissões Frontend

## Resumo Rápido

Todo componente React que renderiza **ações de usuário** (botões, links) deve:
1. Usar o hook `usePermissoes()`
2. Passar a função `temPermissao` ao validador `acaoEstaVisivel()`
3. Respeitar flags de status (`exibirQuandoAtivo`, `exibirQuandoInativo`)

---

## 🔧 Componentes Implementados

### 1. Hook: `usePermissoes.js`
**Localização:** `src/interface_grafica/web/src/hooks/usePermissoes.js`

**O que faz:**
- Carrega dados do usuário via `/auth/me` (inclui `administrador` e `permissoes[]`)
- Retorna função `temPermissao()` que valida permissões com admin bypass

**Como usar:**
```javascript
import { usePermissoes } from '../hooks/usePermissoes';

export function MeuComponente() {
  const { temPermissao, loading, error } = usePermissoes();
  
  if (loading) return <div>Carregando...</div>;
  if (error) return <div>Erro ao carregar permissões</div>;
  
  // temPermissao está pronto para usar
  return (
    <button disabled={!temPermissao('usuarios.editar')}>
      Editar Usuário
    </button>
  );
}
```

**Admin Bypass Automático:**
```javascript
// Se o usuário é admin (administrador === true):
temPermissao('qualquer.coisa') // → SEMPRE true
temPermissao(['usuarios.editar', 'perfis.editar']) // → SEMPRE true

// Se não é admin:
temPermissao('usuarios.editar') // → true SE tiver no array permissoes[]
```

---

### 2. Validador: `validadorAcoes.js`
**Localização:** `src/interface_grafica/web/src/utils/validadorAcoes.js`

**O que faz:**
- Valida se uma ação deve ser exibida (permissão + status + condiçõesQuery)
- Suporta regras complexas (botão só aparece se ativo, ou só aparece se inativo, etc.)

**Como usar:**
```javascript
import { acaoEstaVisivel } from '../utils/validadorAcoes';

export function TabelaComAcoes({ item, meta, temPermissao }) {
  
  return (
    <div>
      {meta.tabela.acoes.map(acao => {
        // queryParams do componente pai (ex: ?inativo=1)
        if (acaoEstaVisivel(acao, temPermissao, item, queryParams)) {
          return (
            <button key={acao.tipo} onClick={() => handleAcao(acao)}>
              {acao.icone}
            </button>
          );
        }
      })}
    </div>
  );
}
```

**Exemplos de Ações:**

```javascript
// Botão "Novo" - só aparece se tem permissão
{
  "tipo": "navegacao",
  "destino": "/usuarios/novo",
  "icone": "bi-plus",
  "permissao": "usuarios.editar"  // ✓ Sempre valida permissão
}

// Botão "Editar" - só aparece se item está ATIVO e tem permissão
{
  "tipo": "navegacao",
  "destino": "/usuarios/{{id}}",
  "icone": "pencil",
  "permissao": "usuarios.editar",
  "exibirQuandoAtivo": true  // ✓ Valida se item.ativo === true
}

// Botão "Deletar" - só aparece se item está ATIVO
{
  "tipo": "confirmacao_delete_ajax",
  "destino": "/api/usuarios/{{id}}",
  "icone": "trash",
  "permissao": "usuarios.excluir",
  "exibirQuandoAtivo": true  // ✓ Item deve estar ativo
}

// Botão "Restaurar" - só aparece se item está INATIVO
{
  "tipo": "confirmacao_post_ajax",
  "destino": "/api/usuarios/{{id}}/restaurar",
  "icone": "arrow-counterclockwise",
  "permissao": "usuarios.editar",
  "exibirQuandoInativo": true  // ✓ Item deve estar inativo
}

// Botão "Aceitar" - só aparece se query param específico
{
  "tipo": "confirmacao_post_ajax",
  "destino": "/api/usuarios/{{id}}/aceitar",
  "icone": "check",
  "permissao": "usuarios.editar",
  "exibirQuandoQuery": "pendente=1"  // ✓ Se URL tem ?pendente=1
}
```

---

## 📱 Padrão de Tela de Pesquisa

**Arquivo Base:** [src/interface_grafica/web/src/components/TelaPesquisa.jsx](../../src/interface_grafica/web/src/components/TelaPesquisa.jsx)

```javascript
import { usePermissoes } from '../hooks/usePermissoes';
import { acaoEstaVisivel, itemEstaAtivo } from '../utils/validadorAcoes';

export function TelaPesquisa() {
  const { temPermissao, loading } = usePermissoes();
  const meta = useMeta();  // carrega pesquisa.json
  
  if (loading) return <Loading />;
  
  return (
    <>
      {/* ─────── AÇÕES DE FORMULÁRIO ─────── */}
      <div className="acoes-form">
        {meta.acoesFormulario?.map(acao => {
          if (acaoEstaVisivel(acao, temPermissao)) {
            return <button key={acao.tipo}>{acao.icone}</button>;
          }
        })}
      </div>
      
      {/* ─────── TABELA ─────── */}
      <table>
        <tbody>
          {items.map(item => (
            <tr key={item.id}>
              <td>{item.nome}</td>
              <td>
                {/* ─────── AÇÕES DA TABELA ─────── */}
                {meta.tabela.acoes?.map(acao => {
                  if (acaoEstaVisivel(acao, temPermissao, item, queryParams)) {
                    return <button>{acao.icone}</button>;
                  }
                })}
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </>
  );
}
```

---

## 🔐 Permissões Disponíveis

Todas essas são validadas automaticamente:

**Administração:**
- `usuarios.{visualizar, editar, excluir}`
- `perfis.{visualizar, editar, excluir}`
- `pessoas.{visualizar, editar, excluir}`
- `organizacoes.{visualizar, editar, excluir}`
- `organizacoes.unidades.{visualizar, editar, excluir}`
- `organizacoes.unidades.setores.{visualizar, editar, excluir}`

**Definições (Catálogos):**
- `catalogos.documentoTipo.{visualizar, editar, excluir}`
- `catalogos.naturezaJuridica.{visualizar, editar, excluir}`
- `catalogos.nivelGoverno.{visualizar, editar, excluir}`
- `catalogos.situacao.{visualizar, editar, excluir}`
- `catalogos.tipoContato.{visualizar, editar, excluir}`
- `catalogos.tipoEndereco.{visualizar, editar, excluir}`
- `catalogos.tipoImovel.{visualizar, editar, excluir}`
- `catalogos.tipoUnidade.{visualizar, editar, excluir}`

**Endereçamento:**
- `enderecos.{visualizar, editar, excluir}`
- `enderecos.bairros.{visualizar, editar, excluir}`
- `enderecos.ceps.{visualizar, editar, excluir}`
- `enderecos.imoveis.{visualizar, editar, excluir}`
- `enderecos.logradouros.{visualizar, editar, excluir}`
- `enderecos.municipios.{visualizar, editar, excluir}`
- `enderecos.paises.{visualizar, editar, excluir}`
- `enderecos.ufs.{visualizar, editar, excluir}`

**Automação:**
- `orquestracao.processos.{visualizar, editar, editar_fluxo, excluir}`

---

## ❌ Erros Comuns

### ❌ Erro 1: Passar boolean em vez de função
```javascript
// ERRADO
<TelaPesquisa temPermissao={true} />

// CORRETO
const { temPermissao } = usePermissoes();
<TelaPesquisa temPermissao={temPermissao} />
```

### ❌ Erro 2: Usar nome antigo de permissão
```javascript
// ERRADO (nomes antigos)
temPermissao('usuarios.criar')
temPermissao('usuarios.deletar')
temPermissao('usuarios.restaurar')

// CORRETO (nomes novos, alinhados com backend)
temPermissao('usuarios.editar')      // para criar e editar
temPermissao('usuarios.excluir')     // para deletar
temPermissao('organizacoes.editar')  // para restaurar
```

### ❌ Erro 3: Não passar item para validar status
```javascript
// ERRADO - vai considerar que todas as ações são visíveis
acaoEstaVisivel(acao, temPermissao)

// CORRETO - valida se item está ativo/inativo
acaoEstaVisivel(acao, temPermissao, item)
```

---

## 🧪 Testando

### Manual
1. Fazer login como **admin**
   - Resultado: Todos os botões aparecem ✓

2. Fazer login como **usuário normal** sem permissões
   - Resultado: Nenhum botão de ação aparece ✓

3. Editar perfil do usuário para adicionar `usuarios.editar`
   - Resultado: Botão "Novo" e "Editar" aparecem ✓

4. Editar para adicionar `usuarios.excluir`
   - Resultado: Botão "Deletar" aparece ✓

### Automático
```javascript
// src/__tests__/validadorAcoes.test.js
describe('acaoEstaVisivel', () => {
  it('deve retornar true se admin user', () => {
    const temPermissao = () => true;
    const acao = { permissao: 'usuarios.editar' };
    expect(acaoEstaVisivel(acao, temPermissao)).toBe(true);
  });
  
  it('deve respeitar exibirQuandoAtivo', () => {
    const temPermissao = () => true;
    const item = { ativo: false };  // inativo!
    const acao = { permissao: 'usuarios.editar', exibirQuandoAtivo: true };
    expect(acaoEstaVisivel(acao, temPermissao, item)).toBe(false);
  });
});
```

---

## 📚 Estrutura de Metadados

Cada módulo tem um arquivo `pesquisa.json` com esta estrutura:

```json
{
  "modulo": "usuarios",
  "acoesFormulario": [
    {
      "tipo": "navegacao",
      "destino": "/usuarios/novo",
      "icone": "bi-plus",
      "permissao": "usuarios.editar"
    }
  ],
  "tabela": {
    "colunas": [...],
    "acoes": [
      {
        "tipo": "navegacao",
        "destino": "/usuarios/{{id}}",
        "icone": "pencil",
        "permissao": "usuarios.editar",
        "exibirQuandoAtivo": true
      },
      {
        "tipo": "confirmacao_delete_ajax",
        "destino": "/api/usuarios/{{id}}",
        "icone": "trash",
        "permissao": "usuarios.excluir",
        "exibirQuandoAtivo": true
      },
      {
        "tipo": "confirmacao_post_ajax",
        "destino": "/api/usuarios/{{id}}/restaurar",
        "icone": "arrow-counterclockwise",
        "permissao": "usuarios.editar",
        "exibirQuandoInativo": true
      }
    ]
  }
}
```

---

## 🚀 Conclusão

O sistema está **100% funcional** e pronto para:
- ✅ Validar permissões em tempo real
- ✅ Renderizar apenas ações permitidas
- ✅ Sincronizar com backend via `/auth/me`
- ✅ Respeitar admin bypass automático
- ✅ Suportar regras complexas (status, query params)

**Próximo passo:** Implementar a mesma validação em formulários (cadastro.json) e seções do sistema.
