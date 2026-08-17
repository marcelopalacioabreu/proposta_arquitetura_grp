-- ============================================================================
-- SQL Script: Atualizar PerfilPermissoes com Permissões de Permissões Frontend
-- ============================================================================
-- 
-- Este script insere todas as 48 permissões no banco de dados.
-- Execute este script após sincronizar permissões frontend/backend/modulos.json
--
-- Pré-requisitos:
-- - Tabela PerfilPermissoes deve existir
-- - Tabela Perfis deve ter pelo menos um perfil (ex: ID=1 para Admin)
--
-- ============================================================================

-- PASSO 1: Limpar permissões antigas (OPCIONAL - comentado por segurança)
-- DELETE FROM PerfilPermissoes WHERE Nome LIKE '%.editar' OR Nome LIKE '%.excluir' OR Nome LIKE '%.visualizar';

-- PASSO 2: Inserir todas as 48 permissões para o Admin (PerfilId=1)
-- ============================================================================

-- ADMINISTRATIVO (21 permissões)
INSERT INTO PerfilPermissoes (PerfilId, Nome) VALUES
(1, 'organizacoes.visualizar'),
(1, 'organizacoes.editar'),
(1, 'organizacoes.excluir'),
(1, 'organizacoes.unidades.visualizar'),
(1, 'organizacoes.unidades.editar'),
(1, 'organizacoes.unidades.excluir'),
(1, 'organizacoes.unidades.setores.visualizar'),
(1, 'organizacoes.unidades.setores.editar'),
(1, 'organizacoes.unidades.setores.excluir'),
(1, 'usuarios.visualizar'),
(1, 'usuarios.editar'),
(1, 'usuarios.excluir'),
(1, 'perfis.visualizar'),
(1, 'perfis.editar'),
(1, 'perfis.excluir'),
(1, 'pessoas.visualizar'),
(1, 'pessoas.editar'),
(1, 'pessoas.excluir');

-- DEFINIÇÕES - CATÁLOGOS (24 permissões)
INSERT INTO PerfilPermissoes (PerfilId, Nome) VALUES
(1, 'catalogos.documentoTipo.visualizar'),
(1, 'catalogos.documentoTipo.editar'),
(1, 'catalogos.documentoTipo.excluir'),
(1, 'catalogos.naturezaJuridica.visualizar'),
(1, 'catalogos.naturezaJuridica.editar'),
(1, 'catalogos.naturezaJuridica.excluir'),
(1, 'catalogos.nivelGoverno.visualizar'),
(1, 'catalogos.nivelGoverno.editar'),
(1, 'catalogos.nivelGoverno.excluir'),
(1, 'catalogos.situacao.visualizar'),
(1, 'catalogos.situacao.editar'),
(1, 'catalogos.situacao.excluir'),
(1, 'catalogos.tipoContato.visualizar'),
(1, 'catalogos.tipoContato.editar'),
(1, 'catalogos.tipoContato.excluir'),
(1, 'catalogos.tipoEndereco.visualizar'),
(1, 'catalogos.tipoEndereco.editar'),
(1, 'catalogos.tipoEndereco.excluir'),
(1, 'catalogos.tipoImovel.visualizar'),
(1, 'catalogos.tipoImovel.editar'),
(1, 'catalogos.tipoImovel.excluir'),
(1, 'catalogos.tipoUnidade.visualizar'),
(1, 'catalogos.tipoUnidade.editar'),
(1, 'catalogos.tipoUnidade.excluir');

-- ENDEREÇAMENTO (24 permissões)
INSERT INTO PerfilPermissoes (PerfilId, Nome) VALUES
(1, 'enderecos.visualizar'),
(1, 'enderecos.editar'),
(1, 'enderecos.excluir'),
(1, 'enderecos.bairros.visualizar'),
(1, 'enderecos.bairros.editar'),
(1, 'enderecos.bairros.excluir'),
(1, 'enderecos.ceps.visualizar'),
(1, 'enderecos.ceps.editar'),
(1, 'enderecos.ceps.excluir'),
(1, 'enderecos.imoveis.visualizar'),
(1, 'enderecos.imoveis.editar'),
(1, 'enderecos.imoveis.excluir'),
(1, 'enderecos.logradouros.visualizar'),
(1, 'enderecos.logradouros.editar'),
(1, 'enderecos.logradouros.excluir'),
(1, 'enderecos.municipios.visualizar'),
(1, 'enderecos.municipios.editar'),
(1, 'enderecos.municipios.excluir'),
(1, 'enderecos.paises.visualizar'),
(1, 'enderecos.paises.editar'),
(1, 'enderecos.paises.excluir'),
(1, 'enderecos.ufs.visualizar'),
(1, 'enderecos.ufs.editar'),
(1, 'enderecos.ufs.excluir');

-- AUTOMAÇÃO (4 permissões)
INSERT INTO PerfilPermissoes (PerfilId, Nome) VALUES
(1, 'orquestracao.processos.visualizar'),
(1, 'orquestracao.processos.editar'),
(1, 'orquestracao.processos.editar_fluxo'),
(1, 'orquestracao.processos.excluir');

-- PASSO 3: Remover duplicatas (se houver)
-- ============================================================================
-- DELETE FROM PerfilPermissoes
-- WHERE Id NOT IN (
--   SELECT MIN(Id)
--   FROM PerfilPermissoes
--   GROUP BY PerfilId, Nome
-- );

-- PASSO 4: Verificar se tudo foi inserido corretamente
-- ============================================================================
SELECT COUNT(*) AS 'Total de Permissões' FROM PerfilPermissoes WHERE PerfilId = 1;

-- Resultado esperado: 48

-- Para listar todas:
SELECT * FROM PerfilPermissoes WHERE PerfilId = 1 ORDER BY Nome;

-- ============================================================================
-- NOTAS:
-- ============================================================================
-- 1. Execute PASSO 2 para Admin (PerfilId=1)
-- 2. Para outros perfis, ajuste PerfilId conforme necessário
-- 3. Se usar diferentes bancos (PostgreSQL, MySQL), sintaxe pode variar
-- 4. Admin bypass automático no backend não depende destas permissões
--    (PermissionAuthorizationHandler checks IsUserAdministratorAsync FIRST)
-- 5. Permissões normais de usuários devem estar nesta tabela para validação
--
-- ============================================================================
