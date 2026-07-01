using Retaguarda.Dominio.Entidades.Base;

namespace Retaguarda.Dominio.Entidades
{
    public class OrganizacaoSetor : MultilocatarioEntidade
    {
        public string Nome { get; set; } = string.Empty;

        public Organizacao? Organizacao { get; set; }
    }
}
