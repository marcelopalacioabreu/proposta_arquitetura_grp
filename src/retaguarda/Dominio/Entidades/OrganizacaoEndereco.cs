using Retaguarda.Dominio.Entidades.Base;

namespace Retaguarda.Dominio.Entidades
{
    public class OrganizacaoEndereco : MultilocatarioEntidade
    {
        public long OrganizacaoId { get; set; }
        public long EnderecoId { get; set; }
        public long? EnderecoTipoId { get; set; }
        public bool EnderecoPrincipal { get; set; }
    }
}
