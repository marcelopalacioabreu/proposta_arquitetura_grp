using Retaguarda.Dominio.Entidades.Base;

namespace Retaguarda.Dominio.Entidades
{
    public class Endereco : MultilocatarioEntidade
    {
        public long UsuarioId { get; set; }
        public long CepId { get; set; }
        public string Complemento { get; set; } = string.Empty;

        public Usuario? Usuario { get; set; }
        public Cep? Cep { get; set; }
    }
}
