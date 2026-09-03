import React from 'react'

/**
 * InputDataHora - Componente para entrada de data e hora
 * 
 * Características:
 * - Tipo HTML5 datetime-local
 * - Formato ISO 8601 (YYYY-MM-DDTHH:mm)
 * - Suporta value como string ISO ou Date
 * - Converte para ISO antes de enviar
 */
export default function InputDataHora({
  name,
  value,
  onChange,
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

  const handleChange = (e) => {
    const isoDateTime = e.target.value // YYYY-MM-DDTHH:mm
    if (onChange) {
      onChange({
        target: {
          name,
          value: isoDateTime // Passa em formato ISO
        }
      })
    }
  }

  return (
    <div className="form-group">
      <input
        type="datetime-local"
        name={name}
        value={getIsoDateTime()}
        onChange={handleChange}
        disabled={disabled}
        required={required}
        className={`form-control ${error ? 'is-invalid' : ''}`}
      />
      {error && <div className="invalid-feedback d-block">{error}</div>}
    </div>
  )
}
