using Retaguarda.Dominio.Entidades.Base;

namespace Retaguarda.Dominio.Entidades
{
    public class OrganizacaoSetor : MultilocatarioEntidade
    {
        public string Nome { get; set; } = string.Empty;

        // Hierarquia organizacional do setor (ex: "/Org/Unidade/Setor")
        public string Hierarquia { get; set; } = string.Empty;

        // Referência ao setor superior (opcional) — armazenamos apenas o Id
        public long? SetorPaiId { get; set; }
        public OrganizacaoSetor? SetorPai { get; set; }

        public Organizacao? Organizacao { get; set; }
    }
}
