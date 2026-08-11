using System;
using System.Collections.Generic;

namespace Retaguarda.Api.Utils
{
    public static class FiltrosHelper
    {
        public static Dictionary<string,string> MontarFiltros(string? campo, string? operador, string? valor, string? valorDe, string? valorAte)
        {
            var filtros = new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);
            if (!string.IsNullOrWhiteSpace(campo)) filtros["campo"] = campo!;
            if (!string.IsNullOrWhiteSpace(operador)) filtros["operador"] = operador!;
            if (!string.IsNullOrWhiteSpace(valor)) filtros["valor"] = valor!;
            if (!string.IsNullOrWhiteSpace(valorDe)) filtros["valor_de"] = valorDe!;
            if (!string.IsNullOrWhiteSpace(valorAte)) filtros["valor_ate"] = valorAte!;
            return filtros;
        }
    }
}
