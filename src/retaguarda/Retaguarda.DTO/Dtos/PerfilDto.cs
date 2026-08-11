namespace Retaguarda.DTO.Dtos
{
    public class PerfilDto
    {
        public long Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public bool AdministradorDoSistema { get; set; } = false;
        public bool Ativo { get; set; } = true;
    }
}
