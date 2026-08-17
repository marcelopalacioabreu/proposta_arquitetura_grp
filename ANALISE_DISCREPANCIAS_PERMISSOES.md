# Análise de Discrepâncias - Permissões Frontend vs modulos.json

## 📊 Resumo

- **Permissões usadas em pesquisa.json:** 73 permissões
- **Permissões definidas em modulos.json:** 60 permissões
- **Discrepâncias encontradas:** 42 permissões não definidas

## 🔴 Permissões Ausentes em modulos.json

### 1. Operações "criar" (CREATE) - Ausentes Completamente
```
catalogos.documentoTipo.criar ❌ (modulos tem: documentoTipo.editar)
catalogos.naturezaJuridica.criar ❌ (modulos tem: natureza.editar)
catalogos.nivelGoverno.criar ❌ (modulos tem: nivelgov.editar)
catalogos.situacao.criar ❌ (modulos tem: situacao.editar)
catalogos.tipoContato.criar ❌ (modulos tem: tipoContato.editar)
catalogos.tipoEndereco.criar ❌ (modulos tem: tipoEndereco.editar)
catalogos.tipoImovel.criar ❌ (modulos tem: tipoImovel.editar)
catalogos.tipoUnidade.criar ❌ (modulos tem: tipoUnidade.editar)
enderecos.criar ❌
enderecos.bairros.criar ❌
enderecos.ceps.criar ❌
enderecos.imoveis.criar ❌
enderecos.logradouros.criar ❌
enderecos.municipios.criar ❌
enderecos.paises.criar ❌
enderecos.ufs.criar ❌
organizacoes.criar ❌
organizacoes.unidades.criar ❌
organizacoes.unidades.setores.criar ❌
orquestracao.processos.criar ❌
perfis.criar ❌
pessoas.criar ❌
usuarios.criar ❌
```

### 2. Operações "deletar" (DELETE) - Usando Nome Diferente
```
catalogos.documentoTipo.deletar ❌ (modulos usa: documentoTipo.excluir)
catalogos.naturezaJuridica.deletar ❌ (modulos usa: natureza.excluir)
catalogos.nivelGoverno.deletar ❌ (modulos usa: nivelgov.excluir)
catalogos.situacao.deletar ❌ (modulos usa: situacao.excluir)
catalogos.tipoContato.deletar ❌ (modulos usa: tipoContato.excluir)
catalogos.tipoEndereco.deletar ❌ (modulos usa: tipoEndereco.excluir)
catalogos.tipoImovel.deletar ❌ (modulos usa: tipoImovel.excluir)
catalogos.tipoUnidade.deletar ❌ (modulos usa: tipoUnidade.excluir)
enderecos.deletar ❌ (modulos usa: enderecos.excluir)
enderecos.bairros.deletar ❌ (modulos usa: bairros.excluir)
enderecos.ceps.deletar ❌ (modulos usa: ceps.excluir)
enderecos.imoveis.deletar ❌ (modulos usa: imoveis.excluir)
enderecos.logradouros.deletar ❌ (modulos usa: logradouros.excluir)
enderecos.municipios.deletar ❌ (modulos usa: municipios.excluir)
enderecos.paises.deletar ❌ (modulos usa: paises.excluir)
enderecos.ufs.deletar ❌ (modulos usa: ufs.excluir)
organizacoes.deletar ❌
perfis.deletar ❌
pessoas.deletar ❌
usuarios.deletar ❌
```

### 3. Operações "restaurar" (RESTORE) - Ausentes Completamente
```
organizacoes.restaurar ❌
organizacoes.unidades.restaurar ❌
organizacoes.unidades.setores.restaurar ❌
perfis.restaurar ❌
pessoas.restaurar ❌
usuarios.restaurar ❌
```

### 4. Outras Operações Ausentes
```
organizacoes.unidades.listar ❌
organizacoes.unidades.setores.listar ❌
orquestracao.processos.editar_fluxo ❌ (modulos tem: orquestracaoFluxo.editar)
```

## 🟡 Problemas de Nomenclatura/Estrutura

### Catálogos - Estrutura "catalogos.X" não existem em modulos.json
Pesquisa usa: `catalogos.documentoTipo.*`
Modulos define: `documentoTipo.*` (sem prefixo "catalogos")

### Catalogo: Nível de Governo
Pesquisa usa: `catalogos.nivelGoverno.editar`
Modulos define: `nivelgov.editar` (forma abreviada)

### Catálogo: Natureza Jurídica
Pesquisa usa: `catalogos.naturezaJuridica.editar`
Modulos define: `natureza.editar` (forma abreviada)

### Endereços - Estrutura "enderecos.TIPO" não completa em modulos.json
Pesquisa usa: `enderecos.bairros.criar`, `enderecos.bairros.editar`, `enderecos.bairros.deletar`
Modulos define: `bairros.editar`, `bairros.excluir`, `bairros.visualizar` (sem prefixo "enderecos")

### Orquestração
Pesquisa usa: `orquestracao.processos.editar`
Modulos define: `orquestracaoFluxo.editar` (nome diferente: "processos" vs "Fluxo")

## ✅ Permissões Que Existem (Com Ressalvas)

### Entidades Principais - Faltam "criar" e "deletar"/"restaurar"
```
usuarios.editar ✅ (modulos tem)
usuarios.criar ❌ (não tem - seria "editar"?)
usuarios.deletar ❌ (não tem)
usuarios.restaurar ❌ (não tem)

perfis.editar ✅ (modulos tem)
perfis.criar ❌
perfis.deletar ❌
perfis.restaurar ❌

pessoas.editar ✅ (modulos tem)
pessoas.criar ❌
pessoas.deletar ❌
pessoas.restaurar ❌

organizacoes.editar ✅ (modulos tem)
organizacoes.criar ❌
organizacoes.deletar ❌
organizacoes.restaurar ❌
```

## 📋 Recomendações

### Opção 1: Atualizar modulos.json (Recomendado)
Adicionar as permissões faltantes em modulos.json:

```json
{
  "chave": "usuarios",
  "permissoes": [
    { "id": "usuarios.visualizar", "texto": "Visualizar" },
    { "id": "usuarios.criar", "texto": "Criar" },
    { "id": "usuarios.editar", "texto": "Editar" },
    { "id": "usuarios.deletar", "texto": "Deletar" },
    { "id": "usuarios.restaurar", "texto": "Restaurar" }
  ]
}
```

**Passos:**
1. ✅ Adicionar "criar" em todas as entidades principais
2. ✅ Adicionar "deletar" + "restaurar" em todas as entidades
3. ✅ Adicionar prefixo "catalogos." às permissões de catálogos
4. ✅ Normalizar nomes de permissões (usar "deletar" ao invés de "excluir" OR vice-versa)
5. ✅ Adicionar estrutura "enderecos.X" para endereços
6. ✅ Corrigir nomes de abreviações (natureza_juridica → naturezaJuridica, etc)

### Opção 2: Atualizar pesquisa.json (Não Recomendado)
Mudar todas as permissões para usar os nomes definidos em modulos.json
- Mais trabalho (27 arquivos)
- Menos consistente com a API
- Seria necessário também atualizar o backend

### Opção 3: Usar Padrão "criar"/"deletar"/"restaurar" Sem Validação Backend
Frontend valida, mas backend não reconhece → Segurança Comprometida ❌

## 🔒 Risco de Segurança

⚠️ **CRÍTICO:** Se permissões não forem definidas corretamente em modulos.json:

1. **Frontend** valida permissões contra lista carregada do `/auth/me`
2. **Backend** precisa reconhecer essas mesmas permissões
3. Se backend não definir permissão, usuário pode conseguir acesso via API sem passar pela UI

**Solução:** Sincronizar modulos.json com permissões realmente implementadas no backend.

## 📝 Próximos Passos

1. Revisar arquivo de permissões no backend (likely: `Authorization/PermissionHandler.cs` ou similar)
2. Verificar quais permissões o backend realmente define
3. Atualizar modulos.json para incluir TODAS as permissões
4. Testar com admin (deve sempre passar) e usuário normal (deve respeitar permissões)
