using System;
using Retaguarda.Dominio.Entidades.Base;

namespace Retaguarda.Dominio.Entidades
{
    public class PessoaJuridica : Pessoa
    {
        public string RazaoSocial { get; set; } = string.Empty;
        public string NomeFantasia { get; set; } = string.Empty;
        public DateTime? DataFundacao { get; set; }
        public DateTime? DataExtincao { get; set; }
        public string Cnpj { get; set; } = string.Empty;
        public string Anotacoes { get; set; } = string.Empty;
        public string InscricaoEstadual { get; set; } = string.Empty;
        public string InscricaoMunicipal { get; set; } = string.Empty;
    }
}
