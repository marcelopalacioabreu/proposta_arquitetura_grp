/**
 * Validador de Ações com Permissões
 * 
 * Centraliza a lógica de validação de permissões para ações
 * em telas de pesquisa, cadastro, etc.
 * 
 * **IMPORTANTE - Regra de Admin:**
 * Se o usuário for administrador do sistema (administrador === true),
 * TODAS as validações de permissão retornam true automaticamente.
 * Admins têm acesso irrestrito a todas as ações.
 */

/**
 * Verifica se uma ação deve ser visível baseado em permissões
 * 
 * **Regra de Admin:** Se usuário é admin, a validação de permissão sempre passa.
 * 
 * @param {Object} acao - Objeto de ação dos metadados
 * @param {string} [acao.permissao] - ID da permissão necessária para exibir a ação
 * @param {boolean} [acao.exibirQuandoAtivo] - Exibir apenas quando item está ativo
 * @param {boolean} [acao.exibirQuandoInativo] - Exibir apenas quando item está inativo
 * @param {string} [acao.exibirQuandoQuery] - Exibir apenas se querystring atender condição (ex: 'inativo=1')
 * 
 * @param {Function} temPermissao - Função que retorna true se usuário tem permissão.
 *                                   Deve já considerar a regra de admin internamente.
 *                                   Assinatura: temPermissao(codigoPermissao: string|string[]) => boolean
 * @param {boolean} itemAtivo - true se o item está ativo
 * @param {URLSearchParams|Object} queryParams - Parâmetros da query string
 * 
 * @returns {boolean} true se ação deve ser exibida
 * 
 * @example
 * const visivel = validarAcao(
 *   acao, 
 *   (perm) => temPermissao(perm),  // já considera admin internamente
 *   true, 
 *   query
 * )
 */
export function validarAcao(acao, temPermissao, itemAtivo = true, queryParams = null){
  if (!acao) return false

  // Validar permissão (se definida)
  // temPermissao() já retorna true se admin, então não precisa verificar aqui
  if (acao.permissao && !temPermissao(acao.permissao)){
    return false
  }

  // Validar visibilidade por status do item
  if (acao.exibirQuandoAtivo === true && !itemAtivo){
    return false
  }

  if (acao.exibirQuandoInativo === true && itemAtivo){
    return false
  }

  // Validar condições de querystring
  if (acao.exibirQuandoQuery && queryParams){
    try{
      const qp = queryParams instanceof URLSearchParams 
        ? queryParams 
        : new URLSearchParams(Object.entries(queryParams || {}))
      
      const parts = (acao.exibirQuandoQuery || '').split('&').map(p => p.trim()).filter(Boolean)
      
      for (const part of parts){
        const [key, val] = part.split('=')
        if (val !== undefined){
          if (qp.get(key) !== val) return false
        } else {
          if (!qp.has(part)) return false
        }
      }
    } catch(e){
      console.warn('Erro ao validar exibirQuandoQuery:', e)
      return false
    }
  }

  return true
}

/**
 * Determina se um item está ativo ou inativo
 * Suporta campos padrão: 'ativo' e 'inativo'
 * 
 * @param {Object} item - O item/registro
 * @returns {boolean} true se item está ativo
 */
export function itemEstaAtivo(item){
  if (!item) return true

  // Campo 'ativo' = true significa ativo
  if (Object.prototype.hasOwnProperty.call(item, 'ativo')){
    return item.ativo !== false && item.ativo !== '0' && item.ativo !== 0
  }

  // Campo 'inativo' = false/0 significa ativo
  if (Object.prototype.hasOwnProperty.call(item, 'inativo')){
    return !(item.inativo === true || item.inativo === '1' || item.inativo === 1)
  }

  return true
}

/**
 * Filtra um array de ações apenas as que o usuário pode ver
 * 
 * **Regra de Admin:** Se usuário é admin, todas as ações aparecem.
 * 
 * @param {Object[]} acoes - Array de ações dos metadados
 * @param {Function} temPermissao - Função de validação temPermissao(codigoPermissao).
 *                                   Deve já considerar regra de admin internamente.
 * @param {Object} [item] - Item/registro para validar status (ativo/inativo)
 * @param {URLSearchParams} [queryParams] - Parâmetros da querystring
 * 
 * @returns {Object[]} Array de ações visíveis
 */
export function filtrarAcoes(acoes, temPermissao, item = null, queryParams = null){
  if (!Array.isArray(acoes)) return []

  return acoes.filter(acao => {
    const permOk = !acao.permissao || temPermissao(acao.permissao)
    const ativoOk = validarAcao(acao, temPermissao, itemEstaAtivo(item), queryParams)
    return ativoOk && permOk
  })
}

/**
 * Valida uma ação individual completa (permissão + visibilidade)
 * 
 * **Regra de Admin:** Se usuário é admin, validação de permissão sempre passa.
 * Admins têm acesso irrestrito a todas as ações.
 * 
 * @param {Object} acao - A ação a validar
 * @param {Function} temPermissao - Função de validação temPermissao(codigoPermissao).
 *                                   Deve já considerar regra de admin internamente.
 * @param {Object} [item] - Item para validar status
 * @param {URLSearchParams} [queryParams] - Parâmetros da querystring
 * 
 * @returns {boolean} true se ação é visível
 */
export function acaoEstaVisivel(acao, temPermissao, item = null, queryParams = null){
  if (!acao) return false
  
  // Validar permissão
  if (acao.permissao){
    if (!temPermissao(acao.permissao)) return false
  }

  // Validar status do item
  const ativo = itemEstaAtivo(item)
  if (acao.exibirQuandoAtivo === true && !ativo) return false
  if (acao.exibirQuandoInativo === true && ativo) return false

  // Validar querystring
  if (acao.exibirQuandoQuery && queryParams){
    try{
      const qp = queryParams instanceof URLSearchParams 
        ? queryParams 
        : new URLSearchParams(Object.entries(queryParams || {}))
      
      const parts = (acao.exibirQuandoQuery || '').split('&').map(p => p.trim()).filter(Boolean)
      
      for (const part of parts){
        const [key, val] = part.split('=')
        if (val !== undefined){
          if (qp.get(key) !== val) return false
        } else {
          if (!qp.has(part)) return false
        }
      }
    } catch(e){
      return false
    }
  }

  return true
}
