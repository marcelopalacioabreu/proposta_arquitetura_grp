using System.Text;
using Retaguarda.Api.Models;
using Retaguarda.Api.Services.Interfaces;
using Retaguarda.Api.Utils;
using Retaguarda.DTO.Dtos;
using Retaguarda.DTO.Exceptions;
using Retaguarda.Dominio.Entidades;
using Retaguarda.Repositorios;
using Retaguarda.Repositorios.Interfaces;
using Retaguarda.Persistencia;
using Retaguarda.Servicos.Util;

namespace Retaguarda.Api.Services
{
    /// <summary>
    /// Serviço de recuperação de senha
    /// Orquestra a lógica de criação de tokens, validação e redefinição de senhas
    /// </summary>
    public class RecuperacaoSenhaServico : IRecuperacaoSenhaServico
    {
        private readonly IRecuperacaoSenhaRepositorio _repositorio;
        private readonly IApplicationDbContext _dbContext;
        private readonly IEmailServico _emailServico;
        private readonly EmailConfiguracao _emailConfig;
        private readonly ILogger<RecuperacaoSenhaServico> _logger;

        public RecuperacaoSenhaServico(
            IRecuperacaoSenhaRepositorio repositorio,
            IApplicationDbContext dbContext,
            IEmailServico emailServico,
            IConfiguration configuration,
            ILogger<RecuperacaoSenhaServico> logger)
        {
            _repositorio = repositorio;
            _dbContext = dbContext;
            _emailServico = emailServico;
            _logger = logger;

            // Lê configuração de email
            _emailConfig = new EmailConfiguracao();
            configuration.GetSection("Email").Bind(_emailConfig);
        }

        /// <summary>
        /// Solicita recuperação de senha para um usuário
        /// </summary>
        public async Task<RecuperacaoSenhaResponseDto> SolicitarRecuperacaoAsync(
            string email, string? ipCliente = null, string? userAgent = null)
        {
            // Validação básica
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ValidationException(
                    "Email é obrigatório",
                    new Dictionary<string, string[]> { { "email", new[] { "Email não pode estar vazio" } } }
                );
            }

            // Busca o usuário por email
            var usuario = await Task.Run(() =>
                _dbContext.Usuarios
                    .FirstOrDefault(u => u.Email == email && u.Ativo)
            );

            // Por segurança, sempre retorna mensagem genérica mesmo que usuário não exista
            if (usuario == null)
            {
                _logger.LogWarning($"Tentativa de recuperação para email inexistente: {email}");
                return new RecuperacaoSenhaResponseDto
                {
                    Mensagem = "Se este email existir em nosso sistema, um link de recuperação será enviado",
                    EmailEnviado = MascararEmail(email),
                    TempoExpiracaoMinutos = _emailConfig.RecuperacaoSenhaExpiracaoMinutos
                };
            }

            try
            {
                // Gera um token único
                var tokenOriginal = Guid.NewGuid().ToString();
                var tokenHash = PasswordHasher.Hash(tokenOriginal);

                // Cria registro de recuperação
                var recuperacao = new RecuperacaoSenha
                {
                    UsuarioId = usuario.Id,
                    OrganizacaoId = usuario.OrganizacaoId,
                    OrganizacaoUnidadeId = usuario.OrganizacaoUnidadeId,
                    SetorId = usuario.SetorId,
                    Token = tokenHash,
                    DataExpiracao = DateTime.UtcNow.AddMinutes(_emailConfig.RecuperacaoSenhaExpiracaoMinutos),
                    IpSolicitacao = ipCliente,
                    UserAgent = userAgent,
                    Ativo = true
                };

                await _repositorio.AdicionarAsync(recuperacao);

                // Monta a URL de recuperação
                var linkRecuperacao = $"{_emailConfig.UrlAplicacaoWeb}/recuperar-senha?token={tokenOriginal}";

                // Prepara placeholders para o template
                var placeholders = new Dictionary<string, string>
                {
                    { "nomeUsuario", usuario.Nome },
                    { "linkRecuperacao", linkRecuperacao },
                    { "tempoExpiracao", _emailConfig.RecuperacaoSenhaExpiracaoMinutos.ToString() },
                    { "urlAplicacao", _emailConfig.UrlAplicacaoWeb },
                    { "ano", DateTime.UtcNow.Year.ToString() }
                };

                // Envia email
                var emailEnviado = await _emailServico.EnviarEmailComTemplateAsync(
                    usuario.Email ?? string.Empty,
                    usuario.Nome,
                    "Recuperação de Senha - GRP",
                    "recuperacao-senha",
                    placeholders
                );

                if (!emailEnviado)
                {
                    _logger.LogError($"Falha ao enviar email de recuperação para {usuario.Email}");
                    throw new Exception("Falha ao enviar email de recuperação. Tente novamente mais tarde.");
                }

                _logger.LogInformation($"Email de recuperação enviado com sucesso para {usuario.Email}");

                return new RecuperacaoSenhaResponseDto
                {
                    Mensagem = "Se este email existir em nosso sistema, um link de recuperação será enviado",
                    EmailEnviado = MascararEmail(usuario.Email ?? string.Empty),
                    TempoExpiracaoMinutos = _emailConfig.RecuperacaoSenhaExpiracaoMinutos
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao solicitar recuperação de senha: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Valida se um token de recuperação é válido
        /// </summary>
        public async Task<bool> ValidarTokenAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return false;
            }

            try
            {
                // Busca todos os tokens válidos
                var recuperacoes = await _repositorio.ObterTodosValidosAsync();

                // Verifica se algum token corresponde (comparação de hash)
                foreach (var recuperacao in recuperacoes)
                {
                    if (PasswordHasher.Verify(token, recuperacao.Token ?? ""))
                    {
                        return true;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao validar token: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Redefine a senha do usuário usando um token válido
        /// </summary>
        public async Task<RedefinicaoSenhaResponseDto> RedefinirSenhaAsync(string token, string novaSenha)
        {
            // Validações básicas
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ValidationException(
                    "Token inválido",
                    new Dictionary<string, string[]> { { "token", new[] { "Token não pode estar vazio" } } }
                );
            }

            if (string.IsNullOrWhiteSpace(novaSenha) || novaSenha.Length < 6)
            {
                throw new ValidationException(
                    "Senha inválida",
                    new Dictionary<string, string[]> { 
                        { "novaSenha", new[] { "Senha deve ter pelo menos 6 caracteres" } } 
                    }
                );
            }

            try
            {
                // Busca todos os tokens válidos com seus usuários
                var recuperacoes = await _repositorio.ObterTodosValidosAsync();

                RecuperacaoSenha? recuperacaoValida = null;

                // Encontra o token correspondente
                foreach (var recuperacao in recuperacoes)
                {
                    if (PasswordHasher.Verify(token, recuperacao.Token ?? ""))
                    {
                        recuperacaoValida = recuperacao;
                        break;
                    }
                }

                if (recuperacaoValida == null)
                {
                    _logger.LogWarning("Tentativa de uso de token inválido ou expirado");
                    throw new ValidationException(
                        "Token inválido ou expirado",
                        new Dictionary<string, string[]> { 
                            { "token", new[] { "Este link de recuperação é inválido ou expirou" } } 
                        }
                    );
                }

                var usuario = recuperacaoValida.Usuario;
                if (usuario == null)
                {
                    throw new ValidationException("Usuário não encontrado");
                }

                // Atualiza a senha
                usuario.SenhaHash = PasswordHasher.Hash(novaSenha);
                usuario.DataAlteracao = DateTime.UtcNow;
                await _dbContext.SaveChangesAsync();

                // Marca o token como utilizado
                recuperacaoValida.Utilizado = true;
                recuperacaoValida.DataUtilizacao = DateTime.UtcNow;
                await _repositorio.UpdateAsync(recuperacaoValida);

                // Cancela todos os outros tokens pendentes deste usuário
                await CancelarTokensPendentesAsync(usuario.Id);

                // Envia email de confirmação
                var placeholders = new Dictionary<string, string>
                {
                    { "nomeUsuario", usuario.Nome },
                    { "urlAplicacao", _emailConfig.UrlAplicacaoWeb },
                    { "ano", DateTime.UtcNow.Year.ToString() }
                };

                await _emailServico.EnviarEmailComTemplateAsync(
                    usuario.Email ?? string.Empty,
                    usuario.Nome,
                    "Senha Redefinida com Sucesso - GRP",
                    "confirmacao-senha-redefinida",
                    placeholders
                );

                _logger.LogInformation($"Senha redefinida com sucesso para usuário {usuario.Id}");

                return new RedefinicaoSenhaResponseDto
                {
                    Sucesso = true,
                    Mensagem = "Senha redefinida com sucesso! Você pode fazer login com a nova senha."
                };
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao redefinir senha: {ex.Message}");
                throw new Exception("Erro ao redefinir senha. Tente novamente mais tarde.");
            }
        }

        /// <summary>
        /// Cancela todos os tokens pendentes de um usuário
        /// </summary>
        public async Task CancelarTokensPendentesAsync(long usuarioId)
        {
            try
            {
                var tokensPendentes = await _repositorio.ObterTodosPendentesAsync(usuarioId);

                foreach (var token in tokensPendentes)
                {
                    token.Utilizado = true;
                    token.DataUtilizacao = DateTime.UtcNow;
                    await _repositorio.UpdateAsync(token);
                }

                _logger.LogInformation($"Tokens pendentes cancelados para usuário {usuarioId}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao cancelar tokens pendentes: {ex.Message}");
                // Não relança exceção pois é uma operação complementar
            }
        }

        /// <summary>
        /// Mascara um email para exibição (mostra apenas primeiro caractere e domínio)
        /// </summary>
        private string MascararEmail(string email)
        {
            if (string.IsNullOrEmpty(email) || !email.Contains("@"))
                return "seu email";

            var partes = email.Split('@');
            var nome = partes[0];
            var dominio = partes[1];

            if (nome.Length <= 1)
                return email;

            return $"{nome[0]}***@{dominio}";
        }
    }
}
