import React from 'react'

export default function AlertModal({ show, title = 'Aviso', message, onClose }){
  if (!show) return null
  return (
    <div className="modal show" style={{ display: 'block', backgroundColor: 'rgba(0,0,0,0.5)' }} tabIndex={-1}>
      <div className="modal-dialog modal-dialog-centered">
        <div className="modal-content">
          <div className="modal-header bg-info text-white">
            <h5 className="modal-title">{title}</h5>
            <button type="button" className="btn-close" onClick={onClose}></button>
          </div>
          <div className="modal-body">
            <div dangerouslySetInnerHTML={{__html: message || ''}} />
          </div>
          <div className="modal-footer">
            <button type="button" className="btn btn-info" onClick={onClose}>OK</button>
          </div>
        </div>
      </div>
    </div>
  )
}
