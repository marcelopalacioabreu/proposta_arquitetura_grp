import React, { useEffect, useState } from 'react'
import axios from 'axios'
import { useLocation, useNavigate } from 'react-router-dom'
import ConfirmModal from './ConfirmModal'

function useQuery(){
  return new URLSearchParams(useLocation().search)
}

export default function TelaPesquisa({ screenKey }){
  const [meta, setMeta] = useState(null)
  const [items, setItems] = useState([])
  const [total, setTotal] = useState(0)
  const query = useQuery()
  const navigate = useNavigate()
  const location = useLocation()
  const [confirmState, setConfirmState] = useState({ show: false, title: null, message: null, onConfirm: null })

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
    // pagination and sorting from querystring
    const page = query.get('page') || 1
    const pageSize = query.get('pageSize') || (meta.pagination?.pageSize || 10)
    const sortField = query.get('sortField') || null
    const sortDir = query.get('sortDir') || null

    axios.get('/api/organizacoes', { params: { ...params, page, pageSize, sortField, sortDir } }).then(r=>{
      // API returns { items, total, page, pageSize }
      if (r.data.items) { setItems(r.data.items); setTotal(r.data.total || r.data.items.length) }
      else setItems(r.data)
    }).catch(()=>setItems([]))
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
            {((meta.tabela.colunas && meta.tabela.colunas.length) ? meta.tabela.colunas : []).map((c, idx) => {
              const field = typeof c === 'string' ? c : c.campo
              const title = typeof c === 'string' ? c : c.titulo
              const curField = query.get('sortField')
              const curDir = query.get('sortDir') || 'asc'
              const isActive = curField === field
              const icon = isActive ? (curDir === 'asc' ? 'arrow-up' : 'arrow-down') : 'dash'
              return (
                <th key={idx}>
                  <button className="btn btn-link p-0" onClick={()=>{
                    const cur = new URLSearchParams(location.search)
                    if (cur.get('sortField') === field){
                      cur.set('sortDir', cur.get('sortDir') === 'asc' ? 'desc' : 'asc')
                    } else {
                      cur.set('sortField', field)
                      cur.set('sortDir', 'asc')
                    }
                    cur.set('page', '1')
                    navigate({ search: cur.toString() })
                  }}>{title} <i className={`bi bi-${icon}`}></i></button>
                </th>
              )
            })}
            <th>Ações</th>
          </tr>
        </thead>
        <tbody>
          {items.map(it => (
            <tr key={it.id}>
              {((meta.tabela.campos && meta.tabela.campos.length) ? meta.tabela.campos : meta.tabela.colunas).map((c, idx) => {
                    const field = typeof c === 'string' ? c : c.campo
                    return (<td key={idx}>{it[field]}</td>)
                  })}
              <td>
                    {meta.tabela.acoes.map((a, ai) => (
                      <React.Fragment key={ai}>
                        {a.tipo === 'navegacao' && <button className="btn btn-sm btn-link" onClick={()=> navigate(a.destino.replace('{id}', it[a.campo_id]))}><i className={`bi bi-${a.icone}`}></i></button>}
                        {a.tipo === 'confirmacao_delete_ajax' && <button className="btn btn-sm btn-link text-danger" onClick={()=> setConfirmState({ show: true, title: 'Excluir', message: 'Confirma exclusão?', onConfirm: async ()=>{ await axios.delete(a.destino.replace('{id}', it[a.campo_id])); setItems(items.filter(x=> x.id !== it.id)); setConfirmState(s=> ({...s, show: false})) } }) }><i className={`bi bi-${a.icone}`}></i></button>}
                      </React.Fragment>
                    ))}
              </td>
            </tr>
          ))}
        </tbody>
      </table>
      {/* Pagination controls */}
      {meta.pagination && (
        <div className="d-flex justify-content-between align-items-center">
          <div>Mostrando {items.length} de {total}</div>
          <div>
            <button className="btn btn-sm btn-secondary me-2" onClick={()=>{ const cur = new URLSearchParams(useLocation().search); const p = Math.max(1, parseInt(cur.get('page')||1)-1); cur.set('page', p); navigate({ search: cur.toString() }) }}>Anterior</button>
            <button className="btn btn-sm btn-secondary" onClick={()=>{ const cur = new URLSearchParams(useLocation().search); const p = Math.max(1, parseInt(cur.get('page')||1)+1); cur.set('page', p); navigate({ search: cur.toString() }) }}>Próxima</button>
          </div>
        </div>
      )}

      <ConfirmModal show={confirmState.show} title={confirmState.title} message={confirmState.message} onConfirm={confirmState.onConfirm} onCancel={()=> setConfirmState(s=> ({...s, show:false}))} />
    </div>
  )
}
