import React, { useState, useEffect, useCallback } from 'react'
import ModalAlerta from './ModalAlerta'
import ModalConfirmacao from './ModalConfirmacao'
import BloqueioTela from './BloqueioTela'
import modalServico from '../../utils/modalServico'
import ReactDOM from 'react-dom'

export const ModalContext = React.createContext(null)

export default function ModalProvedor({ children }){
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
    // registra os gerenciadores de modais usados por código não-React
    modalServico.registrarGerenciadoresDeModais ({ alert, confirm: (m,cb)=> confirm(m,cb), block, unblock, loadHtml: (html)=> alert(html), openComponent: (name, p) => openComponent(name, p) })
    // Expondo funções globais para scripts legados (blade/jQuery) chamarem
    try{
      window.bloquearTela = block
      window.desbloquearTela = unblock
      window.modalAlerta = alert
      window.dialogoConfirmacao = (msg, cb) => confirm(msg, cb)
      window.carregarHtmlNoModal = (htmlOrUrl) => { /* Simplicidade: se html contiver '<', renderizar, caso contrário buscar */
        if (typeof htmlOrUrl === 'string' && htmlOrUrl.includes('<')) alert(htmlOrUrl)
        else fetch(htmlOrUrl).then(r=>r.text()).then(html=> alert(html)).catch(()=> alert('Erro ao carregar'))
      }
      window.abrirComponenteNoModal = (name, props) => openComponent(name, props)
    }catch{}
  },[alert, confirm, block, unblock])

  // Abrir um componente React dentro de um modal pelo nome (importação sob demanda)
  async function openComponent(name, props){
    setBlocking(true)
    try{
      let mod = null
      switch(name){
        case 'TelaCadastro':
          mod = await import('../Cadastros/TelaCadastro')
          break
        case 'TelaPesquisaDetalhesLinhaTelaPequena':
          mod = await import('../Cadastros/TelaPesquisaDetalhesLinhaTelaPequena')
          break
        case 'TelaPesquisa':
          mod = await import('../Cadastros/TelaPesquisa')
          break
        default:
            // Tenta importação dinâmica pelo caminho fornecido (ex: '../Cadastros/Componente')
            try{
              // Allow dynamic runtime path; bundlers may need the comment to skip static analysis
              // Try as provided
              // eslint-disable-next-line no-useless-concat
              mod = await import(/* @vite-ignore */ (name))
            }catch(e){
              // fallback: try adding common extensions
              try{ mod = await import(/* @vite-ignore */ (name + '.jsx')) }catch(_){
                try{ mod = await import(/* @vite-ignore */ (name + '.js')) }catch(__){
                  // fallback: try relative to this folder
                  try{ mod = await import(/* @vite-ignore */ (`./${name}`)) }catch(___){
                    try{ mod = await import(/* @vite-ignore */ (`./${name}.jsx`)) }catch(____){
                      try{ mod = await import(/* @vite-ignore */ (`./${name}.js`)) }catch(_____){ mod = null }
                    }
                  }
                }
              }
            }
      }
        if (!mod) throw new Error('Componente não encontrado: ' + name)
        const Comp = mod.default || mod
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
      <ModalAlerta show={alertState.show} message={alertState.message} onClose={closeAlert} />
      <ModalConfirmacao show={confirmState.show} title={'Confirmação'} message={confirmState.message} onConfirm={confirmState.onConfirm} onCancel={cancelConfirm} />
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
