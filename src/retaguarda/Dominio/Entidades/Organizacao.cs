using Retaguarda.Dominio.Entidades.Base;

namespace Retaguarda.Dominio.Entidades
{
    public class Organizacao : MultilocatarioEntidade
    {
        public string Nome { get; set; } = string.Empty;
        public string Codigo { get; set; } = string.Empty;
        public string Sigla { get; set; } = string.Empty;
        public long? Nivel { get; set; }
        public long? ResponsavelId { get; set; } /*Pessoa responsável pela organização*/
        public long? PessoaId { get; set; }/* Pessoa jurídica da organização */
        public long? SituacaoId { get; set; }
        public long? OrganizacaoPaiId { get; set; }
        public long? OrganizacaoRaizId { get; set; }
    }
}
