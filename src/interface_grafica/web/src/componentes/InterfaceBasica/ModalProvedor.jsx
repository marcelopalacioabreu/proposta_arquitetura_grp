import React, { useState, useEffect, useCallback } from 'react'
import AlertModal from './ModalAlerta'
import ConfirmModal from './ModalConfirmacao'
import BloqueioTela from './BloqueioTela'
import modalService from '../../utils/modalServico'
import ReactDOM from 'react-dom'

export const ModalContext = React.createContext(null)

export default function ModalProvider({ children }){
  const [alertState, setAlertState] = useState({ show:false, message: null })
  const [confirmState, setConfirmState] = useState({ show:false, message:null, onConfirm: null })
  const [blocking, setBlocking] = useState(false)
  const [componentModal, setComponentModal] = useState({ show:false, Comp:null, props:null })

  const alert = useCallback((msg) => setAlertState({ show:true, message: msg }), [])
  const closeAlert = useCallback(()=> setAlertState({ show:false, message:null }), [])

  const confirm = useCallback((msg, cb) => setConfirmState({ show:true, message: msg, onConfirm: () => { cb?.(); setConfirmState({ show:false, message:null, onConfirm:null }) } }), [])
  const cancelConfirm = useCallback(()=> setConfirmState({ show:false, message:null, onConfirm:null }), [])

  const block = useCallback(()=> setBlocking(true), [])
  const unblock = useCallback(()=> setBlocking(false), [])

  useEffect(()=>{
    // register handlers used by non-react code
    modalService.registerModalHandlers({ alert, confirm: (m,cb)=> confirm(m,cb), block, unblock, loadHtml: (html)=> alert(html), openComponent: (name, p) => openComponent(name, p) })
    // expose simple globals for legacy scripts (blade/jQuery) to call
    try{
      window.bloquearTela = block
      window.desbloquearTela = unblock
      window.alertModal = alert
      window.confirmDialog = (msg, cb) => confirm(msg, cb)
      window.loadInModal = (htmlOrUrl) => { /* keep simple: if html contains '<', render, else fetch */
        if (typeof htmlOrUrl === 'string' && htmlOrUrl.includes('<')) alert(htmlOrUrl)
        else fetch(htmlOrUrl).then(r=>r.text()).then(html=> alert(html)).catch(()=> alert('Erro ao carregar'))
      }
      window.openComponentModal = (name, props) => openComponent(name, props)
    }catch{}
  },[alert, confirm, block, unblock])

  // open a React component inside a modal by name (lazy import)
  async function openComponent(name, props){
    setBlocking(true)
    try{
      let mod = null
      switch(name){
        case 'TelaCadastro':
          mod = await import('../Cadastros/TelaCadastro')
          break
        case 'TelaPesquisa':
          mod = await import('../Cadastros/TelaPesquisa')
          break
        default:
          // try dynamic import by path-ish name
          mod = await import(`./${name}`)
      }
      const Comp = mod.default
      setComponentModal({ show:true, Comp, props: props || {} })
    }catch(err){
      alert('Erro ao abrir componente em modal')
      console.error(err)
    }finally{
      setBlocking(false)
    }
  }

  function closeComponentModal(){ setComponentModal({ show:false, Comp:null, props:null }) }

  return (
    <ModalContext.Provider value={{ alert, confirm, block, unblock }}>
      {children}
      <AlertModal show={alertState.show} message={alertState.message} onClose={closeAlert} />
      <ConfirmModal show={confirmState.show} title={'Confirmação'} message={confirmState.message} onConfirm={confirmState.onConfirm} onCancel={cancelConfirm} />
      <BloqueioTela show={blocking} />
      {componentModal.show && componentModal.Comp && (
        <div className="modal show" style={{ display: 'block', backgroundColor: 'rgba(0,0,0,0.5)' }} tabIndex={-1}>
          <div className="modal-dialog modal-xl modal-fullscreen-sm-down">
            <div className="modal-content">
              {/* header with close button, similar to Alert/Confirm modals */}
              <div className="modal-header">
                <h5 className="modal-title">{componentModal.props?.title || ''}</h5>
                <button type="button" className="btn-close" onClick={closeComponentModal}></button>
              </div>
              <div className="modal-body">
                <componentModal.Comp {...componentModal.props} closeModal={closeComponentModal} />
              </div>
            </div>
          </div>
        </div>
      )}
    </ModalContext.Provider>
  )
}
