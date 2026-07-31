import axios from 'axios'
import modalService from '../utils/modalService'

const api = axios.create({
  withCredentials: true
})

// Request interceptor: if config.block === true, trigger blocking overlay
api.interceptors.request.use(cfg => {
  if (cfg && cfg.block) modalService.bloquearTela()
  return cfg
}, err => Promise.reject(err))

// Response interceptor: handle envelope { erro, mensagem, redirecionarPara }
api.interceptors.response.use(resp => {
  if (resp.config && resp.config.block) modalService.desbloquearTela()
  const d = resp.data
  if (d && typeof d === 'object'){
    // envelope format expected: { erro, mensagem, data, items, total, page, pageSize, redirecionarPara }
    if (d.erro){
      modalService.alertModal(d.mensagem || 'Erro')
      // expose envelope on rejection so callers can inspect
      return Promise.reject({ envelope: d, response: resp })
    } else {
      if (d.mensagem) modalService.alertModal(d.mensagem)
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
  modalService.desbloquearTela()
  modalService.alertModal('Erro inesperado na requisição')
  return Promise.reject(err)
})

export default api
