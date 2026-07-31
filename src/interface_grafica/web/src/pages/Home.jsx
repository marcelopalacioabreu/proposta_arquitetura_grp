import React from 'react'
import { Link } from 'react-router-dom'
import BarraNavegacao from '../components/Layout/BarraNavegacao'
import Rodape from '../components/Layout/Rodape'
export default function Home(){
  return (
    <div className="d-flex flex-column min-vh-100">
      <BarraNavegacao brand="Retaguarda" />
      <div className="page-wrapper">
        <div className="page-card">
          <h2>Bem-vindo</h2>
          <p><Link to="/login">Entrar</Link> | <Link to="/cadastro">Criar conta</Link></p>
        </div>
      </div>
      <Rodape />
    </div>
  )
}
