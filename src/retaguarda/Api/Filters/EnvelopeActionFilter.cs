using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Retaguarda.Api.Models;

namespace Retaguarda.Api.Filters
{
    public class EnvelopeActionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // nothing to do before
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // If an exception occurred, let existing exception handling pipeline run (or convert later)
            if (context.Exception != null)
            {
                return;
            }

            // Wrap ObjectResult values into EnvelopeResult unless already wrapped
            if (context.Result is ObjectResult objResult)
            {
                if (objResult.Value is EnvelopeResult)
                {
                    return; // already wrapped
                }

                var status = objResult.StatusCode ?? 200;

                if (status >= 400)
                {
                    // For error status codes, wrap as Error with provided value (or message)
                    var detalhes = objResult.Value;
                    var mensagem = detalhes is string s ? s : null;
                    context.Result = new ObjectResult(EnvelopeResult.Error(mensagem ?? "Erro", detalhes)) { StatusCode = status };
                }
                else
                {
                    context.Result = new ObjectResult(EnvelopeResult.SuccessData(objResult.Value)) { StatusCode = status };
                }

                return;
            }

            // If the action returned plain StatusCodeResult (no body), wrap into an envelope message
            if (context.Result is StatusCodeResult statusOnly)
            {
                var status = statusOnly.StatusCode;
                if (status >= 400)
                {
                    context.Result = new ObjectResult(EnvelopeResult.Error($"Status {status}")) { StatusCode = status };
                }
                else
                {
                    context.Result = new ObjectResult(EnvelopeResult.SuccessMessage("OK")) { StatusCode = status };
                }
            }
        }
    }
}
