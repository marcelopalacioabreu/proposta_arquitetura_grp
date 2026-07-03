import React, { useEffect, useState, useMemo } from 'react'
import { Link, useLocation } from 'react-router-dom'
import axios from 'axios'

export default function Menu(){
  const [modulos, setModulos] = useState([])
  const [overlayOpen, setOverlayOpen] = useState(false)
  const [query, setQuery] = useState('')
  const location = useLocation()

  useEffect(()=>{
    axios.get('/meta/modulos').then(r=> setModulos(r.data.modulos || [] )).catch(()=>setModulos([]))
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
      <div className="floating-bar" aria-hidden={overlayOpen}>
        <div className="floating-buttons d-flex flex-column align-items-center">
          <button className="btn btn-dark btn-sm mb-2" onClick={()=> setOverlayOpen(true)} title="Abrir menu"><i className="bi bi-list"/></button>
          {modulos.map((g,gi)=> (
            (g.items||[]).map((it,ii)=> (
              <Link key={`${gi}-${ii}`} to={it.url} className="btn btn-light btn-sm mb-2 icon-btn" title={it.label}>
                {it.icon ? <i className={`bi bi-${it.icon}`}/> : <i className="bi bi-dot"/>}
              </Link>
            ))
          ))}
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
