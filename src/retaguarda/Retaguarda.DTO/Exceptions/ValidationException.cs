using System;
using System.Collections.Generic;

namespace Retaguarda.DTO.Exceptions
{
    /// <summary>
    /// Exceção para erros de validação de negócio. 
    /// Inclui um dicionário de erros por campo.
    /// </summary>
    public class ValidationException : Exception
    {
        public Dictionary<string, string[]> Errors { get; set; }
        public string? Mensagem { get; set; }

        public ValidationException(string mensagem, Dictionary<string, string[]>? errors = null) 
            : base(mensagem)
        {
            Mensagem = mensagem;
            Errors = errors ?? new Dictionary<string, string[]>();
        }

        public void AdicionarErro(string campo, string mensagem)
        {
            if (!Errors.ContainsKey(campo))
                Errors[campo] = Array.Empty<string>();
            
            var lista = new List<string>(Errors[campo]) { mensagem };
            Errors[campo] = lista.ToArray();
        }
    }
}
