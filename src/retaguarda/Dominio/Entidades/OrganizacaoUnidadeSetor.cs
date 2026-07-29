using Retaguarda.Dominio.Entidades.Base;

namespace Retaguarda.Dominio.Entidades
{
    public class OrganizacaoUnidadeSetor : MultilocatarioEntidade
    {
        public string Nome { get; set; } = string.Empty;

        public OrganizacaoUnidade? OrganizacaoUnidade { get; set; }
    }
}
