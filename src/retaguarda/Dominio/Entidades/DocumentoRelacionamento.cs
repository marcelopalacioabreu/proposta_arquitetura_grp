using Retaguarda.Dominio.Entidades.Base;

namespace Retaguarda.Dominio.Entidades
{
    public class DocumentoRelacionamento : MultilocatarioEntidade
    {
        public long DocumentoId { get; set; }
        public long? PessoaId { get; set; }
        public long? OrganizacaoId { get; set; }
        public long? OrganizacaoUnidadeId { get; set; }
        public long? OrganizacaoSetorId { get; set; }
        public Documento Documento { get; set; }
    }
}
