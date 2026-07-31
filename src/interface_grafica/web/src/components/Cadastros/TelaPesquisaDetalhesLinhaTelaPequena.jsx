import React from 'react'
import { useNavigate } from 'react-router-dom'
import api from '../../services/api'
import modalService from '../../utils/modalService'

export default function TelaPesquisaDetalhesLinhaTelaPequena({ title = 'Detalhes', item = {}, columns = [], actions = [], closeModal }){
  const navigate = useNavigate()

  const handleAction = async (a) => {
    if (a.tipo === 'navegacao'){
      const destino = (a.destino || '').replace('{id}', item[a.campo_id] || item.id)
      navigate(destino)
      if (typeof closeModal === 'function') closeModal()
      return
    }
    if (a.tipo === 'confirmacao_delete_ajax'){
      modalService.confirmDialog(a.mensagem || 'Confirma exclusão?', async () => {
        try{
          const destino = (a.destino || '').replace('{id}', item[a.campo_id] || item.id)
          await api.delete(destino, { block: true })
          if (typeof closeModal === 'function') closeModal()
          // reload to refresh list
          window.location.reload()
        }catch(err){
          modalService.alertModal('Erro ao excluir')
        }
      })
    }
  }

  return (
    <div>
      <div className="row">
        {columns.map((c, idx) => {
          const field = typeof c === 'string' ? c : c.campo
          const label = typeof c === 'string' ? c : (c.titulo || c.label || c.campo)
          return (
            <div key={idx} className="col-12 mb-2">
              <div className="small text-muted">{label}:</div>
              <div>{String(item[field] ?? '')}</div>
            </div>
          )
        })}
      </div>

      {actions && actions.length > 0 && (
        <div className="mt-3 border-top pt-3 d-flex gap-2">
          {actions.map((a, ai) => (
            <button key={ai} className={`btn btn-sm ${a.tipo === 'confirmacao_delete_ajax' ? 'btn-danger' : 'btn-outline-secondary'}`} onClick={()=> handleAction(a)} title={a.titulo || a.icone || ''}>
              {a.icone ? <i className={`bi bi-${a.icone}`} /> : (a.titulo || 'Ação')}
            </button>
          ))}
        </div>
      )}
    </div>
  )
}
