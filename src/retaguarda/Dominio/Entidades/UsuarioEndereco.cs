using Retaguarda.Dominio.Entidades.Base;

namespace Retaguarda.Dominio.Entidades
{
    public class UsuarioEndereco : MultilocatarioEntidade
    {
        public long UsuarioId { get; set; }
        public long EnderecoId { get; set; }
        public long? EnderecoTipoId { get; set; }
        public bool EnderecoPrincipal { get; set; }
    }
}
