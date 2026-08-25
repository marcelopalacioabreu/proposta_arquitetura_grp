namespace Retaguarda.DTO.Dtos
{
    public class SituacaoDto
    {
        public long Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string Contexto { get; set; } = string.Empty; // IMOVEL
        public string Descricao { get; set; } = string.Empty;
        public bool Ativo { get; set; } = true;
    }
}
