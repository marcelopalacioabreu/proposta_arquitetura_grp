using System;
using Retaguarda.Dominio.Entidades.Base;

namespace Retaguarda.Dominio.Entidades
{
    public class Documento : MultilocatarioEntidade
    {
        public long? DocumentoTipoId { get; set; }
        public long? PessoaId { get; set; } // FK para Pessoa - quem é o titular do documento
        public string Numero { get; set; } = string.Empty;
        public string Digito { get; set; } = string.Empty;
        public string OrgaoEmissor { get; set; } = string.Empty;
        public string UfEmissor { get; set; } = string.Empty;
        public DateTime? DataEmissao { get; set; }
        public DateTime? DataValidade { get; set; }
        public bool Principal { get; set; }
        public bool Validado { get; set; }
        public string Observacao { get; set; } = string.Empty;
        public Tipo Tipo { get; set; }
    }
}
