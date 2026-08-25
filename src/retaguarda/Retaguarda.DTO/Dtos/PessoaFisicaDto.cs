using System;

namespace Retaguarda.DTO.Dtos
{
    public class PessoaFisicaDto
    {
        public long Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? NomeSocial { get; set; }
        public string? Cpf { get; set; }
        public DateTime? DataNascimento { get; set; }
        public int? Sexo { get; set; }
        public int? EstadoCivil { get; set; }
        public string? NomeMae { get; set; }
        public string? NomePai { get; set; }
        public bool Pcd { get; set; }
        public DateTime? DataObito { get; set; }
        public bool Ativo { get; set; } = true;
    }
}
