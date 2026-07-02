import React, { useEffect, useState } from 'react'
import axios from 'axios'
import { useParams, useNavigate } from 'react-router-dom'

export default function TelaCadastro({ screenKey }){
  const [meta, setMeta] = useState(null)
  const [model, setModel] = useState({})
  const params = useParams()
  const navigate = useNavigate()

  useEffect(()=>{
    axios.get('/meta/screens').then(r=> setMeta(r.data[screenKey]))
    if (params.id && params.id !== 'new'){
      axios.get(`/api/organizacoes/${params.id}`).then(r=> setModel(r.data)).catch(()=>{})
    }
  },[screenKey, params.id])

  if (!meta) return <div>Carregando...</div>

  const handleSubmit = async (e) =>{
    e.preventDefault()
    const fd = new FormData(e.target)
    const obj = {}
    for (const [k,v] of fd.entries()) obj[k]=v
    if (params.id === 'new'){
      await axios.post('/api/organizacoes', obj)
      navigate('/painel/organizacoes')
    } else {
      // TODO: update endpoint
      await axios.post('/api/organizacoes', obj)
      navigate('/painel/organizacoes')
    }
  }

  return (
    <div>
      <h3>Cadastro</h3>
      <form onSubmit={handleSubmit} className="row g-3">
        {meta.campos.map((c, idx) => (
          <div key={idx} className={`col-12 col-md-${c.col || 12}`}>
            <label className="form-label">{c.label}</label>
            <input name={c.campo} defaultValue={model[c.campo] || ''} className="form-control" />
          </div>
        ))}
        <div className="col-12">
          <button className="btn btn-primary">Salvar</button>
        </div>
      </form>
    </div>
  )
}
