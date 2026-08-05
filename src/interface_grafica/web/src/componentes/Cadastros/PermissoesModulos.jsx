import React, { useEffect, useState } from 'react'
import api from '../../servicos/api'

export default function PermissoesModulos({ name, source, value, error }){
  const [modules, setModules] = useState([])
  const [selected, setSelected] = useState([])

  // Load modules/groups
  useEffect(()=>{
    api.get('/meta/modulos', { block: true }).then(r=>{
      const m = r.data && r.data.modulos ? r.data.modulos : []
      setModules(m)
    }).catch(()=> setModules([]))
  },[source])

  // Normalize incoming `value` into selected permission ids array.
  useEffect(()=>{
    async function resolveValue(v){
      if (!v){ setSelected([]); return }
      // If already an array of strings
      if (Array.isArray(v)){
        setSelected(v.map(x => String(x)))
        return
      }
      // If it's an object with `permissoes` property
      if (typeof v === 'object'){
        if (Array.isArray(v.permissoes)){
          setSelected(v.permissoes.map(x => String(x)))
          return
        }
        // Some endpoints return { perfil: {...}, permissoes: [...] }
        if (v.perfil && Array.isArray(v.permissoes)){
          setSelected(v.permissoes.map(x => String(x)))
          return
        }
        // If object seems to be a Perfil itself (has id), try fetching detailed payload
        if (v.id){
          try{
            const resp = await api.get(`/api/perfis/${v.id}`, { block: false })
            const payload = resp.data
            if (payload){
              if (Array.isArray(payload.permissoes)){
                setSelected(payload.permissoes.map(x => String(x))); return
              }
              if (payload.perfil && Array.isArray(payload.permissoes)){
                setSelected(payload.permissoes.map(x => String(x))); return
              }
            }
          }catch(e){ /* ignore fetch errors */ }
        }
      }
      // If value is numeric id
      if (typeof v === 'number' || (typeof v === 'string' && /^\/?\d+$/.test(String(v)))){
        try{
          const id = typeof v === 'number' ? v : Number(String(v))
          if (!Number.isNaN(id)){
            const resp = await api.get(`/api/perfis/${id}`, { block: false })
            const payload = resp.data
            if (payload){
              if (Array.isArray(payload.permissoes)){
                setSelected(payload.permissoes.map(x => String(x))); return
              }
              if (payload.perfil && Array.isArray(payload.permissoes)){
                setSelected(payload.permissoes.map(x => String(x))); return
              }
            }
          }
        }catch(e){ /* ignore */ }
      }

      // fallback: empty
      setSelected([])
    }

    resolveValue(value)
  },[value])

  function isChecked(id){
    return selected.indexOf(String(id)) !== -1
  }

  return (
    <div className={`permissoes-modulos ${error ? 'is-invalid' : ''}`}>
      {modules.map((g, gi) => (
        <fieldset key={gi} className="border p-2 mb-2">
          <legend className="px-2">{g.grupo}</legend>
          <div className="row">
            {Array.isArray(g.itens) ? g.itens.map((it, ii) => (
              <div key={ii} className="col-12 col-md-6">
                <strong>{it.texto}</strong>
                <div className="ps-2">
                  {Array.isArray(it.permissoes) ? it.permissoes.map((perm, pi) => (
                    <div key={pi} className="form-check">
                      <input
                        className="form-check-input"
                        type="checkbox"
                        name={name}
                        id={`${name}-${gi}-${ii}-${pi}`}
                        value={perm.id}
                        checked={isChecked(perm.id)}
                        onChange={() => {
                          const idStr = String(perm.id)
                          setSelected(prev => prev.indexOf(idStr) === -1 ? [...prev, idStr] : prev.filter(x => x !== idStr))
                        }}
                      />
                      <label className="form-check-label" htmlFor={`${name}-${gi}-${ii}-${pi}`}>{perm.texto}</label>
                    </div>
                  )) : null}
                </div>
              </div>
            )) : null}
          </div>
        </fieldset>
      ))}
    </div>
  )
}
