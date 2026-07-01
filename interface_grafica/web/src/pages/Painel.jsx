import React, { useEffect, useState } from 'react'
import axios from 'axios'
import { Link, Routes, Route } from 'react-router-dom'
import Organizacao from './painel/Organizacao'

function Menu(){
  const [meta,setMeta] = useState(null)
  useEffect(()=>{ axios.get('/meta/all').then(r=>setMeta(r.data)) },[])
  return (
    <div className="menu">
      <h4>Menu</h4>
      <div><Link to="/painel/organizacoes">Organizações</Link></div>
    </div>
  )
}

export default function Painel(){
  return (
    <div>
      <div className="header">Painel</div>
      <div className="layout">
        <Menu />
        <div className="content">
          <Routes>
            <Route path="/organizacoes" element={<Organizacao/>} />
            <Route path="/" element={<div>Escolha uma opção no menu.</div>} />
          </Routes>
        </div>
      </div>
    </div>
  )
}
