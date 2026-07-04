import React from 'react'

export default function BlockingOverlay({ show }){
  if (!show) return null
  return (
    <div style={{ position:'fixed', inset:0, backgroundColor:'rgba(255,255,255,0.7)', zIndex: 9999, display:'flex', alignItems:'center', justifyContent:'center' }}>
      <div style={{ textAlign:'center' }}>
        <div className="spinner-border text-primary" role="status" style={{ width: '3rem', height: '3rem' }}>
          <span className="visually-hidden">Carregando...</span>
        </div>
      </div>
    </div>
  )
}
