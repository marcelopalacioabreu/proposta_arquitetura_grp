import React, { useState } from 'react'
import axios from 'axios'
import { useNavigate } from 'react-router-dom'

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
    <div>
      <div className="header">Login</div>
      <div className="container">
        <form onSubmit={submit}>
          <div><label>Usuário</label><br/><input value={user} onChange={e=>setUser(e.target.value)} /></div>
          <div><label>Senha</label><br/><input type="password" value={pass} onChange={e=>setPass(e.target.value)} /></div>
          <div><button type="submit">Entrar</button></div>
        </form>
      </div>
    </div>
  )
}
