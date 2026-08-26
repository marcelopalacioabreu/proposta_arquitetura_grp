using Retaguarda.Dominio.Entidades.Base;
using Retaguarda.Dominio.Entidades.Enumeracoes;
using System.Collections.Generic;

namespace Retaguarda.Dominio.Entidades
{

    public class Pessoa : MultilocatarioEntidade
    {
        public PessoaTipo TipoPessoa { get; set; } = PessoaTipo.Fisica;
        public List<PessoaEndereco> Enderecos { get; set; } = new List<PessoaEndereco>();
    }
}
