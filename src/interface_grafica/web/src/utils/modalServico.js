// Singleton simples para expor manipuladores de modal/bloqueio para código não React (por exemplo, interceptadores axios)
const gerenciadores = {
  alert: (msg) => console.info('alert:', msg),
  confirm: (msg, cb) => { const ok = window.confirm(msg); if (ok) cb?.() },
  block: () => {},
  unblock: () => {},
  loadHtml: (html) => {}
}

export function registrarGerenciadoresDeModais(h){
  if (h.alert) gerenciadores.alert = h.alert
  if (h.confirm) gerenciadores.confirm = h.confirm
  if (h.block) gerenciadores.block = h.block
  if (h.unblock) gerenciadores.unblock = h.unblock
  if (h.loadHtml) gerenciadores.loadHtml = h.loadHtml
  if (h.openComponent) gerenciadores.openComponent = h.openComponent
}

export function modalAlerta(msg){ gerenciadores.alert(msg) }
export function dialogoConfirmacao(msg, cb){ gerenciadores.confirm(msg, cb) }
export function bloquearTela(){ gerenciadores.block() }
export function desbloquearTela(){ gerenciadores.unblock() }
export function carregarHtmlNoModal(html){ gerenciadores.loadHtml(html) }
export function abrirComponenteNoModal(name, props){ gerenciadores.openComponent?.(name, props) }

export default {
  registrarGerenciadoresDeModais,
  modalAlerta,
  dialogoConfirmacao,
  bloquearTela,
  desbloquearTela,
  carregarHtmlNoModal,
  abrirComponenteNoModal
}
