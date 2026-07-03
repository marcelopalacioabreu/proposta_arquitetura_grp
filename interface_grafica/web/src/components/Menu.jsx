import React, { useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import axios from 'axios'

export default function Menu(){
  const [modulos, setModulos] = useState([])
  const [collapsed, setCollapsed] = useState(() => {
    try{ const v = localStorage.getItem('sidebarCollapsed'); return v === null ? true : v === 'true' }catch{ return true }
  })
  const [expandedOverlay, setExpandedOverlay] = useState(false)

  useEffect(()=>{
    axios.get('/meta/modulos').then(r=> setModulos(r.data.modulos || [] )).catch(()=>setModulos([]))
  },[])

  useEffect(()=>{
    try{ localStorage.setItem('sidebarCollapsed', collapsed ? 'true' : 'false') }catch{}
  },[collapsed])

  function openOverlay(){
    setExpandedOverlay(true)
    setCollapsed(false)
  }

  function closeOverlay(){
    setExpandedOverlay(false)
    try{ const v = localStorage.getItem('sidebarCollapsed'); setCollapsed(v === null ? true : v === 'true') }catch{ setCollapsed(true) }
  }

  return (
    <div className={`sidebar-wrapper ${collapsed ? 'sidebar-collapsed' : 'sidebar-collapsed-not'} ${expandedOverlay ? 'sidebar-expanded-overlay' : ''}`}>
      <div className="sidebar-header d-flex align-items-center justify-content-between px-2">
        <div className="d-flex align-items-center">
          <button className="btn btn-sm btn-outline-secondary me-2" onClick={()=>{ if (expandedOverlay) closeOverlay(); else setCollapsed(c=>!c) }} title={collapsed ? 'Expandir' : 'Colapsar'}>
            <i className={`bi bi-chevron-${collapsed ? 'right' : 'left'}`} />
          </button>
          {!collapsed && <div className="small text-muted">Navegação</div>}
        </div>
        <div>
          {collapsed && <button className="btn btn-sm btn-outline-primary" onClick={openOverlay} title="Abrir menu"> <i className="bi bi-grid-fill"/> </button>}
        </div>
      </div>

      <div className="menu list-group list-group-flush">
        {modulos.map((g, gi) => (
          <div key={gi} className="mb-2">
            {!collapsed && <div className="small text-muted px-2">{g.group}</div>}
            {g.items && g.items.map((it, ii) => (
              <Link key={ii} title={it.label} className="list-group-item list-group-item-action d-flex align-items-center" to={it.url}>
                {it.icon && <i className={`bi bi-${it.icon} me-2`} />}
                <span className="item-label">{it.label}</span>
              </Link>
            ))}
          </div>
        ))}
      </div>

      {expandedOverlay && (
        <>
          <div className="sidebar-overlay-backdrop" onClick={closeOverlay} />
          <div className="sidebar-overlay">
            <div className="p-3">
              {modulos.map((g, gi) => (
                <div key={gi} className="mb-3">
                  <div className="small text-muted mb-2">{g.group}</div>
                  <div className="list-group">
                    {g.items && g.items.map((it, ii) => (
                      <Link key={ii} className="list-group-item list-group-item-action d-flex align-items-center" to={it.url} onClick={closeOverlay}>
                        {it.icon && <i className={`bi bi-${it.icon} me-2`} />}
                        <span>{it.label}</span>
                      </Link>
                    ))}
                  </div>
                </div>
              ))}
            </div>
          </div>
        </>
      )}
    </div>
  )
}
