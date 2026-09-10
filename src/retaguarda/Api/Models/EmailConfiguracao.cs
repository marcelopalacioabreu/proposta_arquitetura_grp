namespace Retaguarda.Api.Models
{
    /// <summary>
    /// Configuração de envio de emails para a aplicação
    /// Lida com SMTP e parâmetros gerais de email
    /// </summary>
    public class EmailConfiguracao
    {
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; } = 587;
        public bool EnableSsl { get; set; } = true;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FromAddress { get; set; } = string.Empty;
        public string FromDisplayName { get; set; } = string.Empty;
        public int RecuperacaoSenhaExpiracaoMinutos { get; set; } = 120;
        public string UrlAplicacaoWeb { get; set; } = string.Empty;
    }
}
