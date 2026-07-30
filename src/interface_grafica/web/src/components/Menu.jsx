import React, { useEffect, useState, useMemo } from 'react'
import { Link, useLocation } from 'react-router-dom'
import api from '../services/api'

export default function Menu(){
  const [modulos, setModulos] = useState([])
  const [userPerms, setUserPerms] = useState([])
  const [query, setQuery] = useState('')
  const [compact, setCompact] = useState(true)
  const location = useLocation()

  useEffect(()=>{
    api.get('/meta/modulos').then(r=> setModulos(r.data.modulos || [] )).catch(()=>setModulos([]))
    api.get('/auth/me').then(r => {
      const data = r.data || null;
      if (data && data.permissoes) setUserPerms(data.permissoes || [])
    }).catch(()=> setUserPerms([]))
  },[])

  // Computa as variáveis CSS para posicionar o menu flutuante corretamente, considerando a altura da navbar e do footer
  useEffect(()=>{
    function updateVars(){
      const navs = Array.from(document.querySelectorAll('.navbar'))
      let nav = navs.find(n => n.getBoundingClientRect().height > 0)
      if (!nav) nav = navs[0]
      const footer = document.querySelector('.app-footer')
      const top = nav ? Math.round(nav.getBoundingClientRect().bottom) : 56
      let bottom = 0
      if (footer){
        const rect = footer.getBoundingClientRect()
        // só usa o bottom se o footer estiver visível na tela, caso contrário, o menu flutuante pode invadir o footer
        bottom = rect.top < window.innerHeight ? Math.round(window.innerHeight - rect.top) : 0
      }
      document.documentElement.style.setProperty('--menu-top', `${top}px`)
        document.documentElement.style.setProperty('--menu-bottom', `${Math.max(0, bottom)}px`)
    }
    updateVars()
    window.addEventListener('resize', updateVars)
      window.addEventListener('scroll', updateVars)
    const mo = new MutationObserver(updateVars)
    mo.observe(document.body, { childList: true, subtree: true })
    return ()=>{ window.removeEventListener('resize', updateVars); window.removeEventListener('scroll', updateVars); mo.disconnect() }
  },[])

  // Limpa a pesquisa ao mudar de rota, para não manter o filtro de pesquisa entre páginas
  useEffect(()=>{ setQuery('') },[location.pathname])

  const normalizeModulo = (g) => ({
    ...g,
    items: g.items || g.itens || [],
    group: g.group || g.grupo || ''
  })

  const filtered = useMemo(()=>{
    if (!query) {
      return modulos.map(normalizeModulo)
    }

    const q = query.toLowerCase()
    return modulos.map(normalizeModulo).map(g => ({ ...g, items: (g.items || []).filter(i => {
      // Pesquisa o texto
      if (!((i.texto||'').toLowerCase().includes(q))) return false
      
      // Valida as permissões      
      /** O administrador vê tudo mas não tem permissões... Por enquanto, vamos tratar na extremidade de metadados de módulos */
      /*
      if (i.permissoes && Array.isArray(i.permissoes) && i.permissoes.length > 0){
        const ids = i.permissoes.map(p => (p.id||p).toString())
        return ids.some(id => userPerms.includes(id))
      }
      */
      
      return true
    }) })).filter(g => (g.items || []).length > 0)

  },[modulos, query])

  function renderCompactIcon(name, active){
    if (!name) return <i className="bi bi-square" />
    const defaultColor = active ? '#fff' : '#495057'
    switch(name){
      case 'building':
        return (
          <svg width="20" height="20" viewBox="0 0 16 16" xmlns="http://www.w3.org/2000/svg" aria-hidden>
            <path fill={defaultColor} d="M14.5 13.5V2a1 1 0 0 0-1-1H2.5a1 1 0 0 0-1 1v11.5H.5a.5.5 0 0 0 0 1h15a.5.5 0 0 0 0-1h-1zM3 3h9v3H3V3zm0 4h2v2H3V7zm3 0h6v2H6V7zM3 10h2v2H3v-2z" />
          </svg>
        )
      default:
        return <i className={`bi bi-${name}`} />
    }
  }

  return (
    <>
      {/* Menu expansível flutuante */}
      <div className={`floating-bar ${compact ? 'compact' : 'expanded'}`}>
        <div className="floating-buttons d-flex flex-column">
          <div className="menu-panel">
            <div className={`p-2 ${compact ? 'compact-body' : ''}`}>
              {!compact ? (
                <>
                  <input className="form-control form-control-sm mb-2" placeholder="Pesquisar menu..." value={query} onChange={e=> setQuery(e.target.value)} />
                  {filtered.map((g, gi) => (
                    <div key={gi} className="mb-2">
                      <div className="small text-muted mb-1 px-1">{g.group}</div>
                      <div className="list-group">
                        {g.items && g.items.map((it, ii) => (
                          <Link key={ii} to={it.url} className={`list-group-item list-group-item-action d-flex align-items-center ${location.pathname === it.url ? 'active' : ''}`} onClick={()=>{}}
                          >
                            {it.icone && <i className={`bi bi-${it.icone} me-2`} />}
                            <span className="item-label">{it.texto}</span>
                          </Link>
                        ))}
                      </div>
                    </div>
                  ))}
                </>
              ) : (
                <div className="compact-icons d-flex flex-column align-items-center">
                  {filtered.flatMap(g => g.items || []).map((it, idx) => {
                    const isActive = location.pathname === it.url
                    return (
                      <Link key={idx} to={it.url} className={`compact-icon mb-2 ${isActive ? 'active' : ''}`} title={it.texto}>
                        {it.icone ? (renderCompactIcon(it.icone, isActive)) : <i className="bi bi-square"/>}
                      </Link>
                    )
                  })}
                </div>
              )}
            </div>
          </div>

          <div className="mt-auto d-flex justify-content-center w-100">
            <button className="btn btn-outline-secondary btn-sm toggle-compact" onClick={()=> setCompact(c=>!c)} title={compact ? 'Expandir barra' : 'Compactar barra'}>
              <i className={`bi bi-${compact ? 'chevron-bar-right' : 'chevron-bar-left'}`}></i>
            </button>
          </div>
        </div>
      </div>
    </>
  )
}
