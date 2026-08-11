namespace Retaguarda.DTO.Dtos
{
    public class OrganizacaoUnidadeDto
    {
        public long Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Codigo { get; set; } = string.Empty;
        public string Sigla { get; set; } = string.Empty;
        public long? TipoUnidadeId { get; set; }
        public long? UnidadePaiId { get; set; }
        public string Cnpj { get; set; } = string.Empty;
        public long? SituacaoId { get; set; }
        public long? ResponsavelPessoaId { get; set; }
        public System.DateTime? DataFundacao { get; set; }
        public System.DateTime? DataExtincao { get; set; }
        public string HierarquiaCodigo { get; set; } = string.Empty;
        public string HierarquiaNome { get; set; } = string.Empty;
        public short? Nivel { get; set; }
        public System.DateTime? ValidoDe { get; set; }
        public System.DateTime? ValidoAte { get; set; }
        public bool Ativo { get; set; } = true;
    }
}
