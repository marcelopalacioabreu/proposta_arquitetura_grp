import React from 'react'
import Navbar from '../components/Navbar'

export default function CadastroUsuario(){
  return (
    <div>
      <Navbar brand="Retaguarda" />
      <div className="page-wrapper vh-100">
        <div className="page-card" style={{maxWidth:720}}>
          <h3 className="mb-3">Criar Conta</h3>
          <form>
            <div className="row g-2">
              <div className="col-md-6">
                <label className="form-label">Nome</label>
                <input className="form-control" />
              </div>
              <div className="col-md-6">
                <label className="form-label">Usuário</label>
                <input className="form-control" />
              </div>
              <div className="col-md-6">
                <label className="form-label">Email</label>
                <input className="form-control" />
              </div>
              <div className="col-md-6">
                <label className="form-label">Senha</label>
                <input className="form-control" type="password" />
              </div>
            </div>
            <div className="mt-3 d-flex justify-content-end">
              <button className="btn btn-primary">Criar</button>
            </div>
          </form>
        </div>
      </div>
    </div>
  )
}
