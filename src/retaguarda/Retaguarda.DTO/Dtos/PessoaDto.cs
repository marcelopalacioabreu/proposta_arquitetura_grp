namespace Retaguarda.DTO.Dtos
{
    public class PessoaDto
    {
        public long Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string NomeSocial { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public System.DateTime? DataNascimento { get; set; }
        public long? SexoId { get; set; }
        public long? EstadoCivilId { get; set; }
        public long? NacionalidadePaisId { get; set; }
        public long? NaturalidadeMunicipioId { get; set; }
        public string NomeMae { get; set; } = string.Empty;
        public string NomePai { get; set; } = string.Empty;
        public bool Pcd { get; set; }
        public System.DateTime? DataObito { get; set; }
        public string TipoPessoaChave { get; set; } = string.Empty;
        public string? Documento { get; set; }
        public string? Telefone { get; set; }
        public string? Email { get; set; }
        public bool Ativo { get; set; } = true;
    }
}
