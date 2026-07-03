import React, { useEffect, useState, useRef } from 'react'
import { useNavigate } from 'react-router-dom'
import axios from 'axios'

export default function Navbar({ brand = 'Painel' }){
  const [user, setUser] = useState(null)
  const [userMenuOpen, setUserMenuOpen] = useState(false)
  const userMenuRef = useRef()
  const navigate = useNavigate()

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
    try{
      await axios.post('/auth/logout');
      setUser(null);
      navigate('/login');
      // reload to ensure cookie is cleared and authenticated UI resets
      window.location.reload();
    }catch{
      navigate('/login')
    }
  }

  return (
    <>
      <nav className="navbar navbar-light bg-light d-flex d-md-none">
        <div className="container-fluid">
          <button className="btn btn-outline-secondary" type="button" data-bs-toggle="offcanvas" data-bs-target="#sidebar" aria-controls="sidebar">
            <i className="bi bi-list" />
          </button>
          <span className="navbar-brand ms-2">{brand}</span>
        </div>
      </nav>

      <nav className="navbar navbar-expand-md navbar-light bg-white border-bottom">
        <div className="container-fluid">
          {/* Home icon placed absolutely to avoid invading sidebar width */}
          <a className="btn btn-outline-secondary btn-sm home-left d-flex align-items-center" href="/" title="Home">
            <i className="bi bi-house" style={{fontSize:'1.1rem'}} />
          </a>
          <a className="navbar-brand d-flex align-items-center" href="/painel">{brand}</a>
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
                      <a className="dropdown-item d-flex align-items-center" href="/painel"><i className="bi bi-speedometer2 me-2" />Painel</a>
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
    </>
  )
}
