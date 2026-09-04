using System;

namespace Retaguarda.DTO.Dtos
{
    public class OrganizacaoUnidadeDto
    {
        // Unidade fields
        public long Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Codigo { get; set; } = string.Empty;
        public string Sigla { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public long? OrganizacaoId { get; set; }
        public string? OrganizacaoCodigo { get; set; }
        public long? UnidadePaiId { get; set; }
        public long? TipoId { get; set; }
        public long? SituacaoId { get; set; }
        public long? ResponsavelPessoaId { get; set; }
        public long? PessoaId { get; set; }
        public string HierarquiaCodigo { get; set; } = string.Empty;
        public string HierarquiaNome { get; set; } = string.Empty;
        public long? Nivel { get; set; }
        public DateTime? DataFundacao { get; set; }
        public DateTime? DataExtincao { get; set; }
        public DateTime? ValidoDe { get; set; }
        public DateTime? ValidoAte { get; set; }
        public DateTime? DataInsercao { get; set; }
        public bool Ativo { get; set; } = true;

        // Pessoa Jurídica fields (composed)
        public string PessoaRazaoSocial { get; set; } = string.Empty;
        public string? PessoaNomeFantasia { get; set; }
        public DateTime? PessoaDataFundacao { get; set; }
        public DateTime? PessoaDataExtincao { get; set; }
        public string? PessoaCnpj { get; set; }
        public string? PessoaAnotacoes { get; set; }
        public string? PessoaInscricaoEstadual { get; set; }
        public string? PessoaInscricaoMunicipal { get; set; }

        // Endereços
        public EnderecoSubcadastroDto[]? Enderecos { get; set; }
    }
}
