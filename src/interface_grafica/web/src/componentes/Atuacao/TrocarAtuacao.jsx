import React, { useEffect, useState } from 'react'
import api from '../../servicos/api'

export default function TrocarAtuacao({ onClose }){
  const [data, setData] = useState(null)
  const [org, setOrg] = useState(null)
  const [unidade, setUnidade] = useState(null)
  const [setor, setSetor] = useState(null)
  const [isAdmin, setIsAdmin] = useState(false)

  useEffect(()=>{
    api.get('/api/usuario/contexto').then(r=>{
      setData(r.data)
      const ultimo = r.data?.ultimoAcesso || {};
      setIsAdmin(!!r.data?.administrado)
      // prefer ultimoAcesso, otherwise auto-select when only one option
      const orgId = ultimo.organizacaoId ?? null
      const unidadeId = ultimo.organizacaoUnidadeId ?? null
      const setorId = ultimo.setorId ?? null
      setOrg(orgId)
      setUnidade(unidadeId)
      setSetor(setorId)
      // fallback: if no ultimo and only one available, auto-select
      const orgs = r.data?.organizacoes || []
      if (!orgId && orgs.length === 1) setOrg(orgs[0].id)
      const unidades = r.data?.unidades || []
      const setores = r.data?.setores || []
      if (!unidadeId) {
        const filteredUnidades = unidades.filter(u => !org || u.organizacaoId === org)
        if (filteredUnidades.length === 1) setUnidade(filteredUnidades[0].id)
      }
      if (!setorId) {
        const filteredSetores = setores.filter(s => (!org || s.organizacaoId === org) && (!unidade || s.organizacaoUnidadeId === unidade))
        if (filteredSetores.length === 1) setSetor(filteredSetores[0].id)
      }
    }).catch(()=> setData(null))
  },[])

  if (!data) return (
    <div className="modal show d-block" tabIndex="-1">
      <div className="modal-dialog"><div className="modal-content"><div className="modal-body">Carregando...</div></div></div>
    </div>
  )

  const organizacoes = data.organizacoes || []
  const unidades = data.unidades || []
  const setores = data.setores || []
  const ultimo = data.ultimoAcesso || {}

  function submit(){
    api.post('/api/usuario/contexto', { organizacaoId: org, organizacaoUnidadeId: unidade, setorId: setor }, { block: true }).then(()=>{
      window.location.reload()
    }).catch(()=> alert('Falha ao atualizar contexto'))
  }

  function labelOrg(o){
    const parts = [o.nome]
    if (ultimo.organizacaoId === o.id) parts.push('(atual)')
    return parts.join(' ')
  }

  function labelUnidade(u){
    const parts = [u.nome]
    if (ultimo.organizacaoUnidadeId === u.id) parts.push('(atual)')
    return parts.join(' ')
  }

  function labelSetor(s){
    const parts = [s.nome]
    if (ultimo.setorId === s.id) parts.push('(atual)')
    return parts.join(' ')
  }

  return (
    <div className="modal show d-block" tabIndex="-1">
      <div className="modal-dialog">
        <div className="modal-content">
          <div className="modal-header">
            <h5 className="modal-title">Trocar de Organização / Setor</h5>
            <button type="button" className="btn-close" onClick={onClose} />
          </div>
          <div className="modal-body">
            {isAdmin && <div className="alert alert-info small">Administrador: acesso a todas as organizações e setores.</div>}

            <div className="mb-2">
              <label className="form-label">Organização</label>
              <select className="form-select" value={org||''} onChange={e=> { setOrg(e.target.value ? Number(e.target.value) : null); setUnidade(null); setSetor(null); }}>
                <option value="">(Nenhuma)</option>
                {organizacoes.map(o=> <option key={o.id} value={o.id}>{labelOrg(o)}</option>)}
              </select>
            </div>

            <div className="mb-2">
              <label className="form-label">Unidade</label>
              <select className="form-select" value={unidade||''} onChange={e=> { setUnidade(e.target.value ? Number(e.target.value) : null); setSetor(null); }}>
                <option value="">(Nenhuma)</option>
                {unidades.filter(u=> !org || u.organizacaoId === org).map(u=> <option key={u.id} value={u.id}>{labelUnidade(u)}</option>)}
              </select>
            </div>

            <div className="mb-2">
              <label className="form-label">Setor</label>
              <select className="form-select" value={setor||''} onChange={e=> setSetor(e.target.value ? Number(e.target.value) : null)}>
                <option value="">(Nenhum)</option>
                {setores.filter(s=> (!org || s.organizacaoId === org) && (!unidade || s.organizacaoUnidadeId === unidade)).map(s=> <option key={s.id} value={s.id}>{labelSetor(s)}</option>)}
              </select>
            </div>

            <div className="small text-muted">Contexto atual: {ultimo.organizacaoId ? `Org ${ultimo.organizacaoId}` : '(nenhuma)'} / {ultimo.organizacaoUnidadeId ? `Un ${ultimo.organizacaoUnidadeId}` : '(nenhuma)'} / {ultimo.setorId ? `Set ${ultimo.setorId}` : '(nenhuma)'}</div>
          </div>
          <div className="modal-footer">
            <button className="btn btn-secondary" onClick={onClose}>Fechar</button>
            <button className="btn btn-primary" onClick={submit}>Trocar</button>
          </div>
        </div>
      </div>
    </div>
  )
}
