import React from 'react'

/**
 * InputData - Componente para entrada de data
 * 
 * Características:
 * - Tipo HTML5 date (input puro, funciona com FormData)
 * - Formato ISO (YYYY-MM-DD)
 * - Suporta value como string ISO ou Date
 * - Sempre envia em formato ISO
 */
export default function InputData({
  name,
  value,
  disabled = false,
  required = false,
  error,
  placeholder = 'dd/mm/aaaa'
}) {
  // Converter Date para ISO string se necessário
  const getIsoValue = () => {
    if (!value) return ''
    if (value instanceof Date) {
      return value.toISOString().split('T')[0]
    }
    if (typeof value === 'string') {
      // Se já é ISO (YYYY-MM-DD), retorna como está
      if (/^\d{4}-\d{2}-\d{2}/.test(value)) {
        return value.split('T')[0]
      }
    }
    return ''
  }

  return (
    <div className="form-group">
      <input
        type="date"
        name={name}
        defaultValue={getIsoValue()}
        disabled={disabled}
        required={required}
        className={`form-control ${error ? 'is-invalid' : ''}`}
      />
      {error && <div className="invalid-feedback d-block">{error}</div>}
    </div>
  )
}
