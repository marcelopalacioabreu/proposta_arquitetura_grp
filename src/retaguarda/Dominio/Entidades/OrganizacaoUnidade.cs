using Retaguarda.Dominio.Entidades.Base;

namespace Retaguarda.Dominio.Entidades
{
    public class OrganizacaoUnidade : MultilocatarioEntidade
    {
        public string Nome { get; set; } = string.Empty;
        public string Codigo { get; set; } = string.Empty;
        public string Sigla { get; set; } = string.Empty;
        public long? UnidadePaiId { get; set; }
        public long? Nivel { get; set; }
        public long? ResponsavelId { get; set; } /*Pessoa responsável pela unidade*/
        public long? PessoaId { get; set; }/* Pessoa jurídica da organização */
        public long? SituacaoId { get; set; }
        public Organizacao? Organizacao { get; set; }
        public Pessoa? Pessoa { get; set; }
        public Situacao? Situacao { get; set; }
        public ICollection<OrganizacaoUnidadeEndereco>? OrganizacaoUnidadeEnderecos { get; set; }

    }
}
