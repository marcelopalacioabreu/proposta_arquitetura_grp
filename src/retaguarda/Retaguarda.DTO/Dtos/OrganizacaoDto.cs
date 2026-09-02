using System;

namespace Retaguarda.DTO.Dtos
{
    public class OrganizacaoDto
    {
        // Organização fields
        public long Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Codigo { get; set; } = string.Empty;
        public string Sigla { get; set; } = string.Empty;
        public long? PessoaId { get; set; }
        public long? TipoId { get; set; }
        public long? SituacaoId { get; set; }
        public long? OrganizacaoPaiId { get; set; }
        public long? OrganizacaoRaizId { get; set; }
        public long? Nivel { get; set; }
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
    }
}
