using Retaguarda.Dominio.Entidades.Base;

namespace Retaguarda.Dominio.Entidades
{
    /// <summary>
    /// Entidade que armazena solicitações de recuperação de senha
    /// Cada registro representa um token único com validade temporal
    /// </summary>
    public class RecuperacaoSenha : MultilocatarioEntidade
    {
        /// <summary>
        /// Identificador do usuário que solicitou a recuperação
        /// </summary>
        public long UsuarioId { get; set; }

        /// <summary>
        /// Referência ao usuário (navegação)
        /// </summary>
        public Usuario? Usuario { get; set; }

        /// <summary>
        /// Token criptografado (hash) - armazenamos o hash por segurança, não o valor original
        /// </summary>
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// Data e hora de expiração do token de recuperação
        /// </summary>
        public DateTime DataExpiracao { get; set; }

        /// <summary>
        /// Indica se este token já foi utilizado para redefinir a senha
        /// </summary>
        public bool Utilizado { get; set; } = false;

        /// <summary>
        /// Data e hora em que o token foi utilizado (preenchido quando Utilizado = true)
        /// </summary>
        public DateTime? DataUtilizacao { get; set; }

        /// <summary>
        /// IP do cliente que solicitou a recuperação (para auditoria)
        /// </summary>
        public string? IpSolicitacao { get; set; }

        /// <summary>
        /// User Agent do cliente (para auditoria)
        /// </summary>
        public string? UserAgent { get; set; }
    }
}
