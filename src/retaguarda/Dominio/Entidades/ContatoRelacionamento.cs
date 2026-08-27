using Retaguarda.Dominio.Entidades.Base;

namespace Retaguarda.Dominio.Entidades
{
    public class ContatoRelacionamento : MultilocatarioEntidade
    {
        public long ContatoId { get; set; }
        public long? OrganizacaoId { get; set; }
        public long? OrganizacaoUnidadeId { get; set; }
        public long? OrganizacaoSetorId { get; set; }
        public long? PessoaId { get; set; }
        public Contato Contato { get; set; }
    }
}
