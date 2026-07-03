import React from 'react'
import { Link } from 'react-router-dom'

export default function InicioPainel(){
  return (
    <div className="page-wrapper">
      <div className="page-card w-100">
        <div className="d-flex justify-content-between align-items-center mb-3">
          <div>
            <h2>Painel</h2>
            <div className="small text-muted">Visão geral rápida</div>
          </div>
          <div>
            <button className="btn btn-outline-secondary me-2">Atualizar</button>
            <Link to="/painel/organizacoes" className="btn btn-primary">Ir para Organizações</Link>
          </div>
        </div>

        <div className="row g-3 mb-3">
          <div className="col-md-4">
            <div className="p-3 bg-white border rounded h-100">
              <h5 className="mb-1">Organizações</h5>
              <div className="display-6">128</div>
              <div className="small text-muted">Total de organizações cadastradas</div>
            </div>
          </div>
          <div className="col-md-4">
            <div className="p-3 bg-white border rounded h-100">
              <h5 className="mb-1">Usuários</h5>
              <div className="display-6">452</div>
              <div className="small text-muted">Usuários ativos no sistema</div>
            </div>
          </div>
          <div className="col-md-4">
            <div className="p-3 bg-white border rounded h-100">
              <h5 className="mb-1">Perfis</h5>
              <div className="display-6">12</div>
              <div className="small text-muted">Perfis de acesso</div>
            </div>
          </div>
        </div>

        <div className="row">
          <div className="col-md-8">
            <div className="p-3 bg-white border rounded mb-3">
              <h6>Atividades recentes</h6>
              <ul className="list-unstyled mb-0">
                <li>Usuário João criou a organização Acme Ltda.</li>
                <li>Perfil Administrador atualizado.</li>
                <li>Permissão de acesso alterada para setor Financeiro.</li>
              </ul>
            </div>
          </div>
          <div className="col-md-4">
            <div className="p-3 bg-white border rounded mb-3">
              <h6>Atalhos</h6>
              <div className="d-grid gap-2">
                <Link to="/painel/organizacoes" className="btn btn-sm btn-outline-primary">Organizações</Link>
                <Link to="/painel/usuarios" className="btn btn-sm btn-outline-secondary">Usuários</Link>
                <Link to="/painel/perfis" className="btn btn-sm btn-outline-secondary">Perfis</Link>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  )
}
