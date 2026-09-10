namespace Retaguarda.DTO.Dtos
{
    /// <summary>
    /// DTO para solicitar recuperação de senha
    /// Enviado pelo usuário quando esqueceu a senha
    /// </summary>
    public class SolicitarRecuperacaoSenhaDto
    {
        /// <summary>
        /// Email do usuário que deseja recuperar a senha
        /// </summary>
        public string Email { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO para validar e redefinir a senha
    /// Contém o token recebido por email e a nova senha
    /// </summary>
    public class RedefinirSenhaDto
    {
        /// <summary>
        /// Token de recuperação recebido por email
        /// </summary>
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// Nova senha (será criptografada no backend)
        /// </summary>
        public string NovaSenha { get; set; } = string.Empty;

        /// <summary>
        /// Confirmação da nova senha (validação do frontend)
        /// </summary>
        public string ConfirmacaoSenha { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO para validar um token de recuperação
    /// </summary>
    public class ValidarTokenRecuperacaoDto
    {
        /// <summary>
        /// Token a ser validado
        /// </summary>
        public string Token { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO de resposta após solicitar recuperação
    /// </summary>
    public class RecuperacaoSenhaResponseDto
    {
        /// <summary>
        /// Mensagem de sucesso
        /// </summary>
        public string Mensagem { get; set; } = string.Empty;

        /// <summary>
        /// Email para o qual o link foi enviado (parcialmente mascarado)
        /// </summary>
        public string EmailEnviado { get; set; } = string.Empty;

        /// <summary>
        /// Tempo de expiração em minutos
        /// </summary>
        public int TempoExpiracaoMinutos { get; set; }
    }

    /// <summary>
    /// DTO de resposta ao validar token
    /// </summary>
    public class ValidacaoTokenResponseDto
    {
        /// <summary>
        /// Indica se o token é válido
        /// </summary>
        public bool TokenValido { get; set; }

        /// <summary>
        /// Mensagem informativa
        /// </summary>
        public string Mensagem { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO de resposta ao redefinir senha
    /// </summary>
    public class RedefinicaoSenhaResponseDto
    {
        /// <summary>
        /// Indica se a redefinição foi bem-sucedida
        /// </summary>
        public bool Sucesso { get; set; }

        /// <summary>
        /// Mensagem informativa
        /// </summary>
        public string Mensagem { get; set; } = string.Empty;
    }
}
