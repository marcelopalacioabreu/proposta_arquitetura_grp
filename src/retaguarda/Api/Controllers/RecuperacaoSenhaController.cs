using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Retaguarda.Api.Models;
using Retaguarda.Api.Services.Interfaces;
using Retaguarda.DTO.Dtos;
using Retaguarda.DTO.Exceptions;
using Retaguarda.Persistencia;

namespace Retaguarda.Api.Controllers
{
    /// <summary>
    /// Controller para gerenciar recuperação de senha
    /// Endpoints públicos (sem autenticação) para solicitar recuperação e redefinir senha
    /// </summary>
    [ApiController]
    [Route("api/recuperacao-senha")]
    public class RecuperacaoSenhaController : BaseController
    {
        private readonly IRecuperacaoSenhaServico _servico;
        private readonly ILogger<RecuperacaoSenhaController> _logger;

        public RecuperacaoSenhaController(
            IRecuperacaoSenhaServico servico,
            ILogger<RecuperacaoSenhaController> logger)
        {
            _servico = servico;
            _logger = logger;
        }

        /// <summary>
        /// Solicita um link de recuperação de senha
        /// Envia um email com um token válido por tempo limitado
        /// </summary>
        /// <param name="dto">Contém o email do usuário</param>
        /// <returns>Mensagem de sucesso (genérica por segurança)</returns>
        /// <response code="200">Solicitação processada (mensagem genérica por segurança)</response>
        /// <response code="400">Email não fornecido ou inválido</response>
        /// <response code="500">Erro ao processar solicitação</response>
        [HttpPost("solicitar")]
        [AllowAnonymous]
        public async Task<IActionResult> Solicitar([FromBody] SolicitarRecuperacaoSenhaDto dto)
        {
            try
            {
                // Extrai informações do cliente para auditoria
                var ipCliente = HttpContext.Connection.RemoteIpAddress?.ToString();
                var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();

                var resultado = await _servico.SolicitarRecuperacaoAsync(dto.Email, ipCliente, userAgent);

                _logger.LogInformation($"Solicitação de recuperação processada para: {dto.Email}");

                return OkData(resultado, resultado.Mensagem);
            }
            catch (ValidationException ex)
            {
                return BadRequest(EnvelopeResult.Error(ex.Mensagem ?? "Validação falhou", ex.Errors));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao solicitar recuperação de senha: {ex.Message}");
                return Error("Erro ao processar solicitação. Tente novamente mais tarde.");
            }
        }

        /// <summary>
        /// Valida se um token de recuperação ainda é válido
        /// Usado pelo frontend para verificar se o link é válido antes de mostrar formulário
        /// </summary>
        /// <param name="dto">Contém o token a ser validado</param>
        /// <returns>Status de validade do token</returns>
        /// <response code="200">Token validado</response>
        /// <response code="400">Token inválido ou não fornecido</response>
        [HttpPost("validar-token")]
        [AllowAnonymous]
        public async Task<IActionResult> ValidarToken([FromBody] ValidarTokenRecuperacaoDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.Token))
                {
                    return BadRequest(EnvelopeResult.Error(
                        "Token não fornecido",
                        new Dictionary<string, string[]> { { "token", new[] { "Token é obrigatório" } } }
                    ));
                }

                var valido = await _servico.ValidarTokenAsync(dto.Token);

                var resposta = new ValidacaoTokenResponseDto
                {
                    TokenValido = valido,
                    Mensagem = valido ? "Token válido" : "Token inválido ou expirado"
                };

                return OkData(resposta, resposta.Mensagem);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao validar token: {ex.Message}");
                return Error("Erro ao validar token. Tente novamente mais tarde.");
            }
        }

        /// <summary>
        /// Redefine a senha do usuário usando um token válido
        /// Este é o endpoint final da recuperação de senha
        /// </summary>
        /// <param name="dto">Contém o token e a nova senha</param>
        /// <returns>Resultado da redefinição</returns>
        /// <response code="200">Senha redefinida com sucesso</response>
        /// <response code="400">Dados inválidos ou token expirado</response>
        /// <response code="500">Erro ao redefinir senha</response>
        [HttpPost("redefinir")]
        [AllowAnonymous]
        public async Task<IActionResult> Redefinir([FromBody] RedefinirSenhaDto dto)
        {
            try
            {
                // Validações básicas
                if (string.IsNullOrWhiteSpace(dto.Token))
                {
                    return BadRequest(EnvelopeResult.Error(
                        "Dados inválidos",
                        new Dictionary<string, string[]> { { "token", new[] { "Token é obrigatório" } } }
                    ));
                }

                if (string.IsNullOrWhiteSpace(dto.NovaSenha))
                {
                    return BadRequest(EnvelopeResult.Error(
                        "Dados inválidos",
                        new Dictionary<string, string[]> { { "novaSenha", new[] { "Senha é obrigatória" } } }
                    ));
                }

                // Valida se senhas correspondem
                if (dto.NovaSenha != dto.ConfirmacaoSenha)
                {
                    return BadRequest(EnvelopeResult.Error(
                        "Dados inválidos",
                        new Dictionary<string, string[]> { 
                            { "confirmacaoSenha", new[] { "Confirmação de senha não corresponde" } } 
                        }
                    ));
                }

                var resultado = await _servico.RedefinirSenhaAsync(dto.Token, dto.NovaSenha);

                _logger.LogInformation("Senha redefinida com sucesso");

                return OkData(resultado, resultado.Mensagem);
            }
            catch (ValidationException ex)
            {
                return BadRequest(EnvelopeResult.Error(ex.Mensagem ?? "Validação falhou", ex.Errors));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao redefinir senha: {ex.Message}");
                return Error("Erro ao redefinir senha. Tente novamente mais tarde.");
            }
        }
    }
}
