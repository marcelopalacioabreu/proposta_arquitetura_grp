import React from 'react'

/**
 * InputData - Componente para entrada de data
 * 
 * Características:
 * - Tipo HTML5 date
 * - Formato ISO (YYYY-MM-DD)
 * - Suporta value como string ISO ou Date
 * - Converte para ISO antes de enviar
 */
export default function InputData({
  name,
  value,
  onChange,
  disabled = false,
  required = false,
  error,
  placeholder = 'dd/mm/aaaa'
}) {
  // Converter Date para ISO string se necessário
  const isoValue = value ? (value instanceof Date ? value.toISOString().split('T')[0] : value) : ''

  const handleChange = (e) => {
    const isoDate = e.target.value // Já em formato ISO YYYY-MM-DD
    if (onChange) {
      onChange({
        target: {
          name,
          value: isoDate // Passa em formato ISO
        }
      })
    }
  }

  return (
    <div className="form-group">
      <input
        type="date"
        name={name}
        value={isoValue}
        onChange={handleChange}
        disabled={disabled}
        required={required}
        className={`form-control ${error ? 'is-invalid' : ''}`}
      />
      {error && <div className="invalid-feedback d-block">{error}</div>}
    </div>
  )
}
