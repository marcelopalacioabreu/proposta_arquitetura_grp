import React, { useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import axios from 'axios'

export default function Menu(){
  const [modulos, setModulos] = useState([])
  useEffect(()=>{
    axios.get('/meta/modulos').then(r=> setModulos(r.data.modulos || [] )).catch(()=>setModulos([]))
  },[])

  return (
    <div className="list-group">
      {modulos.map((g, gi) => (
        <div key={gi} className="mb-3">
          <div className="small text-muted">{g.group}</div>
          {g.items && g.items.map((it, ii) => (
            <Link key={ii} className="list-group-item list-group-item-action d-flex align-items-center" to={it.url}>
              {it.icon && <i className={`bi bi-${it.icon} me-2`} />}<span>{it.label}</span>
            </Link>
          ))}
        </div>
      ))}
    </div>
  )
}
