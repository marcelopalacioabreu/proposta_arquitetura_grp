using System;
using Retaguarda.Dominio.Entidades.Base;

namespace Retaguarda.Dominio.Entidades
{
    public class Pessoa : MultilocatarioEntidade
    {
        // Nome ou razão social
        public string Nome { get; set; } = string.Empty;

        public string NomeSocial { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public DateTime? DataNascimento { get; set; }
        public long? SexoId { get; set; }
        public long? EstadoCivilId { get; set; }
        public long? NacionalidadePaisId { get; set; }
        public long? NaturalidadeMunicipioId { get; set; }
        public string NomeMae { get; set; } = string.Empty;
        public string NomePai { get; set; } = string.Empty;
        public bool Pcd { get; set; }
        public DateTime? DataObito { get; set; }

        // Chave para o tipo de pessoa (ex: "F" = Física, "J" = Jurídica)
        public string TipoPessoaChave { get; set; } = string.Empty;

        // Documentos básicos (opcional)
        public string? Documento { get; set; }

        // Dados de contato
        public string? Telefone { get; set; }
        public string? Email { get; set; }
    }
}
