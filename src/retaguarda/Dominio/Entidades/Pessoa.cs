using Retaguarda.Dominio.Entidades.Base;
using Retaguarda.Dominio.Entidades.Enumeracoes;

namespace Retaguarda.Dominio.Entidades
{

    public class Pessoa : MultilocatarioEntidade
    {
        public PessoaTipo TipoPessoa { get; set; } = PessoaTipo.Fisica;
    }
}
