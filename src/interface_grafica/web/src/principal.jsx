import React from 'react'
import { createRoot } from 'react-dom/client'
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom'
import Inicial from './paginas/Inicial'
import Autenticacao from './paginas/Autenticacao'
import CadastroUsuario from './paginas/CadastroUsuario'
import LayoutPrivado from './componentes/Layout/LayoutPrivado'
import TelaPesquisa from './componentes/Cadastros/TelaPesquisa'
import TelaCadastro from './componentes/Cadastros/TelaCadastro'
import InicioPainel from './paginas/painel/InicioPainel'
import 'bootstrap/dist/css/bootstrap.min.css'
import './styles.css'
import './servicos/api'

function App(){
  return (
    <Routes>
      <Route path='/' element={<Inicial/>} />
      <Route path='/autenticacao' element={<Autenticacao/>} />
      <Route path='/cadastro' element={<CadastroUsuario/>} />
      <Route path='/painel/*' element={<LayoutPrivado/>}>
        <Route path='organizacoes' element={<TelaPesquisa screenKey={'organizacaoPesquisa'} />} />
        <Route path='organizacoes/:organizacaoId' element={<TelaPesquisa screenKey={'organizacaoPesquisa'} />} />
        <Route path='organizacoes/:organizacaoId/unidades' element={<TelaPesquisa screenKey={'organizacaoUnidadePesquisa'} />} />
        <Route path='organizacoes/:organizacaoId/unidades/editar/:id' element={<TelaCadastro screenKey={'organizacaoUnidadeCadastro'} />} />
        <Route path='organizacoes/editar/:id' element={<TelaCadastro screenKey={'organizacaoCadastro'} />} />
        <Route path='organizacoes/unidades' element={<TelaPesquisa screenKey={'organizacaoUnidadePesquisa'} />} />
        <Route path='organizacoes/unidades/:organizacaoId' element={<TelaPesquisa screenKey={'organizacaoUnidadePesquisa'} />} />
        <Route path='organizacoes/unidades/editar/:id' element={<TelaCadastro screenKey={'organizacaoUnidadeCadastro'} />} />
        <Route path='organizacoes/unidades/setores' element={<TelaPesquisa screenKey={'organizacaoUnidadeSetorPesquisa'} />} />
        <Route path='organizacoes/unidades/setores/:organizacaoUnidadeId' element={<TelaPesquisa screenKey={'organizacaoUnidadeSetorPesquisa'} />} />
        <Route path='organizacoes/unidades/setores/editar/:id' element={<TelaCadastro screenKey={'organizacaoUnidadeSetorCadastro'} />} />
        <Route path='organizacoes/:organizacaoId/unidades/:organizacaoUnidadeId/setores' element={<TelaPesquisa screenKey={'organizacaoUnidadeSetorPesquisa'} />} />
        <Route path='organizacoes/:organizacaoId/unidades/:organizacaoUnidadeId/setores/editar/:id' element={<TelaCadastro screenKey={'organizacaoUnidadeSetorCadastro'} />} />
        <Route path='pessoas' element={<TelaPesquisa screenKey={'pessoaPesquisa'} />} />
        <Route path='pessoas/editar/:id' element={<TelaCadastro screenKey={'pessoaCadastro'} />} />
        <Route path='usuarios' element={<TelaPesquisa screenKey={'usuarioPesquisa'} />} />
        <Route path='usuarios/editar/:id' element={<TelaCadastro screenKey={'usuarioCadastro'} />} />
        <Route path='perfis' element={<TelaPesquisa screenKey={'perfilPesquisa'} />} />
        <Route path='perfis/editar/:id' element={<TelaCadastro screenKey={'perfilCadastro'} />} />
        <Route index element={<InicioPainel/>} />
      </Route>
      <Route path='*' element={<Navigate to='/' replace />} />
    </Routes>
  )
}

import ModalProvedor from './componentes/InterfaceBasica/ModalProvedor'

createRoot(document.getElementById('root')).render(
  <BrowserRouter>
    <ModalProvedor>
      <App />
    </ModalProvedor>
  </BrowserRouter>
)
