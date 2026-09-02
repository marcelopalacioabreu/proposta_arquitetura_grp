using Retaguarda.Dominio.Entidades.Base;

namespace Retaguarda.Dominio.Entidades
{
    public class OrganizacaoUnidade : MultilocatarioEntidade
    {
        public string Nome { get; set; } = string.Empty;
        public string Codigo { get; set; } = string.Empty;
        public string Sigla { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public long? OrganizacaoId { get; set; }
        public long? UnidadePaiId { get; set; }
        public long? TipoId { get; set; } /* Tipo de Unidade */
        public long? SituacaoId { get; set; } /* Situação da Unidade */
        public long? ResponsavelId { get; set; } /* Pessoa responsável pela unidade */
        public long? PessoaId { get; set; } /* Pessoa jurídica da unidade */
        public long? Nivel { get; set; }
        public DateTime? DataFundacao { get; set; }
        public DateTime? DataExtincao { get; set; }
        public Organizacao? Organizacao { get; set; }
        public Pessoa? Pessoa { get; set; }
        public Tipo? Tipo { get; set; }
        public Situacao? Situacao { get; set; }
        public ICollection<OrganizacaoUnidadeEndereco>? OrganizacaoUnidadeEnderecos { get; set; }

    }
}
