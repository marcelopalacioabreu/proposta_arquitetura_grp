using Retaguarda.DTO.Dtos;

namespace Retaguarda.Api.Services.Interfaces
{
    /// <summary>
    /// Interface para o serviço de recuperação de senha
    /// Responsável pela lógica de negócio de recuperação de senha
    /// </summary>
    public interface IRecuperacaoSenhaServico
    {
        /// <summary>
        /// Solicita uma recuperação de senha para um usuário
        /// Gera um token único e envia email com o link
        /// </summary>
        /// <param name="email">Email do usuário</param>
        /// <param name="ipCliente">IP do cliente (para auditoria)</param>
        /// <param name="userAgent">User Agent do cliente (para auditoria)</param>
        /// <returns>DTO de resposta com informações da solicitação</returns>
        Task<RecuperacaoSenhaResponseDto> SolicitarRecuperacaoAsync(string email, string? ipCliente = null, string? userAgent = null);

        /// <summary>
        /// Valida se um token de recuperação é válido e não expirou
        /// </summary>
        /// <param name="token">Token a ser validado</param>
        /// <returns>True se válido, False caso contrário</returns>
        Task<bool> ValidarTokenAsync(string token);

        /// <summary>
        /// Redefine a senha do usuário usando um token válido
        /// </summary>
        /// <param name="token">Token de recuperação</param>
        /// <param name="novaSenha">Nova senha em texto plano</param>
        /// <returns>DTO de resposta com resultado da redefinição</returns>
        Task<RedefinicaoSenhaResponseDto> RedefinirSenhaAsync(string token, string novaSenha);

        /// <summary>
        /// Cancela todos os tokens pendentes de um usuário
        /// Útil para invalidar recuperações anteriores quando usuário redefinir senha
        /// </summary>
        /// <param name="usuarioId">ID do usuário</param>
        Task CancelarTokensPendentesAsync(long usuarioId);
    }
}
