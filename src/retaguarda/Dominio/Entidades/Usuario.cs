using Retaguarda.Dominio.Entidades.Base;

namespace Retaguarda.Dominio.Entidades
{
    public class Usuario : MultilocatarioEntidade
    {
        public string Nome { get; set; } = string.Empty;
        public string SenhaHash { get; set; } = string.Empty;
        public string? Email { get; set; }
        public long? PessoaId { get; set; }
        public long? UltimoAcessoOrganizacaoId { get; set; }
        public long? UltimoAcessoOrganizacaoUnidadeId { get; set; }
        public long? UltimoAcessoSetorId { get; set; }
    }
}
