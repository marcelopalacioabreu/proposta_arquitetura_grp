namespace Retaguarda.Api.Models
{
    public class EnvelopeResult
    {
        public bool Erro { get; set; }
        public string? Mensagem { get; set; }
        public object? Data { get; set; }
        public object? Items { get; set; }
        public int? Total { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public string? RedirecionarPara { get; set; }
        public object? Detalhes { get; set; }

        public static EnvelopeResult SuccessData(object? data, string? mensagem = null)
        {
            return new EnvelopeResult { Erro = false, Data = data, Mensagem = mensagem };
        }

        public static EnvelopeResult SuccessList(object items, int total, int page, int pageSize, string? mensagem = null)
        {
            return new EnvelopeResult { Erro = false, Items = items, Total = total, Page = page, PageSize = pageSize, Mensagem = mensagem };
        }

        public static EnvelopeResult SuccessMessage(string mensagem)
        {
            return new EnvelopeResult { Erro = false, Mensagem = mensagem };
        }

        public static EnvelopeResult Error(string mensagem, object? detalhes = null)
        {
            return new EnvelopeResult { Erro = true, Mensagem = mensagem, Detalhes = detalhes };
        }
    }
}
