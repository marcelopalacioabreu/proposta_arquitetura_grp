using Retaguarda.Dominio.Entidades.Base;

namespace Retaguarda.Dominio.Entidades
{
    public class Pessoa : MultilocatarioEntidade
    {
        // Nome ou razão social
        public string Nome { get; set; } = string.Empty;

        // Chave para o tipo de pessoa (ex: "F" = Física, "J" = Jurídica)
        public string TipoPessoaChave { get; set; } = string.Empty;

        // Documentos básicos (opcional)
        public string? Documento { get; set; }

        // Dados de contato
        public string? Telefone { get; set; }
        public string? Email { get; set; }
    }
}
