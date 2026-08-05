import React, { useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import api from '../../servicos/api'

export default function InicioPainel(){
  const [dashboard, setDashboard] = useState(null)
  const [loading, setLoading] = useState(false)

  async function fetchDashboard(){
    try{
      setLoading(true)
      const r = await api.get('/api/dashboard', { block: false })
      setDashboard(r.data)
    }catch(e){
      console.error('Erro ao carregar dashboard', e)
    }finally{ setLoading(false) }
  }

  useEffect(()=>{ fetchDashboard() }, [])

  const cont = dashboard?.contadores || { organizacoes: 0, usuarios: 0, perfis: 0 }
  const atividades = dashboard?.atividades || []
  const atalhos = dashboard?.atalhos || [ { label:'Organizações', path:'/painel/organizacoes', variant:'primary' }, { label:'Usuários', path:'/painel/usuarios', variant:'secondary' }, { label:'Perfis', path:'/painel/perfis', variant:'secondary' } ]

  return (
    <div className="page-wrapper">
      <div className="page-card w-100">
        <div className="d-flex justify-content-between align-items-center mb-3">
          <div>
            <h2>Painel</h2>
            <div className="small text-muted">Visão geral rápida</div>
          </div>
          <div>
            <button onClick={fetchDashboard} className="btn btn-outline-secondary btn-icon me-2" title="Atualizar" aria-label="Atualizar">
              {loading ? <span className="spinner-border spinner-border-sm" aria-hidden="true"></span> : <i className="bi bi-arrow-clockwise" aria-hidden="true"></i>}
            </button>
          </div>
        </div>

        <div className="row g-3 mb-3">
          <div className="col-md-4">
            <div className="p-3 bg-white border rounded h-100">
              <h5 className="mb-1">Organizações</h5>
              <div className="display-6">{cont.organizacoes}</div>
              <div className="small text-muted">Total de organizações cadastradas</div>
            </div>
          </div>
          <div className="col-md-4">
            <div className="p-3 bg-white border rounded h-100">
              <h5 className="mb-1">Usuários</h5>
              <div className="display-6">{cont.usuarios}</div>
              <div className="small text-muted">Usuários ativos no sistema</div>
            </div>
          </div>
          <div className="col-md-4">
            <div className="p-3 bg-white border rounded h-100">
              <h5 className="mb-1">Perfis</h5>
              <div className="display-6">{cont.perfis}</div>
              <div className="small text-muted">Perfis de acesso</div>
            </div>
          </div>
        </div>

        <div className="row">
          <div className="col-md-8">
            <div className="p-3 bg-white border rounded mb-3">
              <h6>Atividades recentes</h6>
              <ul className="list-unstyled mb-0">
                {atividades.length === 0 && <li className="text-muted">Nenhuma atividade recente.</li>}
                {atividades.map((a, idx) => (
                  <li key={idx}><strong>{a.tipo}:</strong> {a.texto} <span className="text-muted">({new Date(a.data).toLocaleString()})</span></li>
                ))}
              </ul>
            </div>
          </div>
          <div className="col-md-4">
            <div className="p-3 bg-white border rounded mb-3">
              <h6>Atalhos</h6>
              <div className="d-grid gap-2">
                {atalhos.map((at, i) => (
                  <Link key={i} to={at.path} className={`btn btn-sm btn-${at.variant === 'primary' ? 'primary' : 'outline-secondary'}`}>{at.label}</Link>
                ))}
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  )
}
