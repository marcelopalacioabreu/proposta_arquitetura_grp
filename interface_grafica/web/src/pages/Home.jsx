import React from 'react'
import { Link } from 'react-router-dom'
import Navbar from '../components/Navbar'
export default function Home(){
  return (
    <div>
      <Navbar brand="Retaguarda" />
      <div className="page-wrapper">
        <div className="page-card">
          <h2>Bem-vindo</h2>
          <p><Link to="/login">Entrar</Link> | <Link to="/cadastro">Criar conta</Link></p>
        </div>
      </div>
    </div>
  )
}
