import React, { useEffect, useRef } from 'react'

/**
 * InputCnpj - Componente para entrada de CNPJ
 * 
 * Características:
 * - Funciona com FormData (input HTML puro, não controlado)
 * - Detecta automaticamente formato (numérico ou alphanumeric)
 * - Formata numérico: 14 dígitos → XX.XXX.XXX/XXXX-XX (apenas display)
 * - Formata alphanumeric: 12-16 caracteres → XXXXXXXXXXXXXXX (sem separadores)
 * - Envia sempre normalizado (sem formatação)
 * - Remove caracteres inválidos automaticamente
 */
export default function InputCnpj({
  name,
  value = '',
  disabled = false,
  required = false,
  error,
  placeholder = '00.000.000/0000-00 ou ABCD1234567890'
}) {
  const inputRef = useRef(null)
  const formRef = useRef(null)

  /**
   * Formata CNPJ numérico (14 dígitos)
   * Formato: XX.XXX.XXX/XXXX-XX
   */
  const formatarCnpjNumerico = (valor) => {
    if (!valor) return ''
    const numeros = valor.replace(/\D/g, '').slice(0, 14)
    if (numeros.length === 0) return ''
    if (numeros.length <= 2) return numeros
    if (numeros.length <= 5) return `${numeros.slice(0, 2)}.${numeros.slice(2)}`
    if (numeros.length <= 8) return `${numeros.slice(0, 2)}.${numeros.slice(2, 5)}.${numeros.slice(5)}`
    if (numeros.length <= 12) return `${numeros.slice(0, 2)}.${numeros.slice(2, 5)}.${numeros.slice(5, 8)}/${numeros.slice(8)}`
    return `${numeros.slice(0, 2)}.${numeros.slice(2, 5)}.${numeros.slice(5, 8)}/${numeros.slice(8, 12)}-${numeros.slice(12, 14)}`
  }

  /**
   * Formata CNPJ alphanumeric (12-16 caracteres)
   * Apenas uppercase, sem separadores
   */
  const formatarCnpjAlphanumeric = (valor) => {
    if (!valor) return ''
    return valor.replace(/[^A-Z0-9]/gi, '').toUpperCase().slice(0, 16)
  }

  /**
   * Detecta e formata o CNPJ conforme tipo (apenas para exibição)
   */
  const formatarParaExibicao = (valor) => {
    if (!valor) return ''

    // Detecta se é numérico (contém principalmente dígitos)
    const apenasNumeros = valor.replace(/\D/g, '')
    if (apenasNumeros.length > 0) {
      return formatarCnpjNumerico(valor)
    }

    // Caso contrário, trata como alphanumeric
    return formatarCnpjAlphanumeric(valor)
  }

  /**
   * Retorna apenas números/letras (sem separadores) para envio ao servidor
   */
  const getNormalizado = (displayValue) => {
    if (!displayValue) return ''
    
    // Se for numérico, remove tudo exceto números e pega 14 dígitos
    const apenasNumeros = displayValue.replace(/\D/g, '')
    if (apenasNumeros.length > 0 && apenasNumeros.length <= 14) {
      return apenasNumeros
    }
    if (apenasNumeros.length > 0) {
      return apenasNumeros.slice(0, 14)
    }
    
    // Senão, trata como alphanumeric
    return displayValue.toUpperCase().replace(/[^A-Z0-9]/g, '')
  }

  const handleInput = (e) => {
    const displayValue = e.target.value
    const formatted = formatarParaExibicao(displayValue)
    
    // Atualiza o display do input
    e.target.value = formatted
  }

  // Quando o formulário é submetido, converter para valor normalizado
  useEffect(() => {
    const input = inputRef.current
    if (!input) return

    const form = input.closest('form')
    if (form) {
      formRef.current = form
      
      // Interceptar o evento submit para normalizar o valor
      const handleFormSubmit = () => {
        const normalizado = getNormalizado(input.value)
        input.value = normalizado
      }

      form.addEventListener('submit', handleFormSubmit)
      return () => {
        form.removeEventListener('submit', handleFormSubmit)
      }
    }
  }, [])

  // Inicializar o valor formatado
  const initialFormatted = formatarParaExibicao(value)

  return (
    <div className="form-group">
      <input
        ref={inputRef}
        type="text"
        name={name}
        defaultValue={initialFormatted}
        onInput={handleInput}
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

