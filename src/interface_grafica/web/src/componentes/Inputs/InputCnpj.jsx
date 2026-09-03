import React from 'react'

/**
 * InputCnpj - Componente para entrada de CNPJ
 * 
 * Características:
 * - Detecta automaticamente formato (numérico ou alphanumeric)
 * - Formata numérico: 14 dígitos → XX.XXX.XXX/XXXX-XX
 * - Formata alphanumeric: 12-16 caracteres → XXXXXXXXXXXXXXX (sem separadores)
 * - Valida enquanto digita
 * - Remove caracteres inválidos automaticamente
 */
export default function InputCnpj({
  name,
  value = '',
  onChange,
  disabled = false,
  required = false,
  error,
  placeholder = '00.000.000/0000-00 ou ABCD1234567890'
}) {
  /**
   * Formata CNPJ numérico (14 dígitos)
   * Formato: XX.XXX.XXX/XXXX-XX
   */
  const formatarCnpjNumerico = (valor) => {
    if (!valor) return ''
    const numeros = valor.replace(/\D/g, '')
    if (numeros.length <= 2) return numeros
    if (numeros.length <= 5) return `${numeros.slice(0, 2)}.${numeros.slice(2)}`
    if (numeros.length <= 8) return `${numeros.slice(0, 2)}.${numeros.slice(2, 5)}.${numeros.slice(5)}`
    if (numeros.length <= 12) return `${numeros.slice(0, 2)}.${numeros.slice(2, 5)}.${numeros.slice(5, 8)}/${numeros.slice(8)}`
    return `${numeros.slice(0, 2)}.${numeros.slice(2, 5)}.${numeros.slice(5, 8)}/${numeros.slice(8, 12)}-${numeros.slice(12, 14)}`
  }

  /**
   * Formata CNPJ alphanumeric (12-16 caracteres)
   * Retorna em uppercase, sem separadores
   */
  const formatarCnpjAlphanumeric = (valor) => {
    if (!valor) return ''
    return valor.replace(/[^A-Z0-9]/gi, '').toUpperCase().slice(0, 16)
  }

  /**
   * Detecta e formata o CNPJ conforme tipo
   */
  const formatarCnpj = (valor) => {
    if (!valor) return ''

    // Tenta detectar se é numérico
    const apenasNumeros = valor.replace(/\D/g, '')
    if (apenasNumeros.length > 0 && /^[0-9]+$/.test(valor.replace(/\D/g, ''))) {
      return formatarCnpjNumerico(valor)
    }

    // Caso contrário, trata como alphanumeric
    return formatarCnpjAlphanumeric(valor)
  }

  /**
   * Retorna apenas números/letras (sem separadores) para envio ao servidor
   */
  const getNormalizado = () => {
    return value.replace(/\D/g, '').slice(0, 14) || value.toUpperCase().replace(/[^A-Z0-9]/g, '')
  }

  const handleChange = (e) => {
    let inputValue = e.target.value

    // Formata conforme digita
    const formatted = formatarCnpj(inputValue)

    if (onChange) {
      onChange({
        target: {
          name,
          value: getNormalizado(), // Envia sem formatação (apenas algarismos)
          _formatted: formatted      // Valor formatado para exibição
        }
      })
    }
  }

  const handleBlur = (e) => {
    // Ao perder foco, garante que o valor está normalizado
    if (onChange) {
      onChange({
        target: {
          name,
          value: getNormalizado()
        }
      })
    }
  }

  return (
    <div className="form-group">
      <input
        type="text"
        name={name}
        value={formatarCnpj(value)}
        onChange={handleChange}
        onBlur={handleBlur}
        disabled={disabled}
        required={required}
        placeholder={placeholder}
        maxLength="18"
        className={`form-control ${error ? 'is-invalid' : ''}`}
      />
      {error && <div className="invalid-feedback d-block">{error}</div>}
      <small className="form-text text-muted">
        Aceita: 14 dígitos (XX.XXX.XXX/XXXX-XX) ou 12-16 caracteres alfanuméricos
      </small>
    </div>
  )
}
