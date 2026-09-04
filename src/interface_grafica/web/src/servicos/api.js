import axios from 'axios'
import modalServico from '../utils/modalServico'

const api = axios.create({
  withCredentials: true
})

// Request interceptor: if config.block === true, trigger blocking overlay
api.interceptors.request.use(cfg => {
  if (cfg && cfg.block) modalServico.bloquearTela()
  return cfg
}, err => Promise.reject(err))

// Response interceptor: handle envelope { erro, mensagem, redirecionarPara }
api.interceptors.response.use(resp => {
  if (resp.config && resp.config.block) modalServico.desbloquearTela()
  const d = resp.data
  if (d && typeof d === 'object'){
    // envelope format expected: { erro, mensagem, data, items, total, page, pageSize, redirecionarPara }
    if (d.erro){
      modalServico.modalAlerta(d.mensagem || 'Erro')
      // expose envelope on rejection so callers can inspect
      return Promise.reject({ envelope: d, response: resp })
    } else {
      if (d.mensagem) modalServico.modalAlerta(d.mensagem)
      if (d.redirecionarPara) window.location.href = d.redirecionarPara
      // unwrap inner payload for convenience:
      // - if the envelope explicitly contains a `data` property (even if null), use it
      // - else if it contains `items`, use that
      // - otherwise keep the original envelope object
      const hasData = Object.prototype.hasOwnProperty.call(d, 'data')
      const hasItems = Object.prototype.hasOwnProperty.call(d, 'items')
      const inner = hasData ? d.data : (hasItems ? d.items : d)
      resp.data = inner
      resp.envelope = d
    }
  }
  return resp
}, err => {
  modalServico.desbloquearTela()
  const status = err.response?.status

  // Sessão expirada – redireciona para login sem mostrar modal genérico
  if (status === 401) {
    if (!window.location.pathname.startsWith('/autenticacao')) {
      window.location.href = '/autenticacao'
    }
    return Promise.reject(err)
  }

  // Erros de validação (400) – o componente é responsável por exibir as mensagens inline
  if (status === 400) {
    return Promise.reject(err)
  }

  // Outros erros (403, 500, rede, etc.) – exibe mensagem genérica ou do envelope
  const mensagem = err.response?.data?.mensagem || 'Erro inesperado na requisição'
  modalServico.modalAlerta(mensagem)
  return Promise.reject(err)
})

export default api
