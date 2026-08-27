import React, { useEffect, useState, useRef } from 'react'
import { useNavigate } from 'react-router-dom'
import api from '../../servicos/api'

export default function BarraNavegacao({ brand = 'Painel' }){
  const [user, setUser] = useState(null)
  const [userMenuOpen, setUserMenuOpen] = useState(false)
  const [showAtuacao, setShowAtuacao] = useState(false)
  const [contexto, setContexto] = useState(null)
  const userMenuRef = useRef()
  const navigate = useNavigate()

  useEffect(()=>{
    let mounted = true
    api.get('/auth/me').then(r=>{ if (mounted) setUser(r.data) }).catch(()=>{ if (mounted) setUser(null) })
    api.get('/api/usuario/contexto').then(r=>{ if (mounted) setContexto(r.data) }).catch(()=>{ if (mounted) setContexto(null) })
    return ()=> { mounted = false }
  },[])

  const contextoDisplay = (() => {
    if (!contexto) return '';
    const ultimo = contexto.ultimoAcesso || {};
    const parts = [];
    if (ultimo.organizacaoId) {
      const o = (contexto.organizacoes || []).find(x => x.id === ultimo.organizacaoId);
      if (o) parts.push(o.nome);
    }
    if (ultimo.organizacaoUnidadeId) {
      const u = (contexto.unidades || []).find(x => x.id === ultimo.organizacaoUnidadeId);
      if (u) parts.push(u.nome);
    }
    if (ultimo.setorId) {
      const s = (contexto.setores || []).find(x => x.id === ultimo.setorId);
      if (s) parts.push(s.nome);
    }
    return parts.join(' / ');
  })();

  useEffect(()=>{
    function onDoc(e){ if (userMenuRef.current && !userMenuRef.current.contains(e.target)) setUserMenuOpen(false) }
    document.addEventListener('click', onDoc)
    return ()=> document.removeEventListener('click', onDoc)
  },[])

  async function handleLogout(){
    try{
      await api.post('/auth/logout', null, { block: true });
      setUser(null);
      navigate('/autenticacao');
      // reload to ensure cookie is cleared and authenticated UI resets
      window.location.reload();
    }catch{
      navigate('/autenticacao')
    }
  }

  return (
    <>
      <nav className="navbar navbar-expand-md navbar-light bg-white border-bottom bg-admin">
        <div className="container-fluid">
          {/* Home icon placed absolutely to avoid invading sidebar width */}
          <a className="navbar-brand d-flex align-items-center" href="/painel">{brand}</a>
          <a className="btn btn-outline-light btn-sm home-left d-flex align-items-center" href="/" title="Home">
            <i className="bi bi-house" style={{fontSize:'1.1rem'}} />
          </a>
          <div className="ms-auto d-flex align-items-center">
            <div className="me-3 d-none d-md-block text-end">
              <div className="small text-light">{user ? `Olá, ${user.nome || user.username}` : ''}</div>
              <div className="small text-light">{contextoDisplay}</div>
            </div>
            <div className="position-relative" ref={userMenuRef}>
              <button className="btn btn-outline-light btn-sm d-flex align-items-center" onClick={()=> setUserMenuOpen(s=>!s)}>
                <i className="bi bi-person-circle" style={{fontSize:'1.2rem'}} />
                <i className="bi bi-caret-down-fill ms-2 d-none d-md-inline" />
              </button>
              {userMenuOpen && (
                <div className="dropdown-menu dropdown-menu-end show" style={{position:'absolute', right:0}}>
                  {user ? (
                    <>
                      <div className="dropdown-item-text">{user.nome || user.username}</div>
                      <button className="dropdown-item d-flex align-items-center" onClick={()=> setShowAtuacao(true)}><i className="bi bi-people me-2"/>Trocar setor</button>
                      <a className="dropdown-item d-flex align-items-center" href="/painel"><i className="bi bi-speedometer2 me-2" />Painel</a>
                      <div className="dropdown-divider" />
                      <button className="dropdown-item" onClick={handleLogout}>Sair</button>
                    </>
                  ) : (
                    <button className="dropdown-item" onClick={()=> navigate('/autenticacao')}>Entrar</button>
                  )}
                </div>
              )}
            </div>
          </div>
        </div>
      </nav>
      {showAtuacao && <React.Suspense fallback={null}><TrocarAtuacao onClose={()=> setShowAtuacao(false)} /></React.Suspense>}
    </>
  )
}

const TrocarAtuacao = React.lazy(() => import('../Atuacao/TrocarAtuacao'))
