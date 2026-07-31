import React from 'react'
import { Link } from 'react-router-dom'
import BarraNavegacao from '../componentes/Layout/BarraNavegacao'
import Rodape from '../componentes/Layout/Rodape'
export default function Inicial(){
  return (
    <div className="d-flex flex-column min-vh-100">
      <BarraNavegacao brand="Painel" />
      <div className="page-wrapper">
        <div className="page-card">
          <h2>Bem-vindo</h2>
          <p><Link to="/autenticacao">Entrar</Link> | <Link to="/cadastro">Criar conta</Link></p>
        </div>
      </div>
      <Rodape />
    </div>
  )
}
