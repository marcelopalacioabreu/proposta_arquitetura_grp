using System;

namespace Retaguarda.DTO.Dtos
{
    public class PessoaJuridicaDto
    {
        public long Id { get; set; }
        public string RazaoSocial { get; set; } = string.Empty;
        public string? NomeFantasia { get; set; }
        public DateTime? DataFundacao { get; set; }
        public DateTime? DataExtincao { get; set; }
        public string? Cnpj { get; set; }
        public string? Anotacoes { get; set; }
        public string? InscricaoEstadual { get; set; }
        public string? InscricaoMunicipal { get; set; }
        public bool Ativo { get; set; } = true;
    }
}
