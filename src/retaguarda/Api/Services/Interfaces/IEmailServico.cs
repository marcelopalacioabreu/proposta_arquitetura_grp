namespace Retaguarda.Api.Services.Interfaces
{
    /// <summary>
    /// Interface para serviço de envio de emails
    /// Fornece métodos para envio de emails com templates
    /// </summary>
    public interface IEmailServico
    {
        /// <summary>
        /// Carrega um template de email e substitui os placeholders
        /// </summary>
        /// <param name="nomeTemplate">Nome do template sem extensão (ex: "recuperacao-senha")</param>
        /// <param name="placeholders">Dictionary com pares chave-valor para substituição</param>
        /// <returns>Conteúdo HTML do template com placeholders substituídos</returns>
        Task<string> CarregarTemplateAsync(string nomeTemplate, Dictionary<string, string> placeholders);

        /// <summary>
        /// Envia um email
        /// </summary>
        /// <param name="paraEmail">Email do destinatário</param>
        /// <param name="paraNome">Nome do destinatário</param>
        /// <param name="assunto">Assunto do email</param>
        /// <param name="conteudoHtml">Conteúdo HTML do email</param>
        /// <returns>True se enviado com sucesso, False caso contrário</returns>
        Task<bool> EnviarEmailAsync(string paraEmail, string paraNome, string assunto, string conteudoHtml);

        /// <summary>
        /// Carrega template e envia email em uma única operação
        /// </summary>
        /// <param name="paraEmail">Email do destinatário</param>
        /// <param name="paraNome">Nome do destinatário</param>
        /// <param name="assunto">Assunto do email</param>
        /// <param name="nomeTemplate">Nome do template sem extensão</param>
        /// <param name="placeholders">Placeholders para substituição</param>
        /// <returns>True se enviado com sucesso</returns>
        Task<bool> EnviarEmailComTemplateAsync(string paraEmail, string paraNome, string assunto, 
            string nomeTemplate, Dictionary<string, string> placeholders);
    }
}
