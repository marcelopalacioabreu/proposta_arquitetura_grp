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
      const orgs = r.data?.organizacoes || []
      const unidades = r.data?.unidades || []
      const setores = r.data?.setores || []

      // choose organization: prefer ultimo, otherwise first available
      let orgId = ultimo.organizacaoId ?? (orgs.length > 0 ? orgs[0].id : null)
      // choose unidade filtered by org: prefer ultimo, otherwise first matching unit
      let unidadeId = ultimo.organizacaoUnidadeId ?? null
      if (!unidadeId) {
        const filteredUnidades = unidades.filter(u => !orgId || u.organizacaoId === orgId)
        unidadeId = filteredUnidades.length > 0 ? filteredUnidades[0].id : null
      }
      // choose setor filtered by org/unidade: prefer ultimo, otherwise first matching
      let setorId = ultimo.setorId ?? null
      if (!setorId) {
        const filteredSetores = setores.filter(s => (!orgId || s.organizacaoId === orgId) && (!unidadeId || s.organizacaoUnidadeId === unidadeId))
        setorId = filteredSetores.length > 0 ? filteredSetores[0].id : null
      }

      setOrg(orgId)
      setUnidade(unidadeId)
      setSetor(setorId)
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
  const unidadesFiltradas = unidades.filter(u=> !org || u.organizacaoId === org)
  const setoresFiltrados = setores.filter(s=> (!org || s.organizacaoId === org) && (!unidade || s.organizacaoUnidadeId === unidade))

  function submit(){
    if (!org || !unidade || !setor) return alert('Selecione organização, unidade e setor válidos.')
    api.post('/api/usuario/contexto', { organizacaoId: org, organizacaoUnidadeId: unidade, setorId: setor }, { block: true }).then(()=>{
      window.location.reload()
    }).catch(err=>{
      const msg = err?.response?.data?.message || 'Falha ao atualizar contexto'
      alert(msg)
    })
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

            <hr />
                <div className="small text-muted">
                Contexto atual: {
                (() => {
                    const orgAtual = organizacoes.find(o => o.id === ultimo.organizacaoId)
                    const unidadeAtual = unidades.find(u => u.id === ultimo.organizacaoUnidadeId)
                    const setorAtual = setores.find(s => s.id === ultimo.setorId)
                    const nomeOrg = orgAtual ? orgAtual.nome : '(nenhuma)'
                    const nomeUn = unidadeAtual ? unidadeAtual.nome : '(nenhuma)'
                    const nomeSet = setorAtual ? setorAtual.nome : '(nenhuma)'
                    return `${nomeOrg} / ${nomeUn} / ${nomeSet}`
                })()
                }
                </div>
            <hr />

            {isAdmin && <div className="alert alert-info small">Administrador: acesso a todas as organizações e setores.</div>}

            <div className="mb-2">
              <label className="form-label">Organização</label>
              {organizacoes.length > 0 ? (
                <select className="form-select" value={org||''} onChange={e=> { setOrg(Number(e.target.value)); setUnidade(null); setSetor(null); }}>
                  {organizacoes.map(o=> <option key={o.id} value={o.id}>{labelOrg(o)}</option>)}
                </select>
              ) : (
                <select className="form-select" disabled>
                  <option>(Nenhuma)</option>
                </select>
              )}
            </div>

            <div className="mb-2">
              <label className="form-label">Unidade</label>
              {unidadesFiltradas.length > 0 ? (
                <select className="form-select" value={unidade||''} onChange={e=> { setUnidade(Number(e.target.value)); setSetor(null); }}>
                  {unidadesFiltradas.map(u=> <option key={u.id} value={u.id}>{labelUnidade(u)}</option>)}
                </select>
              ) : (
                <select className="form-select" disabled>
                  <option>(Nenhuma)</option>
                </select>
              )}
            </div>

            <div className="mb-2">
              <label className="form-label">Setor</label>
              {setoresFiltrados.length > 0 ? (
                <select className="form-select" value={setor||''} onChange={e=> setSetor(Number(e.target.value))}>
                  {setoresFiltrados.map(s=> <option key={s.id} value={s.id}>{labelSetor(s)}</option>)}
                </select>
              ) : (
                <select className="form-select" disabled>
                  <option>(Nenhum)</option>
                </select>
              )}
            </div>
          </div>
          <div className="modal-footer">
            <button className="btn btn-secondary" onClick={onClose}>Fechar</button>
            <button className="btn btn-primary" onClick={submit} disabled={!org || !unidade || !setor}>Trocar</button>
          </div>
        </div>
      </div>
    </div>
  )
}
