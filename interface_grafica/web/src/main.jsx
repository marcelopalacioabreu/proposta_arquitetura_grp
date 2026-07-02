import React from 'react'
import { createRoot } from 'react-dom/client'
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom'
import Home from './pages/Home'
import Login from './pages/Login'
import CadastroUsuario from './pages/CadastroUsuario'
import PrivateLayout from './components/PrivateLayout'
import TelaPesquisa from './components/TelaPesquisa'
import TelaCadastro from './components/TelaCadastro'
import './styles.css'
import 'bootstrap/dist/css/bootstrap.min.css'
import axios from 'axios'

axios.defaults.withCredentials = true

function App(){
  return (
    <BrowserRouter>
      <Routes>
        <Route path='/' element={<Home/>} />
        <Route path='/login' element={<Login/>} />
        <Route path='/cadastro' element={<CadastroUsuario/>} />
        <Route path='/painel/*' element={<PrivateLayout/>}>
          <Route path='organizacoes' element={<TelaPesquisa screenKey={'organizacaoPesquisa'} />} />
          <Route path='organizacoes/editar/:id' element={<TelaCadastro screenKey={'organizacaoCadastro'} />} />
          <Route path='organizacoes/setores' element={<TelaPesquisa screenKey={'setorPesquisa'} />} />
          <Route path='organizacoes/setores/editar/:id' element={<TelaCadastro screenKey={'setorCadastro'} />} />
          <Route index element={<div className="container mt-3">Escolha uma opção no menu.</div>} />
        </Route>
        <Route path='*' element={<Navigate to='/' replace />} />
      </Routes>
    </BrowserRouter>
  )
}

createRoot(document.getElementById('root')).render(<App />)
