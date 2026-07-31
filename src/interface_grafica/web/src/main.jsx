import React from 'react'
import { createRoot } from 'react-dom/client'
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom'
import Home from './pages/Home'
import Login from './pages/Login'
import CadastroUsuario from './pages/CadastroUsuario'
import LayoutPrivado from './components/Layout/LayoutPrivado'
import TelaPesquisa from './components/Cadastros/TelaPesquisa'
import TelaCadastro from './components/Cadastros/TelaCadastro'
import InicioPainel from './pages/painel/InicioPainel'
import 'bootstrap/dist/css/bootstrap.min.css'
import './styles.css'
import './services/api'

function App(){
  return (
    <Routes>
      <Route path='/' element={<Home/>} />
      <Route path='/login' element={<Login/>} />
      <Route path='/cadastro' element={<CadastroUsuario/>} />
      <Route path='/painel/*' element={<LayoutPrivado/>}>
        <Route path='organizacoes' element={<TelaPesquisa screenKey={'organizacaoPesquisa'} />} />
        <Route path='organizacoes/editar/:id' element={<TelaCadastro screenKey={'organizacaoCadastro'} />} />
        <Route path='organizacoes/setores' element={<TelaPesquisa screenKey={'setorPesquisa'} />} />
        <Route path='organizacoes/setores/editar/:id' element={<TelaCadastro screenKey={'setorCadastro'} />} />
        <Route index element={<InicioPainel/>} />
      </Route>
      <Route path='*' element={<Navigate to='/' replace />} />
    </Routes>
  )
}

import ModalProvider from './components/InterfaceBasica/ModalProvedor'

createRoot(document.getElementById('root')).render(
  <BrowserRouter>
    <ModalProvider>
      <App />
    </ModalProvider>
  </BrowserRouter>
)
