import React, { useState } from 'react'
import axios from 'axios'
import { useNavigate } from 'react-router-dom'
import Navbar from '../components/Navbar'
import Footer from '../components/Footer'

export default function Login(){
  const [user,setUser] = useState('')
  const [pass,setPass] = useState('')
  const nav = useNavigate()

  async function submit(e){
    e.preventDefault()
    try{
      const res = await axios.post('/auth/login',{ username: user, password: pass }, { withCredentials: true })
      nav('/painel')
    }catch(err){
      alert('Erro ao autenticar')
    }
  }

  return (
    <div className="d-flex flex-column min-vh-100">
      <Navbar brand="Retaguarda" />
      <div className="page-wrapper flex-fill d-flex align-items-center justify-content-center">
        <div className="page-card" style={{maxWidth:420}}>
          <h3 className="mb-3">Entrar</h3>
          <form onSubmit={submit}>
            <div className="mb-3">
              <label className="form-label">Usuário</label>
              <input className="form-control" value={user} onChange={e=>setUser(e.target.value)} />
            </div>
            <div className="mb-3">
              <label className="form-label">Senha</label>
              <input className="form-control" type="password" value={pass} onChange={e=>setPass(e.target.value)} />
            </div>
            <div className="d-flex justify-content-end">
              <button className="btn btn-primary" type="submit">Entrar</button>
            </div>
          </form>
        </div>
      </div>
      <Footer />
    </div>
  )
}
