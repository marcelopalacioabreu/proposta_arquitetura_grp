import React, { useEffect, useState } from 'react'
import axios from 'axios'

export default function Organizacao(){
  const [items,setItems] = useState([])
  useEffect(()=>{
    axios.get('/api/organizacao').then(r=>setItems(r.data)).catch(()=>setItems([]))
  },[])

  return (
    <div>
      <h3>Organizações</h3>
      <table>
        <thead><tr><th>Id</th><th>Nome</th><th>Ações</th></tr></thead>
        <tbody>
          {items.map(o=> (
            <tr key={o.id}><td>{o.id}</td><td>{o.nome}</td><td /></tr>
          ))}
        </tbody>
      </table>
    </div>
  )
}
