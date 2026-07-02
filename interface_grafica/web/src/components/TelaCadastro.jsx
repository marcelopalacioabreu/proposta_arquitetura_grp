import React, { useEffect, useState } from 'react'
import axios from 'axios'
import { useParams, useNavigate } from 'react-router-dom'

export default function TelaCadastro({ screenKey }){
  const [meta, setMeta] = useState(null)
  const [model, setModel] = useState({})
  const [errors, setErrors] = useState({})
  const params = useParams()
  const navigate = useNavigate()
  const [submitting, setSubmitting] = useState(false)

  useEffect(()=>{
    axios.get('/meta/screens').then(r=> setMeta(r.data[screenKey]))
    if (params.id && params.id !== 'new'){
      // determine API endpoint from metadata
      axios.get('/meta/screens').then(r=>{
        const m = r.data[screenKey]
        let endpoint = null
        if (m && m.endpoint) endpoint = m.endpoint
        else if (m && m.tabela && m.tabela.endpoint) endpoint = m.tabela.endpoint
        if (!endpoint && m && m.tabela && Array.isArray(m.tabela.acoes)){
          for (const a of m.tabela.acoes){
            if (a.destino && a.destino.startsWith('/api/')){
              endpoint = a.destino.replace(/\/{?\{id\}}?$/, '')
              endpoint = endpoint.replace(/\/\{id\}$/, '')
              break
            }
          }
        }
        if (!endpoint) endpoint = '/api/organizacoes'
        axios.get(`${endpoint}/${params.id}`).then(r=> setModel(r.data)).catch(()=>{})
      })
    }
  },[screenKey, params.id])

  if (!meta) return <div>Carregando...</div>

  const handleSubmit = async (e) =>{
    e.preventDefault()
    setSubmitting(true)
    setErrors({})
    const fd = new FormData(e.target)
    const obj = {}
    for (const [k,v] of fd.entries()) obj[k]=v
    try{
      // derive endpoint from meta
      const m = meta
      let endpoint = null
      if (m && m.endpoint) endpoint = m.endpoint
      else if (m && m.tabela && m.tabela.endpoint) endpoint = m.tabela.endpoint
      if (!endpoint && m && m.tabela && Array.isArray(m.tabela.acoes)){
        for (const a of m.tabela.acoes){
          if (a.destino && a.destino.startsWith('/api/')){
            endpoint = a.destino.replace(/\/{?\{id\}}?$/, '')
            endpoint = endpoint.replace(/\/\{id\}$/, '')
            break
          }
        }
      }
      if (!endpoint) endpoint = '/api/organizacoes'

      if (params.id === 'new'){
        await axios.post(endpoint, obj)
      } else {
        await axios.put(`${endpoint}/${params.id}`, obj)
      }
      navigate('/painel/organizacoes')
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
    <div>
      <h3>Cadastro</h3>
      <form onSubmit={handleSubmit} className="row g-3">
        {meta.campos.map((c, idx) => (
          <div key={idx} className={`col-12 col-md-${c.col || 12}`}>
            <label className="form-label">{c.label}</label>
            <input name={c.campo} defaultValue={model[c.campo] || ''} className={`form-control ${errors[c.campo] ? 'is-invalid' : ''}`} />
            {errors[c.campo] && <div className="invalid-feedback">{errors[c.campo]}</div>}
          </div>
        ))}
        {Object.keys(errors).length > 0 && <div className="col-12"><div className="alert alert-danger">Corrija os erros no formulário.</div></div>}
        <div className="col-12">
          <button className="btn btn-primary" disabled={submitting}>{submitting ? 'Salvando...' : 'Salvar'}</button>
        </div>
      </form>
    </div>
  )
}
