import React, { useEffect, useState, useMemo } from 'react'
import { Link, useLocation } from 'react-router-dom'
import axios from 'axios'

export default function Menu(){
  const [modulos, setModulos] = useState([])
  const [overlayOpen, setOverlayOpen] = useState(false)
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

  // close overlay when navigating
  useEffect(()=>{ setOverlayOpen(false); setQuery('') },[location.pathname])

  const filtered = useMemo(()=>{
    if (!query) return modulos
    const q = query.toLowerCase()
    return modulos.map(g => ({ ...g, items: (g.items || []).filter(i => (i.label||'').toLowerCase().includes(q)) })).filter(g => (g.items || []).length > 0)
  },[modulos, query])

  return (
    <>
      {/* Floating collapsed bar (icons only) */}
      <div className={`floating-bar ${compact ? 'compact' : ''}`} aria-hidden={overlayOpen}>
        <div className="floating-buttons d-flex flex-column align-items-center">
          <button className="main-square btn btn-light mb-2" onClick={()=> setOverlayOpen(true)} title="Abrir menu">
            <i className="bi bi-list"/>
          </button>
          <div className="icon-list">
            {modulos.map((g,gi)=> (
              (g.items||[]).map((it,ii)=> (
                <Link key={`${gi}-${ii}`} to={it.url} className="btn btn-light btn-sm mb-2 icon-btn" title={it.label}>
                  {it.icon ? <i className={`bi bi-${it.icon}`}/> : <i className="bi bi-dot"/>}
                </Link>
              ))
            ))}
          </div>
          <button className="btn btn-outline-secondary btn-sm mt-auto toggle-compact" onClick={()=> setCompact(c=>!c)} title={compact ? 'Expandir barra' : 'Compactar barra'}>
            <i className={`bi bi-${compact ? 'chevron-bar-right' : 'chevron-bar-left'}`}></i>
          </button>
        </div>
      </div>

      {/* Overlay panel */}
      {overlayOpen && (
        <>
          <div className="menu-overlay-backdrop" onClick={()=> setOverlayOpen(false)} />
          <div className="menu-overlay" role="dialog" aria-modal="true">
            <div className="overlay-header p-3 d-flex align-items-center">
              <div className="flex-fill">
                <input className="form-control" placeholder="Pesquisar menu..." value={query} onChange={e=> setQuery(e.target.value)} />
              </div>
              <div className="ms-2">
                <button className="btn btn-outline-secondary" onClick={()=> setOverlayOpen(false)} title="Fechar"><i className="bi bi-x"/></button>
              </div>
            </div>
            <div className="overlay-body p-3">
              {filtered.map((g, gi) => (
                <div key={gi} className="mb-3">
                  <div className="small text-muted mb-2">{g.group}</div>
                  <div className="list-group">
                    {g.items && g.items.map((it, ii) => (
                      <Link key={ii} className="list-group-item list-group-item-action d-flex align-items-center" to={it.url} onClick={()=> setOverlayOpen(false)}>
                        {it.icon && <i className={`bi bi-${it.icon} me-2`} />}<span>{it.label}</span>
                      </Link>
                    ))}
                  </div>
                </div>
              ))}
            </div>
          </div>
        </>
      )}
    </>
  )
}
