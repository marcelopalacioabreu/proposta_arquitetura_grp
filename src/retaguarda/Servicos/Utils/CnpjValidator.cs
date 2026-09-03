using System;
using System.Text.RegularExpressions;

namespace Retaguarda.Servicos.Utils
{
    /// <summary>
    /// Utilitário para validação de CNPJ (numérico e alphanumeric)
    /// </summary>
    public static class CnpjValidator
    {
        /// <summary>
        /// Valida um CNPJ em formato numérico (14 dígitos)
        /// </summary>
        public static bool ValidarCnpjNumerico(string cnpj)
        {
            if (string.IsNullOrWhiteSpace(cnpj))
                return false;

            // Remove caracteres não-numéricos
            var numeros = Regex.Replace(cnpj, @"\D", "");

            // Deve ter exatamente 14 dígitos
            if (numeros.Length != 14)
                return false;

            // Valida se não é sequência repetida (ex: 11111111111111)
            if (Regex.IsMatch(numeros, @"^(\d)\1{13}$"))
                return false;

            // Calcula primeiro dígito verificador
            int[] multiplicadores1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma1 = 0;
            for (int i = 0; i < 12; i++)
                soma1 += int.Parse(numeros[i].ToString()) * multiplicadores1[i];

            int resto1 = soma1 % 11;
            int digito1 = resto1 < 2 ? 0 : 11 - resto1;

            // Calcula segundo dígito verificador
            int[] multiplicadores2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma2 = 0;
            for (int i = 0; i < 13; i++)
                soma2 += int.Parse(numeros[i].ToString()) * multiplicadores2[i];

            int resto2 = soma2 % 11;
            int digito2 = resto2 < 2 ? 0 : 11 - resto2;

            // Verifica os dígitos verificadores
            return numeros[12] == digito1.ToString()[0] && numeros[13] == digito2.ToString()[0];
        }

        /// <summary>
        /// Valida um CNPJ em formato alphanumeric (ex: ABCD1234567890)
        /// </summary>
        public static bool ValidarCnpjAlphanumeric(string cnpj)
        {
            if (string.IsNullOrWhiteSpace(cnpj))
                return false;

            // Deve ter entre 12 e 16 caracteres alphanumeric
            if (!Regex.IsMatch(cnpj, @"^[A-Z0-9]{12,16}$", RegexOptions.IgnoreCase))
                return false;

            // Alphanumeric válido (não há validação específica de dígito verificador)
            return true;
        }

        /// <summary>
        /// Valida um CNPJ em qualquer formato (numérico ou alphanumeric)
        /// </summary>
        public static bool Validar(string cnpj)
        {
            if (string.IsNullOrWhiteSpace(cnpj))
                return false;

            // Tenta validar como numérico primeiro
            if (Regex.IsMatch(cnpj, @"^\d{11,14}$|^\d{2}\.\d{3}\.\d{3}/\d{4}-\d{2}$"))
                return ValidarCnpjNumerico(cnpj);

            // Tenta validar como alphanumeric
            return ValidarCnpjAlphanumeric(cnpj);
        }

        /// <summary>
        /// Normaliza um CNPJ (remove caracteres especiais)
        /// </summary>
        public static string Normalizar(string cnpj)
        {
            if (string.IsNullOrWhiteSpace(cnpj))
                return cnpj;

            // Se for numérico, remove pontuação
            if (Regex.IsMatch(cnpj, @"^\d{2}\.\d{3}\.\d{3}/\d{4}-\d{2}$"))
                return Regex.Replace(cnpj, @"\D", "");

            // Se for alphanumeric, mantém como está (upper case)
            return cnpj.ToUpper();
        }
    }
}
