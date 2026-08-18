import React, { useEffect, useState } from 'react'
import BarraNavegacao from '../componentes/Layout/BarraNavegacao'
import Rodape from '../componentes/Layout/Rodape'
import api from '../servicos/api'

export default function CadastroUsuario(){
  const [formData, setFormData] = useState({
    nome: '',
    username: '',
    email: '',
    senhaHash: '',
    pessoa: {
      nome: '',
      tipoPessoaChave: 'F',
      documento: '',
      email: '',
      telefone: ''
    },
    setoresAtuacao: [],
    perfilIds: []
  })

  const [setoresDisponiveis, setSetoresDisponiveis] = useState([])
  const [unidadesDisponiveis, setUnidadesDisponiveis] = useState([])
  const [perfisDisponiveis, setPerfisDisponiveis] = useState([])
  const [pesquisaSetor, setPesquisaSetor] = useState('')
  const [pesquisaUnidade, setPesquisaUnidade] = useState('')
  const [novoSetor, setNovoSetor] = useState(null)
  const [novaUnidade, setNovaUnidade] = useState(null)
  const [errors, setErrors] = useState({})
  const [loading, setLoading] = useState(false)

  // Carregar dados iniciais
  useEffect(() => {
    Promise.all([
      api.get('/api/setores', { params: { pageSize: 1000 } }),
      api.get('/api/organizacao_unidades', { params: { pageSize: 1000 } }),
      api.get('/api/perfis', { params: { pageSize: 1000 } })
    ]).then(([resSetores, resUnidades, resPerfis]) => {
      setSetoresDisponiveis(resSetores.envelope?.items || [])
      setUnidadesDisponiveis(resUnidades.envelope?.items || [])
      setPerfisDisponiveis(resPerfis.envelope?.items || [])
    }).catch(e => console.error('Erro ao carregar dados:', e))
  }, [])

  const handleInputChange = (e) => {
    const { name, value } = e.target
    if (name.startsWith('pessoa.')) {
      const field = name.split('.')[1]
      setFormData(prev => ({
        ...prev,
        pessoa: { ...prev.pessoa, [field]: value }
      }))
    } else {
      setFormData(prev => ({ ...prev, [name]: value }))
    }
  }

  const handleAdicionarSetor = () => {
    if (!novoSetor) {
      alert('Selecione um setor')
      return
    }

    const setorExistente = formData.setoresAtuacao.find(s => s.setorId === novoSetor.id)
    if (setorExistente) {
      alert('Este setor já foi adicionado')
      return
    }

    const novaAtuacao = {
      setorId: novoSetor.id,
      setorNome: novoSetor.nome || novoSetor.Nome,
      unidadeId: novaUnidade?.id || null,
      unidadeNome: novaUnidade?.nome || novaUnidade?.Nome || '',
      padrao: false,
      habilitarPermissoesNegativas: false
    }

    setFormData(prev => ({
      ...prev,
      setoresAtuacao: [...prev.setoresAtuacao, novaAtuacao]
    }))

    setNovoSetor(null)
    setNovaUnidade(null)
    setPesquisaSetor('')
    setPesquisaUnidade('')
  }

  const handleRemoverSetor = (index) => {
    setFormData(prev => ({
      ...prev,
      setoresAtuacao: prev.setoresAtuacao.filter((_, i) => i !== index)
    }))
  }

  const handleAlternardaPadraoSetor = (index) => {
    setFormData(prev => ({
      ...prev,
      setoresAtuacao: prev.setoresAtuacao.map((s, i) => ({
        ...s,
        padrao: i === index
      }))
    }))
  }

  const handleTogglePermissoesNegativas = (index) => {
    setFormData(prev => ({
      ...prev,
      setoresAtuacao: prev.setoresAtuacao.map((s, i) =>
        i === index ? { ...s, habilitarPermissoesNegativas: !s.habilitarPermissoesNegativas } : s
      )
    }))
  }

  const handleTogglePerfil = (perfilId) => {
    setFormData(prev => ({
      ...prev,
      perfilIds: prev.perfilIds.includes(perfilId)
        ? prev.perfilIds.filter(id => id !== perfilId)
        : [...prev.perfilIds, perfilId]
    }))
  }

  const handleSubmit = async (e) => {
    e.preventDefault()
    setLoading(true)
    setErrors({})

    try {
      const payload = {
        nome: formData.nome,
        username: formData.username,
        email: formData.email,
        senhaHash: formData.senhaHash,
        pessoa: formData.pessoa,
        setorIds: formData.setoresAtuacao.map(s => s.setorId),
        padraoSetorId: formData.setoresAtuacao.find(s => s.padrao)?.setorId || null,
        perfilIds: formData.perfilIds
      }

      const response = await api.post('/api/usuarios', payload)
      if (response.status === 201 || response.status === 200) {
        alert('Usuário criado com sucesso!')
        setFormData({
          nome: '',
          username: '',
          email: '',
          senhaHash: '',
          pessoa: {
            nome: '',
            tipoPessoaChave: 'F',
            documento: '',
            email: '',
            telefone: ''
          },
          setoresAtuacao: [],
          perfilIds: []
        })
      }
    } catch (error) {
      const err = error.response?.data || error.message
      setErrors({ submit: err?.message || 'Erro ao criar usuário' })
      alert('Erro: ' + (err?.message || error.message))
    } finally {
      setLoading(false)
    }
  }

  const filteredSetores = setoresDisponiveis.filter(s =>
    (s.nome || s.Nome || '').toLowerCase().includes(pesquisaSetor.toLowerCase())
  )

  const filteredUnidades = unidadesDisponiveis.filter(u =>
    (u.nome || u.Nome || '').toLowerCase().includes(pesquisaUnidade.toLowerCase())
  )

  const tiposPessoa = [
    { chave: 'F', descricao: 'Física' },
    { chave: 'J', descricao: 'Jurídica' }
  ]

  return (
    <div className="d-flex flex-column min-vh-100">
      <BarraNavegacao brand="Retaguarda" />
      <div className="page-wrapper flex-fill">
        <div className="container my-4">
          <div className="card">
            <div className="card-header">
              <h4 className="mb-0">Cadastro de Usuário</h4>
            </div>
            <div className="card-body">
              {errors.submit && <div className="alert alert-danger">{errors.submit}</div>}

              <form onSubmit={handleSubmit}>
                {/* Seção: Dados do Usuário */}
                <fieldset className="mb-4 pb-3 border-bottom">
                  <legend className="text-lg mb-3">Dados do Usuário</legend>
                  <div className="row g-3">
                    <div className="col-md-6">
                      <label className="form-label">Nome <span className="text-danger">*</span></label>
                      <input
                        type="text"
                        className="form-control"
                        name="nome"
                        value={formData.nome}
                        onChange={handleInputChange}
                        required
                      />
                    </div>
                    <div className="col-md-6">
                      <label className="form-label">Usuário <span className="text-danger">*</span></label>
                      <input
                        type="text"
                        className="form-control"
                        name="username"
                        value={formData.username}
                        onChange={handleInputChange}
                        required
                      />
                    </div>
                    <div className="col-md-6">
                      <label className="form-label">Email</label>
                      <input
                        type="email"
                        className="form-control"
                        name="email"
                        value={formData.email}
                        onChange={handleInputChange}
                      />
                    </div>
                    <div className="col-md-6">
                      <label className="form-label">Senha <span className="text-danger">*</span></label>
                      <input
                        type="password"
                        className="form-control"
                        name="senhaHash"
                        value={formData.senhaHash}
                        onChange={handleInputChange}
                        required
                      />
                    </div>
                  </div>
                </fieldset>

                {/* Seção: Dados da Pessoa */}
                <fieldset className="mb-4 pb-3 border-bottom">
                  <legend className="text-lg mb-3">Dados da Pessoa</legend>
                  <div className="row g-3">
                    <div className="col-md-4">
                      <label className="form-label">Tipo de Pessoa</label>
                      <select
                        className="form-select"
                        name="pessoa.tipoPessoaChave"
                        value={formData.pessoa.tipoPessoaChave}
                        onChange={handleInputChange}
                      >
                        {tiposPessoa.map(tipo => (
                          <option key={tipo.chave} value={tipo.chave}>
                            {tipo.descricao}
                          </option>
                        ))}
                      </select>
                    </div>
                    <div className="col-md-8">
                      <label className="form-label">
                        {formData.pessoa.tipoPessoaChave === 'F' ? 'Nome' : 'Razão Social'}
                      </label>
                      <input
                        type="text"
                        className="form-control"
                        name="pessoa.nome"
                        value={formData.pessoa.nome}
                        onChange={handleInputChange}
                      />
                    </div>
                    <div className="col-md-6">
                      <label className="form-label">
                        {formData.pessoa.tipoPessoaChave === 'F' ? 'CPF' : 'CNPJ'}
                      </label>
                      <input
                        type="text"
                        className="form-control"
                        name="pessoa.documento"
                        value={formData.pessoa.documento}
                        onChange={handleInputChange}
                      />
                    </div>
                    <div className="col-md-6">
                      <label className="form-label">Telefone</label>
                      <input
                        type="tel"
                        className="form-control"
                        name="pessoa.telefone"
                        value={formData.pessoa.telefone}
                        onChange={handleInputChange}
                      />
                    </div>
                    <div className="col-md-12">
                      <label className="form-label">Email</label>
                      <input
                        type="email"
                        className="form-control"
                        name="pessoa.email"
                        value={formData.pessoa.email}
                        onChange={handleInputChange}
                      />
                    </div>
                  </div>
                </fieldset>

                {/* Seção: Setores de Atuação */}
                <fieldset className="mb-4 pb-3 border-bottom">
                  <legend className="text-lg mb-3">Setores e Unidades de Atuação</legend>

                  <div className="row g-3 mb-3">
                    <div className="col-md-5">
                      <label className="form-label">Setor</label>
                      <div className="input-group">
                        <input
                          type="text"
                          className="form-control"
                          placeholder="Digite para pesquisar..."
                          value={pesquisaSetor}
                          onChange={(e) => setPesquisaSetor(e.target.value)}
                        />
                        <div className="list-group position-absolute w-100" style={{ top: '100%', zIndex: 1000 }}>
                          {filteredSetores.slice(0, 10).map(setor => (
                            <button
                              key={setor.id}
                              type="button"
                              className="list-group-item list-group-item-action"
                              onClick={() => {
                                setNovoSetor(setor)
                                setPesquisaSetor(setor.nome || setor.Nome)
                              }}
                            >
                              {setor.nome || setor.Nome}
                            </button>
                          ))}
                        </div>
                      </div>
                    </div>

                    <div className="col-md-5">
                      <label className="form-label">Unidade (Opcional)</label>
                      <div className="input-group">
                        <input
                          type="text"
                          className="form-control"
                          placeholder="Digite para pesquisar..."
                          value={pesquisaUnidade}
                          onChange={(e) => setPesquisaUnidade(e.target.value)}
                          disabled={!novoSetor}
                        />
                        <div className="list-group position-absolute w-100" style={{ top: '100%', zIndex: 1000 }}>
                          {filteredUnidades.slice(0, 10).map(unidade => (
                            <button
                              key={unidade.id}
                              type="button"
                              className="list-group-item list-group-item-action"
                              onClick={() => {
                                setNovaUnidade(unidade)
                                setPesquisaUnidade(unidade.nome || unidade.Nome)
                              }}
                            >
                              {unidade.nome || unidade.Nome}
                            </button>
                          ))}
                        </div>
                      </div>
                    </div>

                    <div className="col-md-2 d-flex align-items-end">
                      <button
                        type="button"
                        className="btn btn-primary w-100"
                        onClick={handleAdicionarSetor}
                        disabled={!novoSetor}
                      >
                        Adicionar
                      </button>
                    </div>
                  </div>

                  {/* Subtabela de Atuações */}
                  {formData.setoresAtuacao.length > 0 && (
                    <div className="table-responsive">
                      <table className="table table-sm table-hover">
                        <thead className="table-light">
                          <tr>
                            <th>Setor</th>
                            <th>Unidade</th>
                            <th style={{ width: 'auto' }}>Padrão</th>
                            <th style={{ width: 'auto' }}>Perm. Negativas</th>
                            <th style={{ width: '50px' }}>Ações</th>
                          </tr>
                        </thead>
                        <tbody>
                          {formData.setoresAtuacao.map((setor, index) => (
                            <tr key={index}>
                              <td>{setor.setorNome}</td>
                              <td>{setor.unidadeNome || '—'}</td>
                              <td>
                                <div className="form-check">
                                  <input
                                    className="form-check-input"
                                    type="radio"
                                    name="setorPadrao"
                                    id={`padrao-${index}`}
                                    checked={setor.padrao}
                                    onChange={() => handleAlternardaPadraoSetor(index)}
                                  />
                                </div>
                              </td>
                              <td>
                                <div className="form-check">
                                  <input
                                    className="form-check-input"
                                    type="checkbox"
                                    id={`perm-neg-${index}`}
                                    checked={setor.habilitarPermissoesNegativas}
                                    onChange={() => handleTogglePermissoesNegativas(index)}
                                  />
                                </div>
                              </td>
                              <td>
                                <button
                                  type="button"
                                  className="btn btn-sm btn-danger"
                                  onClick={() => handleRemoverSetor(index)}
                                >
                                  ✕
                                </button>
                              </td>
                            </tr>
                          ))}
                        </tbody>
                      </table>
                    </div>
                  )}
                </fieldset>

                {/* Seção: Perfis */}
                <fieldset className="mb-4">
                  <legend className="text-lg mb-3">Perfis de Acesso</legend>
                  <div className="row g-2">
                    {perfisDisponiveis.map(perfil => (
                      <div key={perfil.id} className="col-md-4">
                        <div className="form-check">
                          <input
                            className="form-check-input"
                            type="checkbox"
                            id={`perfil-${perfil.id}`}
                            checked={formData.perfilIds.includes(perfil.id)}
                            onChange={() => handleTogglePerfil(perfil.id)}
                          />
                          <label className="form-check-label" htmlFor={`perfil-${perfil.id}`}>
                            {perfil.nome || perfil.Nome}
                          </label>
                        </div>
                      </div>
                    ))}
                  </div>
                </fieldset>

                {/* Botões de Ação */}
                <div className="d-flex justify-content-end gap-2">
                  <button type="reset" className="btn btn-secondary">
                    Limpar
                  </button>
                  <button type="submit" className="btn btn-primary" disabled={loading}>
                    {loading ? 'Salvando...' : 'Criar Usuário'}
                  </button>
                </div>
              </form>
            </div>
          </div>
        </div>
      </div>
      <Rodape />
    </div>
  )
}
