import React, { useEffect, useState, useCallback, useRef } from 'react'
import api from '../../servicos/api'
import SelectPesquisavel from './SelectPesquisavel'

/**
 * Componente genérico de subtabela para subcadastros associados
 * 
 * Características:
 * - Reutilizável em diferentes cadastros
 * - Gerencia eventos de adição/remoção de linhas
 * - Dispara eventos para o componente pai (TelaCadastro)
 * - Suporta múltiplos tipos de campos (text, select, checkbox, date, etc)
 * - Permite marcação de linha padrão (radio ou checkbox)
 * - Agrupa dados para envio no formulário principal
 */
export default function SubtabelaCadastro({
  nome,
  titulo,
  definicao,
  valor = [],
  meta,
  onDadosAlterados
}) {
  const [linhas, setLinhas] = useState(valor || [])
  const [linhaEmEdicao, setLinhaEmEdicao] = useState(null)
  const [novaLinha, setNovaLinha] = useState({})
  const [erros, setErros] = useState({})

  // Quando o model carrega (async) o prop valor muda de [] para os dados do servidor
  useEffect(() => {
    if (Array.isArray(valor) && valor.length > 0) {
      setLinhas(valor)
    }
  }, [valor])

  // Mantém ref atualizada para evitar loop infinito (onDadosAlterados é recriado a cada render do pai)
  const onDadosAlteradosRef = useRef(onDadosAlterados)
  useEffect(() => { onDadosAlteradosRef.current = onDadosAlterados })

  // Notificar componente pai APENAS quando linhas muda
  useEffect(() => {
    onDadosAlteradosRef.current?.(linhas)
  }, [linhas])

  const adicionarLinha = () => {
    const errosValidacao = validarLinha(novaLinha)
    if (Object.keys(errosValidacao).length > 0) {
      setErros(errosValidacao)
      return
    }

    const novasLinhas = [...linhas, { ...novaLinha, id: Date.now() }]
    
    // Se é single selection e marcou padrão, desmarcar outros
    if (definicao.selecao?.singleSelecao && novaLinha[definicao.selecao.campo]) {
      novasLinhas.forEach((l, i) => {
        if (i !== novasLinhas.length - 1) l[definicao.selecao.campo] = false
      })
    }

    setLinhas(novasLinhas)
    setNovaLinha({})
    setErros({})
  }

  const removerLinha = (id) => {
    setLinhas(linhas.filter(l => l.id !== id))
    setErros({})
  }

  const marcarComoSelecionada = (id) => {
    if (!definicao.selecao?.campo) return

    const novasLinhas = linhas.map(l => ({
      ...l,
      [definicao.selecao.campo]: l.id === id && !l[definicao.selecao.campo]
    }))

    if (definicao.selecao.singleSelecao) {
      novasLinhas.forEach((l, i) => {
        if (l.id !== id) l[definicao.selecao.campo] = false
      })
    }

    setLinhas(novasLinhas)
  }

  const validarLinha = (linha) => {
    const errosLocal = {}
    // Aqui pode adicionar validações conforme necessário
    return errosLocal
  }

  const renderCampo = (col, valor, onChange) => {
    const erro = erros[col.campo]

    switch (col.tipo) {
      case 'checkbox':
        return (
          <input
            type="checkbox"
            checked={Boolean(valor)}
            onChange={(e) => onChange(e.target.checked)}
            className={`form-check-input ${erro ? 'is-invalid' : ''}`}
          />
        )
      case 'select':
        return (
          <SelectPesquisavel
            name={col.campo}
            value={valor || ''}
            fieldConfig={col}
            meta={meta}
            error={erro}
            onChange={onChange}
          />
        )
      case 'date':
        return (
          <input
            type="date"
            value={valor || ''}
            onChange={(e) => onChange(e.target.value)}
            className={`form-control form-control-sm ${erro ? 'is-invalid' : ''}`}
          />
        )
      case 'number':
        return (
          <input
            type="number"
            value={valor || ''}
            onChange={(e) => onChange(Number(e.target.value))}
            placeholder={col.placeholder}
            className={`form-control form-control-sm ${erro ? 'is-invalid' : ''}`}
          />
        )
      case 'text':
      default:
        return (
          <input
            type="text"
            value={valor || ''}
            onChange={(e) => onChange(e.target.value)}
            placeholder={col.placeholder}
            readOnly={col.readonly}
            className={`form-control form-control-sm ${erro ? 'is-invalid' : ''}`}
          />
        )
    }
  }

  if (!definicao) return null

  return (
    <div className="card mb-3">
      <div className="card-header bg-light">
        <h6 className="mb-0">{titulo || definicao.titulo || nome}</h6>
      </div>
      <div className="card-body">
        {/* Tabela de dados existentes */}
        {linhas.length > 0 && (
          <div className="table-responsive mb-3">
            <table className="table table-sm table-hover mb-0">
              <thead className="table-light">
                <tr>
                  {definicao.selecao?.campo && (
                    <th style={{ width: '50px' }} className="text-center">
                      {definicao.selecao.label || 'Padrão'}
                    </th>
                  )}
                  {definicao.colunas?.map((col, i) => (
                    <th key={i}>{col.label}</th>
                  ))}
                  <th style={{ width: '60px' }} className="text-center">Ação</th>
                </tr>
              </thead>
              <tbody>
                {linhas.map((linha) => (
                  <tr key={linha.id}>
                    {definicao.selecao?.campo && (
                      <td className="text-center">
                        <input
                          type={definicao.selecao.singleSelecao ? 'radio' : 'checkbox'}
                          checked={Boolean(linha[definicao.selecao.campo])}
                          onChange={() => marcarComoSelecionada(linha.id)}
                          className="form-check-input"
                        />
                      </td>
                    )}
                    {definicao.colunas?.map((col, i) => (
                      <td key={i}>
                        {col.readonly ? (
                          <span>{linha[col.campo]}</span>
                        ) : (
                          renderCampo(col, linha[col.campo], (newVal) => {
                            const novasLinhas = linhas.map(l =>
                              l.id === linha.id ? { ...l, [col.campo]: newVal } : l
                            )
                            setLinhas(novasLinhas)
                          })
                        )}
                      </td>
                    ))}
                    <td className="text-center">
                      <button
                        type="button"
                        className="btn btn-sm btn-outline-danger"
                        onClick={() => removerLinha(linha.id)}
                        title="Remover"
                      >
                        <i className="bi bi-trash"></i>
                      </button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}

        {/* Formulário para adicionar nova linha */}
        {(!definicao.maxLinhas || linhas.length < definicao.maxLinhas) && (
          <div className="border-top pt-3">
            <div className="row g-2 align-items-end">
              {definicao.colunas?.map((col, i) => (
                <div key={i} className={`col-12 col-md-${col.col || 4}`}>
                  <label className="form-label small mb-1">{col.label}</label>
                  {renderCampo(col, novaLinha[col.campo], (newVal) => {
                    setNovaLinha({ ...novaLinha, [col.campo]: newVal })
                  })}
                </div>
              ))}
              <div className="col-12 col-md-2">
                <button
                  type="button"
                  className="btn btn-sm btn-success w-100"
                  onClick={adicionarLinha}
                >
                  <i className="bi bi-plus"></i> Adicionar
                </button>
              </div>
            </div>
          </div>
        )}

        {linhas.length >= (definicao.maxLinhas || Infinity) && (
          <div className="alert alert-warning small mb-0">
            Máximo de linhas atingido
          </div>
        )}
      </div>
    </div>
  )
}
