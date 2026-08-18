namespace Retaguarda.Dominio.Entidades.Enumeracoes
{
    /// <summary>
    /// Interface base para enumerações que precisam de conversão entre chave e descrição.
    /// Todas as enumerações devem implementar isso para oferecer métodos comuns.
    /// </summary>
    public interface IEnumeracao
    {
        string Chave { get; }
        string Descricao { get; }
    }
}
