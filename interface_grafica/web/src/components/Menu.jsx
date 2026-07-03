import React, { useEffect, useState, useMemo } from 'react'
import { Link, useLocation } from 'react-router-dom'
import axios from 'axios'

export default function Menu(){
  const [modulos, setModulos] = useState([])
  const [query, setQuery] = useState('')
  const [compact, setCompact] = useState(true)
  const location = useLocation()

  useEffect(()=>{
    axios.get('/meta/modulos').then(r=> setModulos(r.data.modulos || [] )).catch(()=>setModulos([]))
  },[])

  // compute menu top/bottom to fit exactly between navbar and footer
  useEffect(()=>{
    function updateVars(){
      const navs = Array.from(document.querySelectorAll('.navbar'))
      let nav = navs.find(n => n.getBoundingClientRect().height > 0)
      if (!nav) nav = navs[0]
      const footer = document.querySelector('.app-footer')
      const top = nav ? Math.round(nav.getBoundingClientRect().bottom) : 56
      let bottom = 60
      if (footer){
        const rect = footer.getBoundingClientRect()
        bottom = Math.round(window.innerHeight - rect.top)
      }
      document.documentElement.style.setProperty('--menu-top', `${top}px`)
      document.documentElement.style.setProperty('--menu-bottom', `${bottom}px`)
    }
    updateVars()
    window.addEventListener('resize', updateVars)
    const mo = new MutationObserver(updateVars)
    mo.observe(document.body, { childList: true, subtree: true })
    return ()=>{ window.removeEventListener('resize', updateVars); mo.disconnect() }
  },[])

  // clear search when navigating
  useEffect(()=>{ setQuery('') },[location.pathname])

  const filtered = useMemo(()=>{
    if (!query) return modulos
    const q = query.toLowerCase()
    return modulos.map(g => ({ ...g, items: (g.items || []).filter(i => (i.label||'').toLowerCase().includes(q)) })).filter(g => (g.items || []).length > 0)
  },[modulos, query])

  function renderCompactIcon(name){
    // small inline SVG fallbacks for critical icons to guarantee visibility
    if (!name) return <i className="bi bi-square" />
    switch(name){
      case 'building':
        return (
          <svg width="20" height="20" viewBox="0 0 16 16" fill="currentColor" xmlns="http://www.w3.org/2000/svg" aria-hidden>
            <path d="M14.5 13.5V2a1 1 0 0 0-1-1H2.5a1 1 0 0 0-1 1v11.5H.5a.5.5 0 0 0 0 1h15a.5.5 0 0 0 0-1h-1zM3 3h9v3H3V3zm0 4h2v2H3V7zm3 0h6v2H6V7zM3 10h2v2H3v-2z" />
          </svg>
        )
      default:
        return <i className={`bi bi-${name}`} />
    }
  }

  return (
    <>
      {/* Floating collapsed bar (icons only -> use same markup for both states) */}
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
                            {it.icon && <i className={`bi bi-${it.icon} me-2`} />}
                            <span className="item-label">{it.label}</span>
                          </Link>
                        ))}
                      </div>
                    </div>
                  ))}
                </>
              ) : (
                <div className="compact-icons d-flex flex-column align-items-center">
                  {filtered.flatMap(g => g.items || []).map((it, idx) => (
                    <Link key={idx} to={it.url} className={`compact-icon mb-2 ${location.pathname === it.url ? 'active' : ''}`} title={it.label}>
                      {it.icon ? (renderCompactIcon(it.icon)) : <i className="bi bi-square"/>}
                    </Link>
                  ))}
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
