import React, { useEffect, useState } from 'react'
import api from '../../servicos/api'
import { useParams, useNavigate, useLocation } from 'react-router-dom'

function SelectField({ name, value, error, fieldConfig, meta }){
  const [options, setOptions] = useState([])
  const location = useLocation()
  const params = useParams()

  useEffect(()=>{
    let rawEndpoint = fieldConfig?.extremidadeOpcoes || fieldConfig?.optionsEndpoint || fieldConfig?.endpoint || null
    if (!rawEndpoint && meta && (meta.extremidadeOpcoes || meta.options)) rawEndpoint = (meta.extremidadeOpcoes && meta.extremidadeOpcoes[name]) || (meta.options && meta.options[name])
    if (!rawEndpoint){ setOptions([]); return }

    const ctx = { ...(params || {}) }
    const sp = new URLSearchParams(location.search)
    for (const [k,v] of sp.entries()) if (!(k in ctx)) ctx[k] = v

    const [pathPart, qsPart] = rawEndpoint.split('?')
    const finalPath = pathPart.replace(/\{(\w+)\}/g, (_, key) => ctx[key] ?? '')

    const reqParams = {}
    if (qsPart){
      qsPart.split('&').forEach(pair => {
        const [k,v] = pair.split('=')
        if (!k) return
        const replaced = (v || '').replace(/\{(\w+)\}/g, (_, key) => ctx[key] ?? '')
        if (replaced !== '') reqParams[k] = replaced
      })
    }
    if (!('pageSize' in reqParams)) reqParams.pageSize = 1000

    api.get(finalPath, { params: reqParams, block: true }).then(r=>{
      const env = r.envelope || {}
      let items = []
      if (env.items) items = env.items
      else if (Array.isArray(r.data)) items = r.data
      else if (r.data) items = [r.data]
      setOptions(items)
    }).catch(()=> setOptions([]))
  },[name, location.search, fieldConfig, meta])

  return (
    <select name={name} defaultValue={value || ''} className={`form-select ${error ? 'is-invalid' : ''}`} disabled={fieldConfig && fieldConfig._disabled === true}>
      <option value="">-- selecione --</option>
      {options.map(o => (<option key={o.id} value={o.id}>{o.nome || o.Nome || o.id}</option>))}
    </select>
  )
}

export default function TelaCadastro({ screenKey, closeModal }){
  const [meta, setMeta] = useState(null)
  const [model, setModel] = useState({})
  const [errors, setErrors] = useState({})
  const params = useParams()
  const navigate = useNavigate()
  const location = useLocation()
  const [submitting, setSubmitting] = useState(false)

  function obterEndpointCadastro(metaObj){
    if (!metaObj) return null
    if (metaObj.extremidade) return metaObj.extremidade
    if (metaObj.endpoint) return metaObj.endpoint
    if (metaObj.tabela && (metaObj.tabela.extremidade || metaObj.tabela.endpoint)) return metaObj.tabela.extremidade || metaObj.tabela.endpoint
    return null
  }

  function construirObjetoFormulario(formData){
    const obj = {}
    for (const [k,v] of formData.entries()) obj[k] = v
    return obj
  }

  // Extrai valores usando meta.urlTela (preferido) ou mapeia camposChaveUrl para segmentos da URL
  function obterValoresCamposChave(metaObj, pathname, params){
    const map = {}
    try{
      const chaves = metaObj?.camposChaveUrl
      if (metaObj?.urlTela && typeof metaObj.urlTela === 'string'){
        const keys = []
        const regexStr = metaObj.urlTela.replace(/\{(\w+)\}/g, (_, k) => { keys.push(k); return '([^/]*)' })
        const re = new RegExp('^' + regexStr + '$')
        const path = (pathname || '').split('?')[0].split('/').map(s=>s.trim()).filter(Boolean).join('/')
        const m = re.exec(path)
        if (m){ for (let i=0;i<keys.length;i++) map[keys[i]] = m[i+1] }
        Object.keys(params || {}).forEach(k=>{ if (!map[k] && params[k]) map[k] = params[k] })
      } else if (Array.isArray(chaves) && chaves.length){
        const segments = (pathname || '').split('/').map(s=> s.trim()).filter(Boolean)
        const start = Math.max(0, segments.length - chaves.length)
        for (let i=0;i<chaves.length;i++){
          const idx = start + i
          if (idx >= 0 && idx < segments.length) map[chaves[i]] = segments[idx]
        }
        Object.keys(params || {}).forEach(k=>{ if (!map[k] && params[k]) map[k] = params[k] })
      }
    }catch(e){}
    return map
  }

  // load meta
  useEffect(()=>{ api.get('/meta/screens', { block: true }).then(r=> setMeta(r.data[screenKey])).catch(()=>{}) },[screenKey])

  // load model when editing
  useEffect(()=>{
    if (!meta) return
    if (params.id && params.id !== 'new'){
      const endpoint = obterEndpointCadastro(meta)
      if (!endpoint){ console.error('TelaCadastro: endpoint não definido nos metadados para', screenKey); return }
      api.get(`${endpoint}/${params.id}`, { block: true }).then(r=> setModel(r.data)).catch(()=>{})
    }
  },[meta, params.id])

  const camposChaveValores = meta ? obterValoresCamposChave(meta, location.pathname, params) : {}

  function renderCampo(c, key){
    const colunaClass = `col-12 col-md-${c.col || 12}`
    const valor = model[c.campo]
    const erro = errors[c.campo]
    // determine if this campo is driven by URL
    const fromUrl = camposChaveValores && Object.prototype.hasOwnProperty.call(camposChaveValores, c.campo)
    return (
      <div key={key} className={colunaClass}>
        <label className="form-label">{c.label}</label>
        {c.tipo === 'checkbox' ? (
          <div className="form-check">
            <input name={c.campo} defaultChecked={valor ?? true} className={`form-check-input ${erro ? 'is-invalid' : ''}`} type="checkbox" />
            <label className="form-check-label">{c.label}</label>
            {erro && <div className="invalid-feedback">{erro}</div>}
          </div>
        ) : c.tipo === 'select' ? (
          fromUrl ? (
            <>
              {/* Do not render a select when value comes from URL; only hidden input and readonly label */}
              <input type="hidden" name={c.campo} value={camposChaveValores[c.campo]} />
              <div className="form-control-plaintext">{camposChaveValores[c.campo] || '—'}</div>
            </>
          ) : (
            <SelectField name={c.campo} value={valor} error={erro} fieldConfig={c} meta={meta} />
          )
        ) : c.tipo === 'hidden' ? (
          // render only a hidden input; when value comes from URL prefer that
          <input type="hidden" name={c.campo} value={(fromUrl ? camposChaveValores[c.campo] : (valor || ''))} />
        ) : (
          <input name={c.campo} defaultValue={valor || ''} className={`form-control ${erro ? 'is-invalid' : ''}`} />
        )}
      </div>
    )
  }

  if (!meta || !Array.isArray(meta.itens)) return null

  const handleSubmit = async (e) =>{
    e.preventDefault()
    setSubmitting(true)
    setErrors({})
    const fd = new FormData(e.target)
    const obj = construirObjetoFormulario(fd)
    // ensure camposChaveUrl values from URL are included in payload when missing
    if (meta && meta.camposChaveUrl && Array.isArray(meta.camposChaveUrl)){
      for (const k of meta.camposChaveUrl){ if (!(k in obj) && camposChaveValores[k]) obj[k] = camposChaveValores[k] }
    }
    try{
      const endpoint = obterEndpointCadastro(meta)
      if (!endpoint) throw new Error('Endpoint não definido no metadado da tela')

      if (params.id === 'new'){
        let createEndpoint = endpoint
        try{
          if (endpoint === '/api/organizacao_unidades' && camposChaveValores.organizacaoId){ createEndpoint = `/api/organizacoes/${camposChaveValores.organizacaoId}/unidades` }
          if (endpoint === '/api/organizacao_unidade_setores' && camposChaveValores.organizacaoUnidadeId){ createEndpoint = `/api/organizacao_unidades/${camposChaveValores.organizacaoUnidadeId}/setores` }
        }catch(e){}
        await api.post(createEndpoint, obj, { block: true })
      }
      else await api.put(`${endpoint}/${params.id}`, obj, { block: true })
      if (typeof closeModal === 'function') closeModal()
      else navigate('/painel/organizacoes')
    }catch(err){
      if (err.response && err.response.status === 400){
        const data = err.response.data
        if (data && data.errors){
          const map = {}
          Object.keys(data.errors).forEach(k => { map[k] = data.errors[k].join(', ') })
          setErrors(map)
        } else if (typeof data === 'object'){
          setErrors(data)
        }
      }
    }finally{ setSubmitting(false) }
  }

  return (
    <div className="page-wrapper">
      <div className="page-card w-100">
        <h3>{meta.titulo || 'Cadastro'}</h3>
        <form onSubmit={handleSubmit} className="row g-3">
        {meta.itens.map((it, idx) => {
          if (it.campos && Array.isArray(it.campos)){
            return (
              <fieldset key={idx} className="border p-3 mb-3 w-100">
                {it.titulo && <legend className="float-none w-auto px-2">{it.titulo}</legend>}
                <div className="row g-3">
                  {it.campos.map((c, ci) => renderCampo(c, `group-${idx}-${ci}`))}
                </div>
              </fieldset>
            )
          }
          const c = it
          return renderCampo(c, `single-${idx}`)
        })}

        {Object.keys(errors).length > 0 && <div className="col-12"><div className="alert alert-danger">Corrija os erros no formulário.</div></div>}
        <div className="col-12">
          <button className="btn btn-primary" disabled={submitting}>{submitting ? 'Salvando...' : 'Salvar'}</button>
        </div>
        </form>
      </div>
    </div>
  )
}
