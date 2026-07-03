import React, { useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import axios from 'axios'

export default function Menu(){
  const [modulos, setModulos] = useState([])
  useEffect(()=>{
    axios.get('/meta/modulos').then(r=> setModulos(r.data.modulos || [] )).catch(()=>setModulos([]))
  },[])

  return (
    <div className="menu">
      {modulos.map((g, gi) => (
        <div key={gi} className="mb-3">
          <div className="small text-muted px-2">{g.group}</div>
          <div className="list-group">
            {g.items && g.items.map((it, ii) => (
              <Link key={ii} title={it.label} className="list-group-item list-group-item-action d-flex align-items-center" to={it.url}>
                {it.icon && <i className={`bi bi-${it.icon}`} />}<span className="item-label">{it.label}</span>
              </Link>
            ))}
          </div>
        </div>
      ))}
    </div>
  )
}
