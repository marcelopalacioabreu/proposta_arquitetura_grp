import React, { useEffect, useState } from 'react'
import api from '../../servicos/api'
import { useParams, useNavigate, useLocation } from 'react-router-dom'

function SelectField({ name, value, error, fieldConfig, meta }){
  const [options, setOptions] = useState([])
  const location = useLocation()
  const params = useParams()

  useEffect(()=>{
    // Strict metadata-driven: prefer Portuguese key 'extremidadeOpcoes', then 'optionsEndpoint', then meta mappings
    let rawEndpoint = fieldConfig?.extremidadeOpcoes || fieldConfig?.optionsEndpoint || fieldConfig?.endpoint || null
    if (!rawEndpoint && meta && (meta.extremidadeOpcoes || meta.options)) rawEndpoint = (meta.extremidadeOpcoes && meta.extremidadeOpcoes[name]) || (meta.options && meta.options[name])
    if (!rawEndpoint){ setOptions([]); return }

    // Build context from path params and query string
    const ctx = { ...(params || {}) }
    const sp = new URLSearchParams(location.search)
    for (const [k,v] of sp.entries()) if (!(k in ctx)) ctx[k] = v

    // support placeholder substitution in endpoint and querystring, e.g. /api/units?orgId={organizacaoId}
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
    // default pageSize for selects to retrieve full list unless metadata overrides
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
    <select name={name} defaultValue={value || ''} className={`form-select ${error ? 'is-invalid' : ''}`}>
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

  useEffect(()=>{
    api.get('/meta/screens', { block: true }).then(r=> setMeta(r.data[screenKey]))
    if (params.id && params.id !== 'new'){
      // determine API endpoint from metadata
      api.get('/meta/screens').then(r=>{
        const m = r.data[screenKey]
        let endpoint = null
        if (m && m.endpoint) endpoint = m.endpoint
        else if (m && m.tabela && m.tabela.endpoint) endpoint = m.tabela.endpoint
        if (!endpoint){ console.error('TelaCadastro: endpoint not defined in metadata for', screenKey); return }
        api.get(`${endpoint}/${params.id}`, { block: true }).then(r=> setModel(r.data)).catch(()=>{})
      })
    }
  },[screenKey, params.id])

  if (!meta || !Array.isArray(meta.itens)) return null

  const handleSubmit = async (e) =>{
    e.preventDefault()
    setSubmitting(true)
    setErrors({})
    const fd = new FormData(e.target)
    const obj = {}
    for (const [k,v] of fd.entries()) obj[k]=v
    try{
      // derive endpoint from meta (strict)
      const m = meta
      let endpoint = null
      if (m && m.endpoint) endpoint = m.endpoint
      else if (m && m.tabela && m.tabela.endpoint) endpoint = m.tabela.endpoint
      if (!endpoint){ throw new Error('Endpoint não definido no metadado da tela') }

      if (params.id === 'new'){
        await api.post(endpoint, obj, { block: true })
      } else {
        await api.put(`${endpoint}/${params.id}`, obj, { block: true })
      }
      if (typeof closeModal === 'function') closeModal()
      else navigate('/painel/organizacoes')
    }catch(err){
      if (err.response && err.response.status === 400){
        const data = err.response.data
        // try ModelState-like shape: { errors: { field: [msg] } }
        if (data && data.errors){
          const map = {}
          Object.keys(data.errors).forEach(k => { map[k] = data.errors[k].join(', ') })
          setErrors(map)
        } else if (typeof data === 'object'){
          // flatter mapping
          setErrors(data)
        }
      }
    }finally{
      setSubmitting(false)
    }
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
                  {it.campos.map((c, ci) => (
                    <div key={ci} className={`col-12 col-md-${c.col || 12}`}>
                      <label className="form-label">{c.label}</label>
                        {c.tipo === 'checkbox' ? (
                        <div className="form-check">
                          <input name={c.campo} defaultChecked={model[c.campo] ?? true} className={`form-check-input ${errors[c.campo] ? 'is-invalid' : ''}`} type="checkbox" />
                          <label className="form-check-label">{c.label}</label>
                          {errors[c.campo] && <div className="invalid-feedback">{errors[c.campo]}</div>}
                        </div>
                        ) : c.tipo === 'select' ? (
                          <SelectField name={c.campo} value={model[c.campo]} error={errors[c.campo]} fieldConfig={c} meta={meta} />
                        ) : (
                          <input name={c.campo} defaultValue={model[c.campo] || ''} className={`form-control ${errors[c.campo] ? 'is-invalid' : ''}`} />
                        )}
                    </div>
                  ))}
                </div>
              </fieldset>
            )
          }
          const c = it
          return (
            <div key={idx} className={`col-12 col-md-${c.col || 12}`}>
              <label className="form-label">{c.label}</label>
              {c.tipo === 'checkbox' ? (
                <div className="form-check">
                  <input name={c.campo} defaultChecked={model[c.campo] ?? true} className={`form-check-input ${errors[c.campo] ? 'is-invalid' : ''}`} type="checkbox" />
                  <label className="form-check-label">{c.label}</label>
                  {errors[c.campo] && <div className="invalid-feedback">{errors[c.campo]}</div>}
                </div>
              ) : c.tipo === 'select' ? (
                <SelectField name={c.campo} value={model[c.campo]} error={errors[c.campo]} fieldConfig={c} meta={meta} />
              ) : (
                <input name={c.campo} defaultValue={model[c.campo] || ''} className={`form-control ${errors[c.campo] ? 'is-invalid' : ''}`} />
              )}
            </div>
          )
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
