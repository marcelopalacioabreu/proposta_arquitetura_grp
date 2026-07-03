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
  const [formState, setFormState] = useState({ campo: null, operador: null, valor: '', valor_de: '', valor_ate: '' })
  const [operadores, setOperadores] = useState([])
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
    const initialCampo = query.get('campo') || meta.filtro?.[0]?.campo
    const tipo = meta.filtro?.find(f=> f.campo === initialCampo)?.tipo || 'string'
    const operadoresPorTipo = {
      string: ['iniciando_com','contendo','terminando_com','igual'],
      number: ['igual','maior_que','menor_que','entre'],
      date: ['igual','antes','depois','entre']
    }
    const ops = operadoresPorTipo[tipo] || operadoresPorTipo.string
    setOperadores(ops)
    setFormState(s => ({ ...s, campo: initialCampo, operador: query.get('operador') || ops[0], valor: query.get('valor') || '', valor_de: query.get('valor_de') || '', valor_ate: query.get('valor_ate') || '' }))
  },[meta])

  useEffect(()=>{
    if (!meta) return
    // determine API endpoint from metadata
    let endpoint = null
    if (meta.endpoint) endpoint = meta.endpoint
    else if (meta.tabela && meta.tabela.endpoint) endpoint = meta.tabela.endpoint
    // try to derive from delete/navigation actions if not provided
    if (!endpoint && meta.tabela && Array.isArray(meta.tabela.acoes)){
      for (const a of meta.tabela.acoes){
        if (a.destino && a.destino.startsWith('/api/')){
          // strip trailing /{id} if present
          endpoint = a.destino.replace(/\/{?\{id\}}?$/, '')
          // also remove any placeholder like /{id}
          endpoint = endpoint.replace(/\/\{id\}$/, '')
          break
        }
      }
    }
    if (!endpoint) endpoint = '/api/organizacoes'
    // build query from filters (single dynamic filter)
    const params = {}
    const campo = query.get('campo')
    const operador = query.get('operador')
    const valor = query.get('valor')
    const valorDe = query.get('valor_de')
    const valorAte = query.get('valor_ate')
    const inativo = query.get('inativo')
    if (campo) params['campo'] = campo
    if (operador) params['operador'] = operador
    if (valor) params['valor'] = valor
    if (valorDe) params['valor_de'] = valorDe
    if (valorAte) params['valor_ate'] = valorAte
    if (inativo) params['inativo'] = inativo
    // pagination and sorting from querystring
    const page = query.get('page') || 1
    const pageSize = query.get('pageSize') || (meta.pagination?.pageSize || 10)
    const sortField = query.get('sortField') || null
    const sortDir = query.get('sortDir') || null

    axios.get(endpoint, { params: { ...params, page, pageSize, sortField, sortDir } }).then(r=>{
      // API returns { items, total, page, pageSize }
      if (r.data.items) { setItems(r.data.items); setTotal(r.data.total || r.data.items.length) }
      else setItems(r.data)
    }).catch(()=>setItems([]))
  },[meta, useLocation().search])

  if (!meta) return <div>Carregando...</div>

  return (
    <div className="page-wrapper">
      <div className="page-card w-100">
      <div className="d-flex justify-content-between align-items-center mb-2">
        <h3>{meta.titulo || 'Pesquisa'}</h3>
        <div>
          <button
            className="btn btn-primary"
            onClick={()=> navigate((meta.tabela?.acoes?.find(a=>a.tipo==='navegacao')?.destino || '/painel/organizacoes/editar/new').replace('{id}','new'))}
            title="Novo"
            aria-label="Novo"
          >
            <i className="bi bi-plus" />
          </button>
        </div>
      </div>

      {/* Tabs for Ativos / Inativos */}
      <ul className="nav nav-tabs mb-3">
        <li className="nav-item">
          <button
            className={`nav-link ${!query.get('inativo') || query.get('inativo') === '0' ? 'active' : ''}`}
            onClick={()=>{ const cur = new URLSearchParams(location.search); cur.delete('inativo'); navigate({ search: cur.toString() })}}
            title="Ativos"
            aria-label="Ativos"
          >
            <i className="bi bi-check-circle" />
          </button>
        </li>
        <li className="nav-item">
          <button
            className={`nav-link ${query.get('inativo') === '1' ? 'active' : ''}`}
            onClick={()=>{ const cur = new URLSearchParams(location.search); cur.set('inativo','1'); navigate({ search: cur.toString() })}}
            title="Inativos"
            aria-label="Inativos"
          >
            <i className="bi bi-x-circle" />
          </button>
        </li>
      </ul>

      <div className="mb-3">
        <form className="row g-2" onSubmit={e=>{ e.preventDefault(); const qp = []; const s = formState; if (s.campo) qp.push(`campo=${encodeURIComponent(s.campo)}`); if (s.operador) qp.push(`operador=${encodeURIComponent(s.operador)}`); if (s.valor) qp.push(`valor=${encodeURIComponent(s.valor)}`); if (s.valor_de) qp.push(`valor_de=${encodeURIComponent(s.valor_de)}`); if (s.valor_ate) qp.push(`valor_ate=${encodeURIComponent(s.valor_ate)}`); const cur = new URLSearchParams(location.search); if (cur.get('inativo')) qp.push(`inativo=${encodeURIComponent(cur.get('inativo'))}`); navigate({ search: qp.join('&') }) }}>
          {/* Single dynamic filter like filtro-dinamico */}
          <div className="col-md-3">
            <label className="form-label">Campo</label>
            <select id="campo" name="campo" className="form-select" value={formState.campo || ''} onChange={e=>{ const newCampo = e.target.value; const tipo = meta.filtro?.find(f=> f.campo === newCampo)?.tipo || 'string'; const operadoresPorTipo = { string: ['iniciando_com','contendo','terminando_com','igual'], number: ['igual','maior_que','menor_que','entre'], date: ['igual','antes','depois','entre'] }; const ops = operadoresPorTipo[tipo] || operadoresPorTipo.string; setOperadores(ops); setFormState(s=>({ ...s, campo: newCampo, operador: ops[0], valor: '', valor_de: '', valor_ate: '' })) }}>
              {meta.filtro.map((f, idx) => (<option key={idx} value={f.campo} data-tipo={f.tipo}>{f.descricao}</option>))}
            </select>
          </div>
          <div className="col-md-3">
            <label className="form-label">Operador</label>
            <select id="operador" name="operador" className="form-select" value={formState.operador || ''} onChange={e=> setFormState(s=> ({ ...s, operador: e.target.value }))}>
              {operadores.map((op, idx) => (<option key={idx} value={op}>{op.replace(/_/g,' ').replace(/\b\w/g,c=>c.toUpperCase())}</option>))}
            </select>
          </div>
          {formState.operador === 'entre' ? (
            <div className="col-md-4">
              <label className="form-label">Entre</label>
              <input type="text" name="valor_de" value={formState.valor_de} onChange={e=> setFormState(s=> ({ ...s, valor_de: e.target.value }))} className="form-control" placeholder="De" style={{width:'45%', display:'inline-block', marginRight:'4%'}} />
              <input type="text" name="valor_ate" value={formState.valor_ate} onChange={e=> setFormState(s=> ({ ...s, valor_ate: e.target.value }))} className="form-control" placeholder="Até" style={{width:'45%', display:'inline-block'}} />
            </div>
          ) : (
            <div className="col-md-4">
              <label className="form-label d-block">Valor</label>
              <input name="valor" value={formState.valor} onChange={e=> setFormState(s=> ({ ...s, valor: e.target.value }))} placeholder="Valor" className="form-control" />
            </div>
          )}
          <div className="col-md-2 d-none" id="campo-hidden-inativo">
            {/* reserved for hidden inativo input if needed */}
          </div>
          <div className="col-12">
            <button className="btn btn-secondary" title="Filtrar" aria-label="Filtrar"><i className="bi bi-funnel" /></button>
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
                  {title}{' '}
                  <button className="btn btn-link p-0" title={`Ordenar por ${title}`} aria-label={`Ordenar por ${title}`} onClick={()=>{
                    const cur = new URLSearchParams(location.search)
                    if (cur.get('sortField') === field){
                      cur.set('sortDir', cur.get('sortDir') === 'asc' ? 'desc' : 'asc')
                    } else {
                      cur.set('sortField', field)
                      cur.set('sortDir', 'asc')
                    }
                    cur.set('page', '1')
                    navigate({ search: cur.toString() })
                  }}><i className={`bi bi-${icon}`}></i></button>
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
            <button className="btn btn-sm btn-secondary me-2" title="Anterior" aria-label="Anterior" onClick={()=>{ const cur = new URLSearchParams(useLocation().search); const p = Math.max(1, parseInt(cur.get('page')||1)-1); cur.set('page', p); navigate({ search: cur.toString() }) }}><i className="bi bi-chevron-left" /></button>
            <button className="btn btn-sm btn-secondary" title="Próxima" aria-label="Próxima" onClick={()=>{ const cur = new URLSearchParams(useLocation().search); const p = Math.max(1, parseInt(cur.get('page')||1)+1); cur.set('page', p); navigate({ search: cur.toString() }) }}><i className="bi bi-chevron-right" /></button>
          </div>
        </div>
      )}

      <ConfirmModal show={confirmState.show} title={confirmState.title} message={confirmState.message} onConfirm={confirmState.onConfirm} onCancel={()=> setConfirmState(s=> ({...s, show:false}))} />
      </div>
    </div>
  )
}
