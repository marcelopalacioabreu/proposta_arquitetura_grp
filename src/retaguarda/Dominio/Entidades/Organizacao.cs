using System.Collections.Generic;
using Retaguarda.Dominio.Entidades.Base;

namespace Retaguarda.Dominio.Entidades
{
    public class Organizacao : MultilocatarioEntidade
    {
        public string Nome { get; set; } = string.Empty;

        public ICollection<OrganizacaoSetor> Setores { get; set; } = new List<OrganizacaoSetor>();
        public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
        public ICollection<Perfil> Perfis { get; set; } = new List<Perfil>();
        public ICollection<Funcao> Funcoes { get; set; } = new List<Funcao>();
    }
}
