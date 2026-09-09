import React, { useEffect, useState, useRef, useCallback } from 'react'
import api from '../../servicos/api'
import { useParams, useLocation } from 'react-router-dom'

/**
 * InputAutocomplete - Campo de input com busca enquanto digita
 * 
 * Características:
 * - Input text que permite digitar para buscar
 * - Dropdown dinâmico que aparece enquanto digita
 * - Requisições à API com delay para evitar spam
 * - Cache de resultados
 * 
 * Uso:
 * <InputAutocomplete 
 *   name="cepId" 
 *   value={model.cepId}
 *   fieldConfig={{ endpoint: "/api/ceps", optionLabel: "codigo" }}
 *   onChange={(id, label) => handleChange(id, label)}
 *   placeholder="Digite para buscar CEP..."
 * />
 */

export default function InputAutocomplete({
  name,
  value,
  fieldConfig = {},
  meta = {},
  error,
  disabled = false,
  onChange,
  placeholder = "Digite para buscar...",
  onDisplayLabelChange
}) {
  const [inputValue, setInputValue] = useState('')
  const [opcoes, setOpcoes] = useState([])
  const [mostrandoOpcoes, setMostrandoOpcoes] = useState(false)
  const [carregando, setCarregando] = useState(false)
  const [erroCarregamento, setErroCarregamento] = useState(null)
  const [displayLabel, setDisplayLabel] = useState('')
  const params = useParams()
  const location = useLocation()
  const timeoutRef = useRef(null)
  const wrapperRef = useRef(null)

  // Resolver endpoint
  const obterEndpoint = useCallback(() => {
    const endpoint = fieldConfig?.url || 
                    fieldConfig?.endpoint || 
                    fieldConfig?.extremidade || 
                    fieldConfig?.extremidadeOpcoes
    return endpoint
  }, [fieldConfig])

  // Extrair label da opção
  const extrairLabel = (item) => {
    if (!item) return ''
    
    if (fieldConfig?.optionLabel) {
      return item[fieldConfig.optionLabel] || item.id || ''
    }

    // Ordem de preferência para CEP
    return item.codigo || 
           item.Codigo ||
           item.nome || 
           item.Nome ||
           item.name ||
           item.id ||
           item.Id ||
           ''
  }

  // Extrair ID da opção
  const extrairId = (item) => {
    if (!item) return ''
    
    const id = fieldConfig?.optionId ? 
               item[fieldConfig.optionId] : 
               (item.id || item.Id || item.chave)
    
    return id !== undefined && id !== null ? String(id) : ''
  }

  // Carregar opção selecionada no mount
  useEffect(() => {
    if (value) {
      // Se temos um valor, precisamos carregar o label dela
      // Por enquanto, apenas mostramos o ID
      setInputValue(value)
    }
  }, [value])

  // Buscar opções enquanto digita
  const buscarOpcoes = useCallback((searchTerm) => {
    const endpoint = obterEndpoint()
    if (!endpoint) {
      setOpcoes([])
      return
    }

    if (!searchTerm || searchTerm.length < 1) {
      setOpcoes([])
      setMostrandoOpcoes(false)
      return
    }

    setCarregando(true)
    setErroCarregamento(null)

    // Construir URL com search parameter
    const url = `${endpoint}${endpoint.includes('?') ? '&' : '?'}q=${encodeURIComponent(searchTerm)}&pageSize=10`

    api.get(url, { block: false })
      .then(r => {
        let items = []

        // Extrair items de várias estruturas de resposta
        if (r.envelope && r.envelope.items) {
          items = r.envelope.items
        } else if (r.data?.items) {
          items = r.data.items
        } else if (Array.isArray(r.data)) {
          items = r.data
        } else if (r.data && typeof r.data === 'object') {
          items = [r.data]
        }

        items = items.filter(i => i != null)
        setOpcoes(items)
        setMostrandoOpcoes(items.length > 0)
        setCarregando(false)
      })
      .catch(err => {
        console.error(`Erro ao buscar opções para ${name}:`, err)
        setErroCarregamento(err.message || 'Erro ao buscar opções')
        setOpcoes([])
        setMostrandoOpcoes(false)
        setCarregando(false)
      })
  }, [obterEndpoint, name])

  // Handle input change com debounce
  const handleInputChange = (e) => {
    const valor = e.target.value
    setInputValue(valor)

    // Limpar timeout anterior
    if (timeoutRef.current) clearTimeout(timeoutRef.current)

    // Debounce de 300ms antes de buscar
    timeoutRef.current = setTimeout(() => {
      buscarOpcoes(valor)
    }, 300)
  }

  // Handle seleção de opção
  const handleSelectOpcao = (item) => {
    const id = extrairId(item)
    const label = extrairLabel(item)

    setInputValue(label)
    setDisplayLabel(label)
    setMostrandoOpcoes(false)
    setOpcoes([])

    if (onChange) {
      onChange(id)
    }

    if (onDisplayLabelChange) {
      onDisplayLabelChange(label)
    }
  }

  // Fechar dropdown ao clicar fora
  useEffect(() => {
    const handleClickFora = (e) => {
      if (wrapperRef.current && !wrapperRef.current.contains(e.target)) {
        setMostrandoOpcoes(false)
      }
    }

    document.addEventListener('mousedown', handleClickFora)
    return () => document.removeEventListener('mousedown', handleClickFora)
  }, [])

  return (
    <div className="input-autocomplete-wrapper" ref={wrapperRef} style={{ position: 'relative' }}>
      <input
        type="text"
        name={name}
        value={inputValue}
        onChange={handleInputChange}
        onFocus={() => inputValue && opcoes.length > 0 && setMostrandoOpcoes(true)}
        placeholder={placeholder}
        disabled={disabled}
        className={`form-control ${error ? 'is-invalid' : ''} ${carregando ? 'opacity-75' : ''}`}
        autoComplete="off"
      />

      {carregando && (
        <small className="text-muted d-block mt-1">
          <i className="spinner-border spinner-border-sm me-2"></i>Buscando...
        </small>
      )}

      {erroCarregamento && (
        <small className="text-danger d-block mt-1">{erroCarregamento}</small>
      )}

      {mostrandoOpcoes && opcoes.length > 0 && (
        <div 
          className="list-group position-absolute w-100 mt-1"
          style={{
            top: '100%',
            zIndex: 1000,
            maxHeight: '200px',
            overflowY: 'auto',
            boxShadow: '0 4px 6px rgba(0,0,0,0.1)'
          }}
        >
          {opcoes.map((item) => (
            <button
              key={extrairId(item)}
              type="button"
              className="list-group-item list-group-item-action"
              onClick={() => handleSelectOpcao(item)}
            >
              <div className="fw-500">{extrairLabel(item)}</div>
              {item.nome && item.nome !== extrairLabel(item) && (
                <small className="text-muted">{item.nome}</small>
              )}
              {item.endereco && (
                <small className="text-muted d-block">
                  {item.endereco.bairro?.nome}, {item.endereco.cepId}
                </small>
              )}
            </button>
          ))}
        </div>
      )}

      {mostrandoOpcoes && opcoes.length === 0 && !carregando && inputValue.length > 0 && (
        <div 
          className="list-group position-absolute w-100 mt-1"
          style={{
            top: '100%',
            zIndex: 1000,
            boxShadow: '0 4px 6px rgba(0,0,0,0.1)'
          }}
        >
          <div className="list-group-item text-muted">
            Nenhum resultado encontrado para "{inputValue}"
          </div>
        </div>
      )}

      {error && <div className="invalid-feedback d-block">{error}</div>}
    </div>
  )
}
