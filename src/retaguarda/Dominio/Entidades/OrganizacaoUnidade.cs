using System;
using Retaguarda.Dominio.Entidades.Base;

namespace Retaguarda.Dominio.Entidades
{
    public class OrganizacaoUnidade : MultilocatarioEntidade
    {
        public string Nome { get; set; } = string.Empty;

        // Business fields
        public string Codigo { get; set; } = string.Empty;
        public string Sigla { get; set; } = string.Empty;
        public long? TipoUnidadeId { get; set; }
        public long? UnidadePaiId { get; set; }
        public string Cnpj { get; set; } = string.Empty;
        public long? SituacaoId { get; set; }
        public long? ResponsavelPessoaId { get; set; }
        public DateTime? DataFundacao { get; set; }
        public DateTime? DataExtincao { get; set; }
        public string HierarquiaCodigo { get; set; } = string.Empty;
        public string HierarquiaNome { get; set; } = string.Empty;
        public short? Nivel { get; set; }
        public DateTime? ValidoDe { get; set; }
        public DateTime? ValidoAte { get; set; }

        public Organizacao? Organizacao { get; set; }
    }
}
