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
import PainelPlanejadorFluxo from './paginas/painel/PainelPlanejadorFluxo'
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
        <Route path='enderecos' element={<TelaPesquisa screenKey={'enderecosPesquisa'} />} />
        <Route path='enderecos/editar/:id' element={<TelaCadastro screenKey={'enderecosCadastro'} />} />
        <Route path='enderecos/bairros' element={<TelaPesquisa screenKey={'bairrosPesquisa'} />} />
        <Route path='enderecos/bairros/editar/:id' element={<TelaCadastro screenKey={'bairrosCadastro'} />} />
        <Route path='enderecos/municipios' element={<TelaPesquisa screenKey={'municipioPesquisa'} />} />
        <Route path='enderecos/municipios/editar/:id' element={<TelaCadastro screenKey={'municipioCadastro'} />} />
        <Route path='enderecos/logradouros' element={<TelaPesquisa screenKey={'logradouroPesquisa'} />} />
        <Route path='enderecos/logradouros/editar/:id' element={<TelaCadastro screenKey={'logradouroCadastro'} />} />
        <Route path='enderecos/ceps' element={<TelaPesquisa screenKey={'cepPesquisa'} />} />
        <Route path='enderecos/ceps/editar/:id' element={<TelaCadastro screenKey={'cepCadastro'} />} />
        <Route path='enderecos/imoveis' element={<TelaPesquisa screenKey={'imovelPesquisa'} />} />
        <Route path='enderecos/imoveis/editar/:id' element={<TelaCadastro screenKey={'imovelCadastro'} />} />
        <Route path='enderecos/paises' element={<TelaPesquisa screenKey={'paisPesquisa'} />} />
        <Route path='enderecos/paises/editar/:id' element={<TelaCadastro screenKey={'paisCadastro'} />} />
        <Route path='enderecos/ufs' element={<TelaPesquisa screenKey={'ufPesquisa'} />} />
        <Route path='enderecos/ufs/editar/:id' element={<TelaCadastro screenKey={'ufCadastro'} />} />
        {/* Catálogos */}
        <Route path='catalogos/nivel-governo' element={<TelaPesquisa screenKey={'nivelGovernoPesquisa'} />} />
        <Route path='catalogos/nivel-governo/editar/:id' element={<TelaCadastro screenKey={'nivelGovernoCadastro'} />} />
        <Route path='catalogos/natureza-juridica' element={<TelaPesquisa screenKey={'naturezaJuridicaPesquisa'} />} />
        <Route path='catalogos/natureza-juridica/editar/:id' element={<TelaCadastro screenKey={'naturezaJuridicaCadastro'} />} />
        <Route path='catalogos/tipo-unidade' element={<TelaPesquisa screenKey={'tipoUnidadePesquisa'} />} />
        <Route path='catalogos/tipo-unidade/editar/:id' element={<TelaCadastro screenKey={'tipoUnidadeCadastro'} />} />
        <Route path='catalogos/tipo-endereco' element={<TelaPesquisa screenKey={'tipoEnderecoPesquisa'} />} />
        <Route path='catalogos/tipo-endereco/editar/:id' element={<TelaCadastro screenKey={'tipoEnderecoCadastro'} />} />
        <Route path='catalogos/tipo-contato' element={<TelaPesquisa screenKey={'tipoContatoPesquisa'} />} />
        <Route path='catalogos/tipo-contato/editar/:id' element={<TelaCadastro screenKey={'tipoContatoCadastro'} />} />
        <Route path='catalogos/documento-tipo' element={<TelaPesquisa screenKey={'documentoTipoPesquisa'} />} />
        <Route path='catalogos/documento-tipo/editar/:id' element={<TelaCadastro screenKey={'documentoTipoCadastro'} />} />
        <Route path='catalogos/tipo-imovel' element={<TelaPesquisa screenKey={'tipoImovelPesquisa'} />} />
        <Route path='catalogos/tipo-imovel/editar/:id' element={<TelaCadastro screenKey={'tipoImovelCadastro'} />} />
        <Route path='catalogos/situacoes' element={<TelaPesquisa screenKey={'situacaoPesquisa'} />} />
        <Route path='catalogos/situacoes/editar/:id' element={<TelaCadastro screenKey={'situacaoCadastro'} />} />
        <Route index element={<InicioPainel/>} />
        <Route path='planejadorFluxo' element={<PainelPlanejadorFluxo/>} />
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
