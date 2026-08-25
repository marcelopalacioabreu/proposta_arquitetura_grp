using Retaguarda.Dominio.Entidades.Base;

namespace Retaguarda.Dominio.Entidades
{
    /// <summary>
    /// Tabela unificada de Tipos do sistema.
    /// Agrupa todos os tipos (Endereço, Contato, Unidade, Imóvel, Documento) em uma única tabela.
    /// O campo Contexto identifica o domínio do tipo (ENDERECO, CONTATO, UNIDADE, IMOVEL, DOCUMENTO).
    /// </summary>
    public class Tipo : MultilocatarioEntidade
    {
        public string Codigo { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string Contexto { get; set; } = string.Empty; // ENDERECO, CONTATO, UNIDADE, IMOVEL, DOCUMENTO
        public string Descricao { get; set; } = string.Empty;
        public int? Ordem { get; set; }
    }
}
