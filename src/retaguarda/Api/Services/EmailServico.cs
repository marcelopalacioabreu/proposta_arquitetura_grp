using System.Net;
using System.Net.Mail;
using Retaguarda.Api.Models;
using Retaguarda.Api.Services.Interfaces;

namespace Retaguarda.Api.Services
{
    /// <summary>
    /// Implementação do serviço de envio de emails
    /// Responsável por carregar templates e enviar emails via SMTP
    /// </summary>
    public class EmailServico : IEmailServico
    {
        private readonly EmailConfiguracao _emailConfig;
        private readonly ILogger<EmailServico> _logger;
        private readonly string _caminhoTemplates;

        public EmailServico(IConfiguration configuration, ILogger<EmailServico> logger)
        {
            _logger = logger;
            _emailConfig = new EmailConfiguracao();
            configuration.GetSection("Email").Bind(_emailConfig);

            // Caminho dos templates: bin/Release|Debug/EMAIL_TEMPLATES ou src/retaguarda/Api/EMAIL_TEMPLATES
            _caminhoTemplates = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "EMAIL_TEMPLATES"
            );

            ValidarConfiguracao();
        }

        /// <summary>
        /// Valida se a configuração de email está completa
        /// </summary>
        private void ValidarConfiguracao()
        {
            if (string.IsNullOrWhiteSpace(_emailConfig.Host) ||
                string.IsNullOrWhiteSpace(_emailConfig.Username) ||
                string.IsNullOrWhiteSpace(_emailConfig.Password) ||
                string.IsNullOrWhiteSpace(_emailConfig.FromAddress))
            {
                _logger.LogWarning("Configuração de email incompleta. Verifique appsettings.json");
            }

            if (!Directory.Exists(_caminhoTemplates))
            {
                _logger.LogWarning($"Diretório de templates não encontrado: {_caminhoTemplates}");
            }
        }

        /// <summary>
        /// Carrega um template de email e substitui placeholders
        /// </summary>
        public async Task<string> CarregarTemplateAsync(string nomeTemplate, Dictionary<string, string> placeholders)
        {
            try
            {
                var caminhoTemplate = Path.Combine(_caminhoTemplates, $"{nomeTemplate}.html");

                if (!File.Exists(caminhoTemplate))
                {
                    _logger.LogError($"Template não encontrado: {caminhoTemplate}");
                    throw new FileNotFoundException($"Template '{nomeTemplate}' não encontrado");
                }

                var conteudo = await File.ReadAllTextAsync(caminhoTemplate);

                // Substitui placeholders: {{chave}} por valor
                foreach (var placeholder in placeholders)
                {
                    conteudo = conteudo.Replace($"{{{{{placeholder.Key}}}}}", placeholder.Value ?? string.Empty);
                }

                return conteudo;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao carregar template '{nomeTemplate}': {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Envia um email via SMTP
        /// </summary>
        public async Task<bool> EnviarEmailAsync(string paraEmail, string paraNome, string assunto, string conteudoHtml)
        {
            try
            {
                using (var client = new SmtpClient(_emailConfig.Host, _emailConfig.Port))
                {
                    client.EnableSsl = _emailConfig.EnableSsl;
                    client.Credentials = new NetworkCredential(_emailConfig.Username, _emailConfig.Password);
                    client.Timeout = 30000; // 30 segundos

                    using (var mensagem = new MailMessage())
                    {
                        mensagem.From = new MailAddress(_emailConfig.FromAddress, _emailConfig.FromDisplayName);
                        mensagem.To.Add(new MailAddress(paraEmail, paraNome));
                        mensagem.Subject = assunto;
                        mensagem.Body = conteudoHtml;
                        mensagem.IsBodyHtml = true;

                        await client.SendMailAsync(mensagem);

                        _logger.LogInformation($"Email enviado com sucesso para {paraEmail}");
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao enviar email para {paraEmail}: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Carrega template e envia email em uma única operação
        /// </summary>
        public async Task<bool> EnviarEmailComTemplateAsync(string paraEmail, string paraNome, string assunto,
            string nomeTemplate, Dictionary<string, string> placeholders)
        {
            try
            {
                var conteudoHtml = await CarregarTemplateAsync(nomeTemplate, placeholders);
                return await EnviarEmailAsync(paraEmail, paraNome, assunto, conteudoHtml);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao enviar email com template '{nomeTemplate}': {ex.Message}");
                return false;
            }
        }
    }
}
