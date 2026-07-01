import React from 'react'
import { Link } from 'react-router-dom'
export default function Home(){
  return (
    <div>
      <div className="header">Retaguarda - Home</div>
      <div className="container">
        <h2>Bem-vindo</h2>
        <p><Link to="/login">Entrar</Link> | <Link to="/cadastro">Criar conta</Link></p>
      </div>
    </div>
  )
}
