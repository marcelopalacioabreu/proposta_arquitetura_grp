import { useEffect, useState } from 'react'
import api from './api'

/**
 * Hook customizado para carregar e validar permissões do usuário
 * 
 * **IMPORTANTE:** Se o usuário for administrador do sistema, TODAS as validações de permissão
 * retornam true (admins têm acesso a tudo).
 * 
 * @returns {Object} { 
 *   permissoes: string[], 
 *   administrador: boolean,
 *   temPermissao: function, 
 *   loading: boolean 
 * }
 * 
 * @example
 * const { permissoes, administrador, temPermissao, loading } = usePermissoes()
 * 
 * if (loading) return <div>Carregando...</div>
 * 
 * if (temPermissao('organizacoes.editar')) {
 *   // Renderizar botão de editar
 *   // Retorna true se:
 *   // 1. Usuario é administrador (administrador === true)
 *   // 2. OU usuário possui a permissão específica
 * }
 * 
 * // Validar múltiplas permissões (AND logic)
 * if (temPermissao(['usuarios.listar', 'usuarios.deletar'])) {
 *   // Retorna true se admin OU possui TODAS as permissões
 * }
 */
export default function usePermissoes(){
  const [permissoes, setPermissoes] = useState([])
  const [administrador, setAdministrador] = useState(false)
  const [loading, setLoading] = useState(true)

  useEffect(()=>{
    let mounted = true

    api.get('/auth/me', { block: false })
      .then(r => {
        if (!mounted) return
        
        const data = r.data || {}
        const perms = Array.isArray(data.permissoes) ? data.permissoes : []
        const isAdmin = data.administrador === true
        
        // Normalizar: converter numbers para strings
        const normalized = perms.map(p => String(p))
        
        setPermissoes(normalized)
        setAdministrador(isAdmin)
        setLoading(false)
      })
      .catch(() => {
        if (mounted) {
          setPermissoes([])
          setAdministrador(false)
          setLoading(false)
        }
      })

    return () => { mounted = false }
  },[])

  /**
   * Verifica se usuário possui uma ou mais permissões
   * 
   * **Regra de admin:** Se usuário é administrador do sistema, sempre retorna true
   * (admins têm acesso a TUDO).
   * 
   * @param {string|string[]} codigosPermissao - ID(s) da(s) permissão(ões)
   * @returns {boolean} true se:
   *   - Sem codigoPermissao = sempre true (ação pública)
   *   - Usuario é administrador = sempre true
   *   - String: possui essa permissão
   *   - Array: possui TODAS as permissões (AND logic)
   */
  function temPermissao(codigosPermissao){
    if (!codigosPermissao) return true // Sem restrição

    // Regra de admin: administrador tem acesso a tudo
    if (administrador) return true

    // Se for string única
    if (typeof codigosPermissao === 'string'){
      return permissoes.includes(String(codigosPermissao))
    }

    // Se for array - verifica se possui TODAS (AND logic)
    if (Array.isArray(codigosPermissao)){
      return codigosPermissao.every(p => permissoes.includes(String(p)))
    }

    return false
  }

  return {
    permissoes,
    administrador,
    temPermissao,
    loading,
    // Conveniência: retornar também um objeto para spread se necessário
    carregado: !loading,
  }
}
