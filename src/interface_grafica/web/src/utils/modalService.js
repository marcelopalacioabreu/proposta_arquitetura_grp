// Simple singleton to expose modal/blocking handlers to non-React code (e.g. axios interceptors)
const handlers = {
  alert: (msg) => console.info('alert:', msg),
  confirm: (msg, cb) => { const ok = window.confirm(msg); if (ok) cb?.() },
  block: () => {},
  unblock: () => {},
  loadHtml: (html) => {}
}

export function registerModalHandlers(h){
  if (h.alert) handlers.alert = h.alert
  if (h.confirm) handlers.confirm = h.confirm
  if (h.block) handlers.block = h.block
  if (h.unblock) handlers.unblock = h.unblock
  if (h.loadHtml) handlers.loadHtml = h.loadHtml
  if (h.openComponent) handlers.openComponent = h.openComponent
}

export function alertModal(msg){ handlers.alert(msg) }
export function confirmDialog(msg, cb){ handlers.confirm(msg, cb) }
export function bloquearTela(){ handlers.block() }
export function desbloquearTela(){ handlers.unblock() }
export function loadInModalHtml(html){ handlers.loadHtml(html) }
export function openComponentModal(name, props){ handlers.openComponent?.(name, props) }

export default {
  registerModalHandlers,
  alertModal,
  confirmDialog,
  bloquearTela,
  desbloquearTela,
  loadInModalHtml
  , openComponentModal
}
