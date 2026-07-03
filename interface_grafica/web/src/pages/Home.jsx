import React from 'react'
import { Link } from 'react-router-dom'
import Navbar from '../components/Navbar'
import Footer from '../components/Footer'
export default function Home(){
  return (
    <div className="d-flex flex-column min-vh-100">
      <Navbar brand="Retaguarda" />
      <div className="page-wrapper flex-fill d-flex align-items-center justify-content-center">
        <div className="page-card">
          <h2>Bem-vindo</h2>
          <p><Link to="/login">Entrar</Link> | <Link to="/cadastro">Criar conta</Link></p>
        </div>
      </div>
      <Footer />
    </div>
  )
}
