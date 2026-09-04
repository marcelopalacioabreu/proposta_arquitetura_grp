using System;

namespace Retaguarda.DTO.Dtos
{
    public class PessoaDto
    {
        public long Id { get; set; }
        public string TipoPessoa { get; set; } = "F"; // "F" = Física | "J" = Jurídica
        public DateTime? DataInsercao { get; set; }
        public bool Ativo { get; set; } = true;

        // Pessoa Física
        public string? Nome { get; set; }
        public string? NomeSocial { get; set; }
        public string? Cpf { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string? Sexo { get; set; }         // chave: MASCULINO / FEMININO
        public string? EstadoCivil { get; set; }  // chave: SOLTEIRO / CASADO …
        public string? NomeMae { get; set; }
        public string? NomePai { get; set; }
        public bool Pcd { get; set; }
        public DateTime? DataObito { get; set; }

        // Pessoa Jurídica
        public string? RazaoSocial { get; set; }
        public string? NomeFantasia { get; set; }
        public string? Cnpj { get; set; }
        public DateTime? DataFundacao { get; set; }
        public DateTime? DataExtincao { get; set; }
        public string? InscricaoEstadual { get; set; }
        public string? InscricaoMunicipal { get; set; }
        public long? SituacaoId { get; set; }

        // Endereços
        public EnderecoSubcadastroDto[]? Enderecos { get; set; }
    }
}
