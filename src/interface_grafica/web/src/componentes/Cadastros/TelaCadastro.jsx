import React, { useEffect, useState, useRef } from 'react'
import api from '../../servicos/api'
import { useParams, useNavigate, useLocation } from 'react-router-dom'
import PermissoesModulos from './PermissoesModulos'
import SelectPesquisavel from './SelectPesquisavel'
import SubtabelaCadastro from './SubtabelaCadastro'

export default function TelaCadastro({ screenKey, closeModal }){
  const [meta, setMeta] = useState(null)
  const [model, setModel] = useState({})
  const [errors, setErrors] = useState({})
  const [subcadastrosData, setSubcadastrosData] = useState({})
  const params = useParams()
  const navigate = useNavigate()
  const location = useLocation()
  const [submitting, setSubmitting] = useState(false)
  const subcadastrosRef = useRef({})

  function obterEndpointCadastro(metaObj){
    if (!metaObj) return null
    if (metaObj.extremidade) return metaObj.extremidade
    if (metaObj.endpoint) return metaObj.endpoint
    if (metaObj.tabela && (metaObj.tabela.extremidade || metaObj.tabela.endpoint)) return metaObj.tabela.extremidade || metaObj.tabela.endpoint
    return null
  }

  function construirObjetoFormulario(formData){
    const obj = {}
    for (const [k,v] of formData.entries()){
      if (Object.prototype.hasOwnProperty.call(obj, k)){
        if (!Array.isArray(obj[k])) obj[k] = [obj[k]]
        obj[k].push(v)
      } else {
        obj[k] = v
      }
    }
      // Normalize checkbox values to boolean based on meta definitions when available
      try{
        if (meta && Array.isArray(meta.itens)){
          const normalizeField = (c) => {
            if (!c || !c.campo) return
            if (c.tipo === 'checkbox'){
              // checkbox present in formData -> 'on' or value; absent -> undefined
              obj[c.campo] = Boolean(obj[c.campo] === 'on' || obj[c.campo] === 'true' || obj[c.campo] === true)
            }
            // Converter campos select para número
            else if (c.tipo === 'select' && obj[c.campo]){
              const val = String(obj[c.campo]).trim()
              if (val === '') {
                obj[c.campo] = null
              } else {
                const num = parseInt(val, 10)
                obj[c.campo] = isNaN(num) ? val : num
              }
            }
          }
          meta.itens.forEach(it => {
            if (it.campos && Array.isArray(it.campos)) it.campos.forEach(normalizeField)
            else normalizeField(it)
          })
        }
      }catch(e){ /* ignore */ }

      // also coerce any remaining 'on' strings to true to be safe
      Object.keys(obj).forEach(k => { if (obj[k] === 'on') obj[k] = true })

      // Agregar dados dos subcadastros na submissão
      if (meta && Array.isArray(meta.subcadastros)){
        meta.subcadastros.forEach(sub => {
          if (sub.campoArmazenamento && subcadastrosRef.current[sub.nome]){
            const dados = subcadastrosRef.current[sub.nome]
            obj[sub.campoArmazenamento] = dados
          }
        })
      }

      return obj
  }

  // Extrai valores usando meta.urlTela (preferido), parâmetros de rota, ou query string
  function obterValoresCamposChave(metaObj, pathname, params, searchParams){
    const map = {}
    try{
      const chaves = metaObj?.camposChaveUrl
      
      if (metaObj?.urlTela && typeof metaObj.urlTela === 'string'){
        // Extrai placeholders da URL (ex: {organizacaoId}, {contexto})
        const keys = []
        const regexStr = metaObj.urlTela.replace(/\{(\w+)\}/g, (_, k) => { keys.push(k); return '([^/]*)' })
        const re = new RegExp('^' + regexStr + '$')
        const path = (pathname || '').split('?')[0].split('/').map(s=>s.trim()).filter(Boolean).join('/')
        const m = re.exec(path)
        if (m){ for (let i=0;i<keys.length;i++) map[keys[i]] = m[i+1] }
      } else if (Array.isArray(chaves) && chaves.length){
        // Extrai dos últimos segmentos da URL
        const segments = (pathname || '').split('/').map(s=> s.trim()).filter(Boolean)
        const start = Math.max(0, segments.length - chaves.length)
        for (let i=0;i<chaves.length;i++){
          const idx = start + i
          if (idx >= 0 && idx < segments.length) map[chaves[i]] = segments[idx]
        }
      }
      
      // Complementa com parâmetros de rota
      Object.keys(params || {}).forEach(k=>{ if (!map[k] && params[k]) map[k] = params[k] })
      
      // Complementa com query string (útil para contexto e outros parâmetros)
      if (searchParams){
        const queryParams = new URLSearchParams(searchParams)
        queryParams.forEach((value, key) => {
          if (!map[key]) map[key] = value
        })
      }
    }catch(e){
      console.error('Erro ao extrair valores de campos chave:', e)
    }
    return map
  }

  // load meta
  useEffect(()=>{ api.get('/meta/screens', { block: true }).then(r=> setMeta(r.data[screenKey])).catch(()=>{}) },[screenKey])

  // load model when editing
  useEffect(()=>{
    if (!meta) return
    if (params.id && params.id !== 'new'){
      const endpoint = obterEndpointCadastro(meta)
      if (!endpoint){ console.error('TelaCadastro: endpoint não definido nos metadados para', screenKey); return }
      console.debug('Fetching model for', endpoint, 'id=', params.id)
      api.get(`${endpoint}/${params.id}`, { block: true }).then(r=>{
        console.debug('Fetched model payload', r.data)
        setModel(r.data)
      }).catch((e)=>{ console.error('Error fetching model', e) })
    }
  },[meta, params.id])

  const camposChaveValores = meta ? obterValoresCamposChave(meta, location.pathname, params, location.search) : {}

  function getFieldValue(modelObj, campo){
    if (!modelObj) return undefined
    // direct (case-sensitive)
    if (Object.prototype.hasOwnProperty.call(modelObj, campo)) return modelObj[campo]
    // direct (case-insensitive)
    const lowerCampo = String(campo).toLowerCase()
    for (const k of Object.keys(modelObj)){
      if (String(k).toLowerCase() === lowerCampo) return modelObj[k]
    }
    // try one-level nested objects: perfil, data, etc. (case-insensitive)
    for (const k of Object.keys(modelObj)){
      const v = modelObj[k]
      if (v && typeof v === 'object'){
        if (Object.prototype.hasOwnProperty.call(v, campo)) return v[campo]
        for (const nk of Object.keys(v)){
          if (String(nk).toLowerCase() === lowerCampo) return v[nk]
        }
      }
    }
    return undefined
  }

  function renderCampo(c, key){
    // do not render hidden fields here (they are emitted at the top of the form)
    if (c.tipo === 'hidden') return null

    const colunaClass = `col-12 col-md-${c.col || 12}`
    const valor = getFieldValue(model, c.campo)
    const erro = errors[c.campo]
    // determine if this campo is driven by URL
    const fromUrl = camposChaveValores && Object.prototype.hasOwnProperty.call(camposChaveValores, c.campo)
    if (c.tipo === 'permissoes_modulos'){
      const colunaClass = `col-12 col-md-${c.col || 12}`
      const valor = model[c.campo]
      const erro = errors[c.campo]
      return (
        <div key={key} className={colunaClass}>
          <label className="form-label">
            {c.label}
            {c.obrigatorio && <span className="text-danger ms-1">*</span>}
          </label>
          <PermissoesModulos name={c.campo} source={c.source} value={valor} error={erro} />
        </div>
      )
    }

    return (
      <div key={key} className={colunaClass}>
        <label className="form-label">
          {c.label}
          {c.obrigatorio && <span className="text-danger ms-1">*</span>}
        </label>
        {c.tipo === 'checkbox' ? (
          <div className="form-check">
              {/* Use a key tied to the resolved value so the input is remounted when model is loaded */}
              <input key={`${c.campo}_${String(Boolean(valor))}`} name={c.campo} defaultChecked={Boolean(valor)} className={`form-check-input ${erro ? 'is-invalid' : ''}`} type="checkbox" />
            <label className="form-check-label">{c.label}</label>
            {erro && <div className="invalid-feedback">{erro}</div>}
          </div>
        ) : c.tipo === 'select' ? (
          fromUrl ? (
            <>
              {/* Do not render a select when value comes from URL; only hidden input and readonly label */}
              <input type="hidden" name={c.campo} value={camposChaveValores[c.campo]} />
              <div className="form-control-plaintext">{camposChaveValores[c.campo] || '—'}</div>
            </>
          ) : (
            <SelectPesquisavel name={c.campo} value={valor} error={erro} fieldConfig={c} meta={meta} />
          )
        ) : (
          <input name={c.campo} defaultValue={valor || ''} className={`form-control ${erro ? 'is-invalid' : ''}`} />
        )}
      </div>
    )
  }

  function renderSubcadastro(sub, key){
    if (!sub.nome) return null
    
    const valorCarregado = getFieldValue(model, sub.campoArmazenamento) || []
    
    return (
      <div key={key} className="col-12">
        <SubtabelaCadastro
          nome={sub.nome}
          titulo={sub.titulo}
          definicao={sub}
          valor={valorCarregado}
          meta={meta}
          onDadosAlterados={(dados) => {
            subcadastrosRef.current[sub.nome] = dados
            setSubcadastrosData({...subcadastrosData, [sub.nome]: dados})
          }}
        />
      </div>
    )
  }

  if (!meta || !Array.isArray(meta.itens)) return null

  // Collect hidden fields to render as bare inputs at the top of the form
  const hiddenInputs = []
  meta.itens.forEach(it => {
    if (it.campos && Array.isArray(it.campos)){
      it.campos.forEach(c => { if (c.tipo === 'hidden') hiddenInputs.push(c) })
    } else {
      if (it.tipo === 'hidden') hiddenInputs.push(it)
    }
  })

  // Constrói a URL de retorno usando padraoURLInterface e substituindo placeholders
  function construirURLRetorno(metaObj, valoresChave){
    if (!metaObj?.padraoURLInterface) return '/painel/organizacoes'
    
    let url = metaObj.padraoURLInterface
    
    // Substitui {placeholder} pelos valores extraídos da URL
    Object.entries(valoresChave).forEach(([chave, valor]) => {
      url = url.replace(`{${chave}}`, valor)
    })
    
    return url
  }

  // Obtém o endpoint correto para criar novo registro (alguns temos subrotas)
  function obterEndpointCriacao(metaObj, valoresChave){
    const endpoint = obterEndpointCadastro(metaObj)
    if (!endpoint) return endpoint
    
    // Alguns endpoints têm subrotas específicas
    if (endpoint === '/api/organizacao_unidades' && valoresChave.organizacaoId){
      return `/api/organizacoes/${valoresChave.organizacaoId}/unidades`
    }
    if (endpoint === '/api/organizacao_unidade_setores' && valoresChave.organizacaoUnidadeId){
      return `/api/organizacao_unidades/${valoresChave.organizacaoUnidadeId}/setores`
    }
    
    return endpoint
  }

  const handleSubmit = async (e) =>{
    e.preventDefault()
    setSubmitting(true)
    setErrors({})
    
    try{
      // Constrói o objeto a partir do formulário
      const fd = new FormData(e.target)
      const obj = construirObjetoFormulario(fd)
      
      // Garante que valores de campos-chave da URL sejam incluídos no payload
      if (meta?.camposChaveUrl && Array.isArray(meta.camposChaveUrl)){
        meta.camposChaveUrl.forEach(campo => {
          if (!(campo in obj) && camposChaveValores[campo]){
            obj[campo] = camposChaveValores[campo]
          }
        })
      }
      
      const endpoint = obterEndpointCadastro(meta)
      if (!endpoint) throw new Error('Endpoint não definido no metadado da tela')
      
      // Executa operação (criar ou atualizar)
      const isNovo = params.id === 'new'
      if (isNovo){
        const createEndpoint = obterEndpointCriacao(meta, camposChaveValores)
        console.debug('Criando novo registro em:', createEndpoint, 'payload:', obj)
        await api.post(createEndpoint, obj, { block: true })
      } else {
        console.debug('Atualizando registro:', endpoint, params.id, 'payload:', obj)
        await api.put(`${endpoint}/${params.id}`, obj, { block: true })
      }
      
      // Navega de volta ou fecha modal
      if (typeof closeModal === 'function'){
        closeModal()
      } else {
        const urlRetorno = construirURLRetorno(meta, camposChaveValores)
        navigate(urlRetorno)
      }
    }catch(err){
      console.error('Erro ao salvar:', err)
      if (err.response) console.error('Resposta do servidor:', err.response.data)
      
      // Trata erros de validação
      if (err.response?.status === 400){
        const data = err.response.data
        
        // Se for um envelope com campo 'detalhes' (novo padrão)
        if (data?.detalhes && typeof data.detalhes === 'object'){
          const errosMapeados = {}
          Object.keys(data.detalhes).forEach(campo => {
            const msgs = data.detalhes[campo]
            errosMapeados[campo] = Array.isArray(msgs) ? msgs.join(', ') : String(msgs)
          })
          setErrors(errosMapeados)
          // Se houver mensagem geral, também exibe
          if (data.mensagem){
            errosMapeados._geral = data.mensagem
          }
        }
        // Se for um envelope com mensagem, exibe como erro geral
        else if (data?.mensagem){
          setErrors({ _geral: data.mensagem })
        }
        // Se tiver campo 'errors' com validações por campo (compatibilidade)
        else if (data?.errors){
          const errosMapeados = {}
          Object.keys(data.errors).forEach(campo => {
            errosMapeados[campo] = data.errors[campo].join(', ')
          })
          setErrors(errosMapeados)
        } 
        // Se for um objeto simples, trata como está
        else if (typeof data === 'object'){
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
        {/* Exibe erro geral se houver */}
        {errors._geral && (
          <div className="alert alert-danger alert-dismissible fade show" role="alert">
            {errors._geral}
            <button type="button" className="btn-close" onClick={() => setErrors(e => ({ ...e, _geral: undefined }))}></button>
          </div>
        )}
        <form onSubmit={handleSubmit} className="row g-3">
        {/* hidden inputs first */}
        {
          hiddenInputs.map((c, hi) => {
            let valorFinal = '';

            // 1. Verifica se a propriedade 'valor' existe e está preenchida
            if (c.valor) {
              valorFinal = c.valor;
            } 
            // 2. Verifica se existe no camposChaveValores
            else if (camposChaveValores && camposChaveValores[c.campo]) {
              valorFinal = camposChaveValores[c.campo];
            } 
            // 3. Busca do model ou retorna vazio
            else {
              valorFinal = getFieldValue(model, c.campo) || '';
            }

            return (
              <input 
                key={`hidden-${hi}`} 
                type="hidden" 
                name={c.campo} 
                value={valorFinal} 
              />
            );
          })
        }
        {/*
        hiddenInputs.map((c, hi) => (
            <input key={`hidden-${hi}`} type="hidden" name={c.campo} value={(camposChaveValores && camposChaveValores[c.campo]) ? camposChaveValores[c.campo] : (getFieldValue(model,c.campo) || '')} />
        ))
        */}

        {meta.itens.map((it, idx) => {
          if (it.campos && Array.isArray(it.campos)){
            return (
              <fieldset key={idx} className="border p-3 mb-3 w-100">
                {it.titulo && <legend className="float-none w-auto px-2">{it.titulo}</legend>}
                <div className="row g-3">
                  {it.campos.map((c, ci) => renderCampo(c, `group-${idx}-${ci}`))}
                </div>
              </fieldset>
            )
          }
          const c = it
          return renderCampo(c, `single-${idx}`)
        })}

        {/* Renderizar subcadastros */}
        {meta.subcadastros && meta.subcadastros.map((sub, idx) => renderSubcadastro(sub, `subcadastro-${idx}`))}

        {/* Debug panel - temporary: show model and resolved admin flag */}
        {/*
        <div className="col-12 mt-3">
          <details>
            <summary>DEBUG: model / resolved fields</summary>
            <pre style={{maxHeight:300, overflow:'auto'}}>{JSON.stringify(model, null, 2)}</pre>
            <div>resolved administradorDoSistema: {String(getFieldValue(model, 'administradorDoSistema'))}</div>
          </details>
        </div>
        */}

        {Object.keys(errors).length > 0 && <div className="col-12"><div className="alert alert-danger">Corrija os erros no formulário.</div></div>}
        <div className="col-12">
          <button className="btn btn-primary" disabled={submitting}>{submitting ? 'Salvando...' : 'Salvar'}</button>
        </div>
        </form>
      </div>
    </div>
  )
}
