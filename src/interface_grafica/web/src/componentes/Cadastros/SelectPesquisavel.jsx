import React, { useEffect, useState, useCallback, useRef } from 'react'
import api from '../../servicos/api'
import { useParams, useLocation } from 'react-router-dom'

/**
 * SelectPesquisavel - Componente genérico de select sensível ao contexto
 * 
 * Características:
 * - Carrega opções dinamicamente via API
 * - Sensível ao contexto (substitui placeholders como {organizacaoId})
 * - Suporta múltiplas variações de configuração (url, endpoint, extremidade, enumeracao)
 * - Tratamento robusto de diferentes formatos de resposta
 * - Cache de opções para evitar requisições repetidas
 * - Busca/filtro local de opções (futuro)
 * 
 * Uso:
 * <SelectPesquisavel 
 *   name="organizacaoId" 
 *   value={model.organizacaoId}
 *   fieldConfig={{ url: "/api/organizacoes", optionLabel: "nome" }}
 *   meta={meta}
 *   error={errors.organizacaoId}
 *   disabled={false}
 * />
 */

const cacheOpcoes = {} // Cache global para evitar requisições repetidas

export default function SelectPesquisavel({
  name,
  value,
  fieldConfig = {},
  meta = {},
  error,
  disabled = false,
  onChange,
  placeholder = "-- selecione --"
}) {
  const [opcoes, setOpcoes] = useState([])
  const [carregando, setCarregando] = useState(false)
  const [erro, setErro] = useState(null)
  const params = useParams()
  const location = useLocation()
  const cacheKeyRef = useRef(null)

  // Resolver endpoint a partir de várias possibilidades
  const obterEndpoint = useCallback(() => {
    let endpoint = null
    
    // 1. Tentar fieldConfig (ordem de preferência)
    endpoint = fieldConfig?.url || 
               fieldConfig?.endpoint || 
               fieldConfig?.extremidade || 
               fieldConfig?.extremidadeOpcoes ||
               fieldConfig?.optionsEndpoint ||
               fieldConfig?.optionEndpoint

    // 2. Tentar em meta (se não encontrou em fieldConfig)
    if (!endpoint && meta) {
      if (meta.extremidadeOpcoes && meta.extremidadeOpcoes[name]) {
        endpoint = meta.extremidadeOpcoes[name]
      } else if (meta.options && meta.options[name]) {
        endpoint = meta.options[name]
      }
    }

    // 3. Tentar resolver enumeracao (para enumerações do sistema)
    if (!endpoint && fieldConfig?.enumeracao) {
      endpoint = `/api/enumeracoes/${fieldConfig.enumeracao}`
    }

    return endpoint
  }, [fieldConfig, meta, name])

  // Substituir placeholders no endpoint com valores do contexto
  const resolverEndpoint = useCallback(() => {
    const baseEndpoint = obterEndpoint()
    if (!baseEndpoint) return null

    const ctx = { ...params }
    const sp = new URLSearchParams(location.search)
    
    // Adicionar valores da querystring ao contexto
    for (const [k, v] of sp.entries()) {
      if (!(k in ctx)) ctx[k] = v
    }

    // Substituir placeholders no endpoint
    const [pathPart, qsPart] = baseEndpoint.split('?')
    const finalPath = pathPart.replace(/\{(\w+)\}/g, (_, key) => ctx[key] ?? '')

    // Construir query parameters
    const reqParams = {}
    if (qsPart) {
      qsPart.split('&').forEach(pair => {
        const [k, v] = pair.split('=')
        if (!k) return
        const replaced = (v || '').replace(/\{(\w+)\}/g, (_, key) => ctx[key] ?? '')
        if (replaced !== '') reqParams[k] = replaced
      })
    }

    // Adicionar pageSize se não existir
    if (!('pageSize' in reqParams)) reqParams.pageSize = 1000

    return { path: finalPath, params: reqParams }
  }, [obterEndpoint, params, location.search])

  // Carregar opções quando endpoint ou contexto mudar
  useEffect(() => {
    const config = resolverEndpoint()
    if (!config) {
      setOpcoes([])
      return
    }

    const cacheKey = `${config.path}?${JSON.stringify(config.params)}`
    cacheKeyRef.current = cacheKey

    // Verificar cache
    if (cacheOpcoes[cacheKey]) {
      setOpcoes(cacheOpcoes[cacheKey])
      return
    }

    setCarregando(true)
    setErro(null)

    api.get(config.path, { params: config.params, block: true })
      .then(r => {
        let items = []

        // Tentar extrair items de várias estruturas de resposta
        if (r.envelope && r.envelope.items) {
          items = r.envelope.items
        } else if (r.data?.items) {
          items = r.data.items
        } else if (Array.isArray(r.data)) {
          items = r.data
        } else if (r.data && typeof r.data === 'object') {
          items = [r.data]
        }

        // Filtrar valores nulos/undefined
        items = items.filter(i => i != null)

        // Armazenar em cache
        cacheOpcoes[cacheKey] = items
        setOpcoes(items)
        setCarregando(false)
      })
      .catch(err => {
        console.error(`Erro ao carregar opções para ${name}:`, err)
        setErro(err.message || 'Erro ao carregar opções')
        setOpcoes([])
        setCarregando(false)
      })
  }, [resolverEndpoint, name])

  // Extrair label da opção (suporta múltiplas variações)
  const extrairLabel = (item) => {
    if (!item) return ''
    
    // Tentar configuração customizada
    if (fieldConfig?.optionLabel) {
      return item[fieldConfig.optionLabel] || ''
    }

    // Ordem de preferência comum (inclui descricao para enumerações)
    return item.nome || 
           item.Nome ||
           item.name ||
           item.Name ||
           item.titulo ||
           item.Titulo ||
           item.label ||
           item.Label ||
           item.descricao ||
           item.Descricao ||
           item.texto ||
           item.Texto ||
           item.id ||
           item.Id ||
           ''
  }

  // Extrair ID da opção (suporta chave para enumerações)
  const extrairId = (item) => {
    if (!item) return ''
    
    let id
    if (fieldConfig?.optionId) {
      id = item[fieldConfig.optionId]
    } else {
      // Para enumerações, chave é o ID; para outros, é id
      id = item.chave || item.id || item.Id || item.value
    }

    // Sempre retorna como string para consistência com o value do select
    return id !== undefined && id !== null ? String(id) : ''
  }

  const handleChange = (e) => {
    const novoValor = e.target.value
    if (onChange) {
      onChange(novoValor)
    }
  }

  return (
    <div className="select-pesquisavel-wrapper">
      <select
        name={name}
        defaultValue={String(value || '')}
        onChange={handleChange}
        disabled={disabled || carregando}
        className={`form-select ${error ? 'is-invalid' : ''} ${carregando ? 'opacity-75' : ''}`}
        title={carregando ? 'Carregando opções...' : erro ? erro : ''}
      >
        <option value="">{placeholder}</option>
        {opcoes.map(item => (
          <option key={extrairId(item)} value={String(extrairId(item))}>
            {extrairLabel(item)}
          </option>
        ))}
      </select>
      {carregando && <small className="text-muted d-block mt-1">Carregando...</small>}
      {erro && <small className="text-danger d-block mt-1">{erro}</small>}
      {!carregando && opcoes.length === 0 && <small className="text-warning d-block mt-1">Nenhuma opção disponível</small>}
    </div>
  )
}
