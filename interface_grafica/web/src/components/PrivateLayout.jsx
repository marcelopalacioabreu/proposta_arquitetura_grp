import React, { useEffect, useState, useRef } from 'react'
import { Outlet, useNavigate } from 'react-router-dom'
import Menu from './Menu'
import axios from 'axios'

export default function PrivateLayout(){
  const [user, setUser] = useState(null)
  const [menuOpen, setMenuOpen] = useState(false)
  const [userMenuOpen, setUserMenuOpen] = useState(false)
  const navigate = useNavigate()
  const userMenuRef = useRef()

  useEffect(()=>{
    let mounted = true
    axios.get('/auth/me').then(r=>{ if (mounted) setUser(r.data) }).catch(()=>{ if (mounted) setUser(null) })
    return ()=> { mounted = false }
  },[])

  useEffect(()=>{
    function onDoc(e){ if (userMenuRef.current && !userMenuRef.current.contains(e.target)) setUserMenuOpen(false) }
    document.addEventListener('click', onDoc)
    return ()=> document.removeEventListener('click', onDoc)
  },[])

  async function handleLogout(){
    try{ await axios.post('/auth/logout'); setUser(null); navigate('/login') }catch{ navigate('/login') }
  }
  return (
    <div>
      <nav className="navbar navbar-light bg-light d-flex d-md-none">
        <div className="container-fluid">
          <button className="btn btn-outline-secondary" type="button" data-bs-toggle="offcanvas" data-bs-target="#sidebar" aria-controls="sidebar" onClick={()=> setMenuOpen(s=>!s)}>
            <i className="bi bi-list" />
          </button>
          <span className="navbar-brand ms-2">Painel</span>
        </div>
      </nav>

      {/* Top navbar for desktop and mobile */}
      <nav className="navbar navbar-expand-md navbar-light bg-white border-bottom">
        <div className="container-fluid">
          <a className="navbar-brand d-flex align-items-center" href="/painel">Painel</a>
          <div className="ms-auto d-flex align-items-center">
            <div className="me-2 d-none d-md-block small text-muted">{user ? `Olá, ${user.nome || user.username}` : ''}</div>
            <div className="position-relative" ref={userMenuRef}>
              <button className="btn btn-outline-secondary btn-sm d-flex align-items-center" onClick={()=> setUserMenuOpen(s=>!s)}>
                <i className="bi bi-person-circle" style={{fontSize:'1.2rem'}} />
                <i className="bi bi-caret-down-fill ms-2 d-none d-md-inline" />
              </button>
              {userMenuOpen && (
                <div className="dropdown-menu dropdown-menu-end show" style={{position:'absolute', right:0}}>
                  {user ? (
                    <>
                      <div className="dropdown-item-text">{user.nome || user.username}</div>
                      <div className="dropdown-divider" />
                      <button className="dropdown-item" onClick={handleLogout}>Sair</button>
                    </>
                  ) : (
                    <button className="dropdown-item" onClick={()=> navigate('/login')}>Entrar</button>
                  )}
                </div>
              )}
            </div>
          </div>
        </div>
      </nav>

      <div className="container-fluid">
        <div className="row">
          <div className="col-12 col-md-3 col-lg-2 p-2">
            <div className="d-none d-md-block">
              <Menu />
            </div>
            <div className="offcanvas offcanvas-start d-md-none" tabIndex={-1} id="sidebar" aria-labelledby="sidebarLabel">
              <div className="offcanvas-header">
                <h5 className="offcanvas-title" id="sidebarLabel">Menu</h5>
                <button type="button" className="btn-close text-reset" data-bs-dismiss="offcanvas" aria-label="Close"></button>
              </div>
              <div className="offcanvas-body">
                <Menu />
              </div>
            </div>
          </div>
          <div className="col-12 col-md-9 col-lg-10 p-3">
            <div className="page-wrapper">
              <div className="page-card w-100">
                <Outlet />
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  )
}
