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

  return (
    <>
      {/* Floating collapsed bar (icons only) */}
      <div className={`floating-bar ${compact ? 'compact' : 'expanded'}`}>
        <div className="floating-buttons d-flex flex-column">
          <div className="d-flex justify-content-center">
            <button className="main-square btn btn-light mb-2" onClick={()=> setCompact(c=>!c)} title={compact ? 'Expandir menu' : 'Fechar menu'}>
              <i className="bi bi-list"/>
            </button>
          </div>

          {!compact && (
            <div className="menu-panel">
              <div className="p-2">
                <input className="form-control form-control-sm mb-2" placeholder="Pesquisar menu..." value={query} onChange={e=> setQuery(e.target.value)} />
                {filtered.map((g, gi) => (
                  <div key={gi} className="mb-2">
                    <div className="small text-muted mb-1 px-1">{g.group}</div>
                    <div className="list-group">
                      {g.items && g.items.map((it, ii) => (
                        <Link key={ii} to={it.url} className="list-group-item list-group-item-action d-flex align-items-center" onClick={()=>{}}
                        >
                          {it.icon && <i className={`bi bi-${it.icon} me-2`} />}<span>{it.label}</span>
                        </Link>
                      ))}
                    </div>
                  </div>
                ))}
              </div>
            </div>
          )}

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
