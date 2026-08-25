namespace Retaguarda.DTO.Dtos
{
    public class TipoDto
    {
        public long Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string Contexto { get; set; } = string.Empty; // ENDERECO, CONTATO, UNIDADE, IMOVEL, DOCUMENTO
        public string Descricao { get; set; } = string.Empty;
        public int? Ordem { get; set; }
        public bool Ativo { get; set; } = true;
    }
}
