using Retaguarda.Dominio.Entidades.Base;

namespace Retaguarda.Dominio.Entidades
{
    public class Endereco : MultilocatarioEntidade
    {
        public long? PaisId { get; set; }
        public long? UfId { get; set; }
        public long? MunicipioId { get; set; }
        public long? BairroId { get; set; }
        public long? LogradouroId { get; set; }
        public long CepId { get; set; }
        public EnderecoPais? Pais { get; set; }
        public EnderecoUF? Uf { get; set; }
        public EnderecoMunicipio? Municipio { get; set; }
        public EnderecoBairro? Bairro { get; set; }
        public EnderecoLogradouro? Logradouro { get; set; }
        public EnderecoCEP? Cep { get; set; }
        public string Complemento { get; set; } = string.Empty;
    }
}
