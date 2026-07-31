import React, { useEffect, useState } from 'react'
import api from '../../servicos/api'
import { useLocation, useNavigate, useParams } from 'react-router-dom'
import ConfirmModal from '../InterfaceBasica/ModalConfirmacao'
import modalService from '../../utils/modalServico'

/*
  TelaPesquisa - utilitários e responsabilidades isoladas

  Objetivos da refatoração:
  - Extrair lógica de construção de endpoint e parâmetros para funções nomeadas em português
  - Isolar renderização de colunas/ações para reduzir duplicação
  - Documentar com exemplos de uso de URL e metadados

  Exemplo de URL e metadado esperado:
  URL: /painel/organizacoes/unidades/1?campo=nome
  Metadado (tela):
  {
    "organizacaoUnidadePesquisa": {
      "tipo": "TELA_PESQUISA",
      "titulo": "Unidades",
      "endpoint": "/api/organizacao_unidades",
      "pagination": { "pageSize": 10 }
    }
  }

  A função `obterEndpoint` usa estritamente `meta.endpoint` ou `meta.tabela.endpoint`.
  A função `construirParametros` combina querystring e path params (useParams).
*/

// Retorna a extremidade (endpoint) definida nos metadados da tela (estrito - sem fallbacks)
// Prioriza a chave em português `extremidade` e mantém compatibilidade com `endpoint`.
function obterEndpoint(meta){
  if (!meta) return null
  if (meta.extremidade) return meta.extremidade
  if (meta.endpoint) return meta.endpoint
  if (meta.tabela && (meta.tabela.extremidade || meta.tabela.endpoint)) return meta.tabela.extremidade || meta.tabela.endpoint
  return null
}

// Constroi um objeto de query params a partir da URL (querystring) e dos path params (useParams())
// Mantemos apenas chaves que o metadado espera — por enquanto pass-through genérico.
// Construir parâmetros combinando querystring, path params (useParams) e camposChaveUrl definidos nos metadados.
// Se `meta.camposChaveUrl` existir, extraí valores numéricos da URL e mapeia na ordem para as chaves.
function construirParametros(queryObj, pathParams, pathname, meta){
  const p = {}
  // copiar todos os pares da querystring
  for (const [k,v] of queryObj.entries()) if (v !== null && v !== undefined && v !== '') p[k] = v
  // mapear path params para query quando não existirem
  try{ Object.keys(pathParams || {}).forEach(k=> { if (!p[k] && pathParams[k]) p[k] = pathParams[k] }) }catch(e){}

  // Suporte camposChaveUrl: extrair segmentos numéricos da pathname e mapear para as chaves fornecidas
  try{
    const chaves = meta?.camposChaveUrl
    if (Array.isArray(chaves) && chaves.length){
      const segments = (pathname || '').split('/').map(s=> s.trim()).filter(Boolean)
      // extrair apenas segmentos que são números
      const nums = segments.map(s=> (s.match(/^\d+$/) ? s : null)).filter(Boolean)
      for (let i=0;i<chaves.length && i<nums.length;i++){
        const key = chaves[i]
        if (!p[key] && nums[i]) p[key] = nums[i]
      }
    }
  }catch(e){}

  return p
}

// Render helpers — mantêm a renderização principal enxuta
function obterColunasVisiveis(meta, isMobile){
  const allCols = (meta.tabela && meta.tabela.colunas && meta.tabela.colunas.length) ? meta.tabela.colunas : []
  return isMobile ? allCols.filter(col => (col.visivelTelaPequena !== false)) : allCols
}

function renderCelula(item, c, idx){
  const field = typeof c === 'string' ? c : c.campo
  return (<td key={idx}>{item[field]}</td>)
}

// Substitui placeholders no destino usando valores do item.
// Exemplo: destino "/painel/organizacoes/unidades?organizacaoId={id}" com item.id=5
// resulta em "/painel/organizacoes/unidades?organizacaoId=5"
function aplicarDestino(destino, item, campoId){
  if (!destino) return destino
  return destino.replace(/\{(\w+)\}/g, (m, key) => {
    // first try item[key], then if key === 'id' and campoId provided, try item[campoId]
    if (item && Object.prototype.hasOwnProperty.call(item, key)) return item[key]
    if (key === 'id' && campoId && item && Object.prototype.hasOwnProperty.call(item, campoId)) return item[campoId]
    return ''
  })
}

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
  const params = useParams()
  const [confirmState, setConfirmState] = useState({ show: false, title: null, message: null, onConfirm: null })
  const [isMobile, setIsMobile] = useState(typeof window !== 'undefined' ? window.innerWidth <= 768 : false)

  useEffect(()=>{
    api.get('/meta/screens', { block: true }).then(r=>{
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
    function onResize(){ setIsMobile(window.innerWidth <= 768) }
    window.addEventListener('resize', onResize)
    return ()=> window.removeEventListener('resize', onResize)
  },[])

  useEffect(()=>{
    if (!meta) return
    const endpoint = obterEndpoint(meta)
    if (!endpoint) {
      console.error('TelaPesquisa: endpoint não definido nos metadados para', screenKey)
      setItems([]); setTotal(0); return
    }

    // construir parâmetros combinando querystring e path params
    const pathParams = params || {}
    const qp = construirParametros(query, pathParams, location.pathname, meta)
    // paginação e ordenação
    const page = query.get('page') || 1
    const pageSize = query.get('pageSize') || (meta.pagination?.pageSize || 10)
    const sortField = query.get('sortField') || null
    const sortDir = query.get('sortDir') || null

    api.get(endpoint, { params: { ...qp, page, pageSize, sortField, sortDir }, block: true }).then(r=>{
      // api.js unwraps envelope into resp.data and keeps the full envelope at resp.envelope
      const env = r.envelope || {}
      if (env.items) {
        setItems(env.items)
        setTotal(env.total || (Array.isArray(env.items) ? env.items.length : 0))
      } else if (Array.isArray(r.data)) {
        setItems(r.data)
        setTotal(env.total || r.data.length)
      } else if (r.data) {
        // single object returned
        setItems([r.data])
        setTotal(env.total || 1)
      } else {
        setItems([])
        setTotal(0)
      }
    }).catch(()=>{ setItems([]); setTotal(0) })
  },[meta, location.search, JSON.stringify(params)])

  if (!meta) return null

  return (
    <div className="page-wrapper">
      <div className="page-card w-100">
      <div className="d-flex justify-content-between align-items-center mb-2">
        <h3>{meta.titulo || 'Pesquisa'}</h3>
        <div>
          <button
            className="btn btn-primary btn-icon btn-comando-tela-pesquisa"
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
            className={`nav-link btn-icon ${!query.get('inativo') || query.get('inativo') === '0' ? 'active' : ''}`}
            onClick={()=>{ const cur = new URLSearchParams(location.search); cur.delete('inativo'); navigate({ search: cur.toString() })}}
            title="Ativos"
            aria-label="Ativos"
          >
            <i className="bi bi-check-circle" />
          </button>
        </li>
        <li className="nav-item">
          <button
            className={`nav-link btn-icon ${query.get('inativo') === '1' ? 'active' : ''}`}
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
          <div className="col-md-2 d-flex align-items-end" id="campo-hidden-inativo">
            <button className="btn btn-secondary btn-icon" title="Filtrar" aria-label="Filtrar"><i className="bi bi-funnel" /></button>
          </div>
        </form>
      </div>

      <div className="table-responsive">
      <table className="table table-striped">
        <thead>
          <tr>
            {(() => {
              const allCols = (meta.tabela.colunas && meta.tabela.colunas.length) ? meta.tabela.colunas : []
              const cols = isMobile ? allCols.filter(col => (col.visivelTelaPequena !== false)) : allCols
                const colsVisiveis = obterColunasVisiveis(meta, isMobile)
                return colsVisiveis.map((c, idx) => {
                  const field = typeof c === 'string' ? c : c.campo
                  const title = typeof c === 'string' ? c : c.titulo
                  const curField = query.get('sortField')
                  const curDir = query.get('sortDir') || 'asc'
                  const isActive = curField === field
                  const icon = isActive ? (curDir === 'asc' ? 'arrow-up' : 'arrow-down') : 'dash'
                  return (
                    <th key={idx}>
                      {title}{' '}
                      <button className="btn btn-link p-0 btn-icon" title={`Ordenar por ${title}`} aria-label={`Ordenar por ${title}`} onClick={()=>{
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
                })
            })()}
            {isMobile && <th />}
            {!isMobile && <th>Ações</th>}
          </tr>
        </thead>
        <tbody>
          {items.map(it => (
            <tr key={it.id}>
              {(() => {
                const allCols = (meta.tabela.colunas && meta.tabela.colunas.length) ? meta.tabela.colunas : []
                const cols = isMobile ? allCols.filter(col => (col.visivelTelaPequena !== false)) : allCols
                const colsVisiveis = obterColunasVisiveis(meta, isMobile)
                return colsVisiveis.map((c, idx) => renderCelula(it, c, idx))
              })()}
              {isMobile ? (
                <td>
                  <button className="btn btn-sm btn-link btn-icon" title="Mais campos" aria-label="Mais campos" onClick={() => {
                    const allCols = (meta.tabela.colunas && meta.tabela.colunas.length) ? meta.tabela.colunas : []
                    // pass ALL columns so modal shows complete record
                    modalService.openComponentModal('../Cadastros/TelaPesquisaDetalhesLinhaTelaPequena', { title: `${meta.titulo}`, item: it, columns: allCols, actions: meta.tabela.acoes })
                  }}><i className="bi bi-three-dots" /></button>
                </td>
              ) : (
                <td>
                      {meta.tabela.acoes.map((a, ai) => (
                        <React.Fragment key={ai}>
                          {a.tipo === 'navegacao' && <button className="btn btn-sm btn-link btn-icon" onClick={()=> navigate(aplicarDestino(a.destino, it, a.campo_id))}><i className={`bi bi-${a.icone}`}></i></button>}
                          {a.tipo === 'confirmacao_delete_ajax' && <button className="btn btn-sm btn-link text-danger btn-icon" onClick={()=> setConfirmState({ show: true, title: 'Excluir', message: 'Confirma exclusão?', onConfirm: async ()=>{ await api.delete(aplicarDestino(a.destino, it, a.campo_id), { block: true }); setItems(items.filter(x=> x.id !== it.id)); setConfirmState(s=> ({...s, show: false})) } }) }><i className={`bi bi-${a.icone}`}></i></button>}
                        </React.Fragment>
                      ))}
                </td>
              )}
            </tr>
          ))}
        </tbody>
      </table>
      </div>
      {/* Pagination controls */}
      {meta.pagination && (
        <div className="d-flex justify-content-between align-items-center">
          <div>Mostrando {items.length} de {total}</div>
            <div>
            <button className="btn btn-sm btn-secondary btn-icon me-2 btn-icone-paginacao" title="Anterior" aria-label="Anterior" onClick={()=>{ const cur = new URLSearchParams(useLocation().search); const p = Math.max(1, parseInt(cur.get('page')||1)-1); cur.set('page', p); navigate({ search: cur.toString() }) }}><i className="bi bi-chevron-left" /></button>
            <button className="btn btn-sm btn-secondary btn-icon btn-icone-paginacao" title="Próxima" aria-label="Próxima" onClick={()=>{ const cur = new URLSearchParams(useLocation().search); const p = Math.max(1, parseInt(cur.get('page')||1)+1); cur.set('page', p); navigate({ search: cur.toString() }) }}><i className="bi bi-chevron-right" /></button>
          </div>
        </div>
      )}

      <ConfirmModal show={confirmState.show} title={confirmState.title} message={confirmState.message} onConfirm={confirmState.onConfirm} onCancel={()=> setConfirmState(s=> ({...s, show:false}))} />
      </div>
    </div>
  )
}
