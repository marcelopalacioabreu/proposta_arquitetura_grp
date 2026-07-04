using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;

namespace Retaguarda.Api.Utils
{
    public static class ModelStateExtensions
    {
        public static object ToErrorDictionary(ModelStateDictionary modelState)
        {
            var dict = new Dictionary<string, string[]>();
            foreach (var kv in modelState)
            {
                var key = kv.Key;
                var errors = kv.Value?.Errors;
                if (errors != null && errors.Count > 0)
                {
                    var msgs = new List<string>();
                    foreach (var e in errors)
                    {
                        if (!string.IsNullOrEmpty(e.ErrorMessage)) msgs.Add(e.ErrorMessage);
                        else if (e.Exception != null) msgs.Add(e.Exception.Message);
                    }
                    dict[key] = msgs.ToArray();
                }
            }
            return dict;
        }
    }
}
