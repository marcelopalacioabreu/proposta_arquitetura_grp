import React, { useEffect, useState } from 'react'
import api from '../../servicos/api'

export default function PermissoesModulos({ name, source, value, error }){
  const [modules, setModules] = useState([])
  const selected = Array.isArray(value) ? value : (value ? [value] : [])

  useEffect(()=>{
    // Prefer meta endpoint which serves aggregated modulos
    api.get('/meta/modulos', { block: true }).then(r=>{
      const m = r.data && r.data.modulos ? r.data.modulos : []
      setModules(m)
    }).catch(()=> setModules([]))
  },[source])

  function isChecked(id){
    return selected.indexOf(id) !== -1
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
                      <input className="form-check-input" type="checkbox" name={name} id={`${name}-${gi}-${ii}-${pi}`} value={perm.id} defaultChecked={isChecked(perm.id)} />
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
