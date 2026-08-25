using System;
using Retaguarda.Dominio.Entidades.Base;

namespace Retaguarda.Dominio.Entidades
{
    public enum Sexo
    {
        FEMININO = 1,
        MASCULINO = 2
    }

    public enum EstadoCivil
    {
        SOLTEIRA = 1,
        CASADA = 2,
        DIVORCIADA = 3,
        VIÚVA = 4,
        UNIÃO_ESTÁVEL = 5
    }

    public class PessoaFisica : Pessoa
    {
        public string Nome { get; set; } = string.Empty;
        public string NomeSocial { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public DateTime? DataNascimento { get; set; }
        public Sexo? Sexo { get; set; }
        public EstadoCivil? EstadoCivil { get; set; }
        public string NomeMae { get; set; } = string.Empty;
        public string NomePai { get; set; } = string.Empty;
        public bool Pcd { get; set; } = false;
        public DateTime? DataObito { get; set; }
    }
}
