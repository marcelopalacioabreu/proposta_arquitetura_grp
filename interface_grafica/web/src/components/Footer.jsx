import React from 'react'

export default function Footer(){
  return (
    <footer className="app-footer bg-light py-3 mt-auto" style={{borderTop:'1px solid #e9ecef'}}>
      <div className="container d-flex justify-content-between small text-muted">
        <div>© {new Date().getFullYear()} Minha Empresa</div>
        <div>Versão 1.0</div>
      </div>
    </footer>
  )
}
