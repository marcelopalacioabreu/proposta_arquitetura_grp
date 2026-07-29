using Retaguarda.Dominio.Entidades.Base;

namespace Retaguarda.Dominio.Entidades
{
    // Association between Usuario and Setor with extra flags
    public class SetorUsuario : MultilocatarioEntidade
    {
        public long UsuarioId { get; set; }
        public long SetorId { get; set; }

        // When true, this association enables negative permissions behavior
        public bool HabilitarPermissoesNegativas { get; set; } = false;

        public Usuario? Usuario { get; set; }
        public OrganizacaoSetor? Setor { get; set; }
    }
}
