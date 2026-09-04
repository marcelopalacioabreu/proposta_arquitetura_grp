using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Retaguarda.Api.Models;
using Retaguarda.DTO.Exceptions;

namespace Retaguarda.Api.Filters
{
    /// <summary>
    /// Converte ValidationException (incluindo quando embrulhada em AggregateException pelo Task.Result) em 400.
    /// </summary>
    public class ValidationExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var ex = context.Exception;

            // Task.Result embrulha exceções em AggregateException; desembrulha aqui
            if (ex is AggregateException agg && agg.InnerException is not null)
                ex = agg.InnerException;

            if (ex is not ValidationException ve) return;

            context.Result = new BadRequestObjectResult(
                EnvelopeResult.Error(ve.Mensagem ?? "Validação falhou", ve.Errors));
            context.ExceptionHandled = true;
        }
    }
}
