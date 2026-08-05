using System;
using System.Collections.Generic;
using Retaguarda.Dominio.Entidades.Base;

namespace Retaguarda.Dominio.Entidades
{
    public class Organizacao : MultilocatarioEntidade
    {
        public string Nome { get; set; } = string.Empty;

        public string Codigo { get; set; } = string.Empty;
        public string Sigla { get; set; } = string.Empty;
        public string RazaoSocial { get; set; } = string.Empty;
        public string NomeFantasia { get; set; } = string.Empty;
        public string Cnpj { get; set; } = string.Empty;
        public string InscricaoEstadual { get; set; } = string.Empty;
        public string InscricaoMunicipal { get; set; } = string.Empty;
        public long? TipoOrganizacaoId { get; set; }
        public long? NivelGovernoId { get; set; }
        public long? NaturezaJuridicaId { get; set; }
        public long? OrganizacaoPaiId { get; set; }
        public long? OrganizacaoRaizId { get; set; }
        public long? SituacaoId { get; set; }
        public DateTime? DataFundacao { get; set; }
        public DateTime? DataExtincao { get; set; }
        public string HierarquiaCodigo { get; set; } = string.Empty;
        public short? Nivel { get; set; }

        public ICollection<OrganizacaoSetor> Setores { get; set; } = new List<OrganizacaoSetor>();
        public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
        public ICollection<Perfil> Perfis { get; set; } = new List<Perfil>();
        public ICollection<Funcao> Funcoes { get; set; } = new List<Funcao>();
    }
}
