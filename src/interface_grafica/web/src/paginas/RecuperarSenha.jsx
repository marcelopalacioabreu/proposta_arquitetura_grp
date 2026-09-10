import React, { useState } from 'react'
import { useNavigate, useSearchParams } from 'react-router-dom'
import api from '../servicos/api'
import modalServico from '../utils/modalServico'
import BarraNavegacao from '../componentes/Layout/BarraNavegacao'
import Rodape from '../componentes/Layout/Rodape'

export default function RecuperarSenha() {
  const [email, setEmail] = useState('')
  const [token, setToken] = useState('')
  const [novaSenha, setNovaSenha] = useState('')
  const [confirmacaoSenha, setConfirmacaoSenha] = useState('')
  const [etapa, setEtapa] = useState(1) // 1: solicitação, 2: redefinição
  const [loading, setLoading] = useState(false)
  const nav = useNavigate()
  const [searchParams] = useSearchParams()

  // Se vem token na URL, ir direto para etapa 2
  React.useEffect(() => {
    const tokenUrl = searchParams.get('token')
    if (tokenUrl) {
      setToken(tokenUrl)
      setEtapa(2)
    }
  }, [searchParams])

  async function solicitarRecuperacao(e) {
    e.preventDefault()
    if (!email) {
      modalServico.modalAlerta('Por favor, informe o e-mail')
      return
    }

    setLoading(true)
    try {
      const res = await api.post('/api/recuperacao-senha/solicitar', { email })
      modalServico.modalSucesso('E-mail de recuperação enviado com sucesso!')
      setEmail('')
    } catch (err) {
      modalServico.modalAlerta(err.response?.data?.mensagem || 'Erro ao solicitar recuperação')
    } finally {
      setLoading(false)
    }
  }

  async function validarToken(e) {
    e.preventDefault()
    if (!token) {
      modalServico.modalAlerta('Token não fornecido')
      return
    }

    setLoading(true)
    try {
      await api.post('/api/recuperacao-senha/validar-token', { token })
      setEtapa(2)
    } catch (err) {
      modalServico.modalAlerta('Token inválido ou expirado')
    } finally {
      setLoading(false)
    }
  }

  async function redefinirSenha(e) {
    e.preventDefault()

    if (!novaSenha || !confirmacaoSenha) {
      modalServico.modalAlerta('Por favor, preencha todos os campos')
      return
    }

    if (novaSenha !== confirmacaoSenha) {
      modalServico.modalAlerta('As senhas não coincidem')
      return
    }

    if (novaSenha.length < 6) {
      modalServico.modalAlerta('A senha deve ter no mínimo 6 caracteres')
      return
    }

    setLoading(true)
    try {
      await api.post('/api/recuperacao-senha/redefinir', {
        token,
        novaSenha,
        confirmacaoSenha
      })
      modalServico.modalSucesso('Senha redefinida com sucesso! Faça login com sua nova senha.')
      setTimeout(() => nav('/'), 2000)
    } catch (err) {
      modalServico.modalAlerta(err.response?.data?.mensagem || 'Erro ao redefinir senha')
    } finally {
      setLoading(false)
    }
  }

  return (
    <div className="d-flex flex-column min-vh-100">
      <BarraNavegacao brand="Retaguarda" />
      <div className="page-wrapper flex-fill d-flex align-items-center justify-content-center">
        <div className="page-card" style={{ maxWidth: 420 }}>
          {etapa === 1 ? (
            <>
              <h3 className="mb-3">Recuperar Senha</h3>
              <p className="text-muted small mb-4">
                Informe o e-mail associado à sua conta. Você receberá um link para redefinir sua senha.
              </p>
              <form onSubmit={solicitarRecuperacao}>
                <div className="mb-3">
                  <label className="form-label">E-mail</label>
                  <input
                    className="form-control"
                    type="email"
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                    placeholder="seu.email@exemplo.com"
                    disabled={loading}
                  />
                </div>
                <div className="d-flex gap-2">
                  <button
                    className="btn btn-primary flex-grow-1"
                    type="submit"
                    disabled={loading}
                  >
                    {loading ? 'Enviando...' : 'Enviar Link'}
                  </button>
                  <a href="/" className="btn btn-secondary">
                    Voltar
                  </a>
                </div>
              </form>
            </>
          ) : (
            <>
              <h3 className="mb-3">Redefinir Senha</h3>
              <p className="text-muted small mb-4">
                Informe sua nova senha nos campos abaixo.
              </p>
              <form onSubmit={redefinirSenha}>
                <div className="mb-3">
                  <label className="form-label">Nova Senha</label>
                  <input
                    className="form-control"
                    type="password"
                    value={novaSenha}
                    onChange={(e) => setNovaSenha(e.target.value)}
                    placeholder="Nova senha"
                    disabled={loading}
                  />
                </div>
                <div className="mb-3">
                  <label className="form-label">Confirmar Senha</label>
                  <input
                    className="form-control"
                    type="password"
                    value={confirmacaoSenha}
                    onChange={(e) => setConfirmacaoSenha(e.target.value)}
                    placeholder="Confirme a senha"
                    disabled={loading}
                  />
                </div>
                <div className="d-flex gap-2">
                  <button
                    className="btn btn-primary flex-grow-1"
                    type="submit"
                    disabled={loading}
                  >
                    {loading ? 'Processando...' : 'Redefinir Senha'}
                  </button>
                  <a href="/" className="btn btn-secondary">
                    Voltar
                  </a>
                </div>
              </form>
            </>
          )}
        </div>
      </div>
      <Rodape />
    </div>
  )
}
