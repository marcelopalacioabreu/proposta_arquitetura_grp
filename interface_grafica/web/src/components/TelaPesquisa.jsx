import React, { useEffect, useState } from 'react'
import axios from 'axios'
import { useLocation, useNavigate } from 'react-router-dom'

function useQuery(){
  return new URLSearchParams(useLocation().search)
}

export default function TelaPesquisa({ screenKey }){
  const [meta, setMeta] = useState(null)
  const [items, setItems] = useState([])
  const query = useQuery()
  const navigate = useNavigate()

  useEffect(()=>{
    axios.get('/meta/screens').then(r=>{
      const s = r.data[screenKey]
      setMeta(s)
    })
  },[screenKey])

  useEffect(()=>{
    if (!meta) return
    // build query from filters
    const params = {}
    if (meta.filtro){
      meta.filtro.forEach(f => {
        const v = query.get(f.campo)
        if (v) params[f.campo] = v
      })
    }
    axios.get('/api/organizacoes', { params }).then(r=> setItems(r.data)).catch(()=>setItems([]))
  },[meta, useLocation().search])

  if (!meta) return <div>Carregando...</div>

  return (
    <div>
      <div className="d-flex justify-content-between align-items-center mb-2">
        <h3>Pesquisa</h3>
        <div>
          <button className="btn btn-primary" onClick={()=> navigate('/painel/organizacoes/editar/new')}>Novo</button>
        </div>
      </div>

      <div className="mb-3">
        <form className="row g-2" onSubmit={e=>{ e.preventDefault(); const form = new FormData(e.target); const qp = []; for (const [k,v] of form.entries()) if (v) qp.push(`${encodeURIComponent(k)}=${encodeURIComponent(v)}`); navigate({ search: qp.join('&') }) }}>
          {meta.filtro.map((f, idx) => (
            <div key={idx} className={`col-12 col-md-${f.col || 4}`}>
              <label className="form-label">{f.descricao}</label>
              <input name={f.campo} defaultValue={new URLSearchParams(useLocation().search).get(f.campo) || ''} className="form-control" />
            </div>
          ))}
          <div className="col-12">
            <button className="btn btn-secondary">Filtrar</button>
          </div>
        </form>
      </div>

      <table className="table table-striped">
        <thead>
          <tr>
            {meta.tabela.colunas.map((c, idx) => (<th key={idx}>{c.titulo}</th>))}
            <th>Ações</th>
          </tr>
        </thead>
        <tbody>
          {items.map(it => (
            <tr key={it.id}>
              {meta.tabela.colunas.map((c, idx) => (<td key={idx}>{it[c.campo]}</td>))}
              <td>
                {meta.tabela.acoes.map((a, ai) => (
                  <React.Fragment key={ai}>
                    {a.tipo === 'navegacao' && <button className="btn btn-sm btn-link" onClick={()=> navigate(a.destino.replace('{id}', it[a.campo_id]))}>{a.icone}</button>}
                    {a.tipo === 'confirmacao_delete_ajax' && <button className="btn btn-sm btn-link text-danger" onClick={async ()=>{ if (!confirm('Confirma excluir?')) return; await axios.delete(a.destino.replace('{id}', it[a.campo_id])); setItems(items.filter(x=> x.id !== it.id)) }}>{a.icone}</button>}
                  </React.Fragment>
                ))}
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  )
}
