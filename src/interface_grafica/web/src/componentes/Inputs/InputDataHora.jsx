import React from 'react'

/**
 * InputDataHora - Componente para entrada de data e hora
 * 
 * Características:
 * - Tipo HTML5 datetime-local (input puro, funciona com FormData)
 * - Formato ISO 8601 (YYYY-MM-DDTHH:mm)
 * - Suporta value como string ISO ou Date
 * - Sempre envia em formato ISO
 */
export default function InputDataHora({
  name,
  value,
  disabled = false,
  required = false,
  error,
  placeholder = 'dd/mm/aaaa hh:mm'
}) {
  // Converter Date ou string para ISO datetime format
  const getIsoDateTime = () => {
    if (!value) return ''
    
    if (value instanceof Date) {
      // Remove Z e converte para formato local
      return value.toISOString().slice(0, 16)
    }
    
    if (typeof value === 'string') {
      // Se já está em formato ISO, retorna os primeiros 16 caracteres
      if (value.includes('T')) {
        return value.slice(0, 16)
      }
    }
    
    return ''
  }

  return (
    <div className="form-group">
      <input
        type="datetime-local"
        name={name}
        defaultValue={getIsoDateTime()}
        disabled={disabled}
        required={required}
        className={`form-control ${error ? 'is-invalid' : ''}`}
      />
      {error && <div className="invalid-feedback d-block">{error}</div>}
    </div>
  )
}
