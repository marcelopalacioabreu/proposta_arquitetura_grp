using Microsoft.AspNetCore.Mvc;
using Retaguarda.Api.Models;
using Retaguarda.Api.Utils;

namespace Retaguarda.Api.Controllers
{
    public class BaseController : ControllerBase
    {
        protected IActionResult OkData(object? data, string? mensagem = null)
        {
            return Ok(EnvelopeResult.SuccessData(data, mensagem));
        }

        protected IActionResult OkList(object items, int total, int page, int pageSize, string? mensagem = null)
        {
            return Ok(EnvelopeResult.SuccessList(items, total, page, pageSize, mensagem));
        }

        protected IActionResult OkMessage(string mensagem)
        {
            return Ok(EnvelopeResult.SuccessMessage(mensagem));
        }

        protected IActionResult CreatedDataAtAction(string actionName, object? routeValues, object? data, string? mensagem = null)
        {
            return CreatedAtAction(actionName, routeValues, EnvelopeResult.SuccessData(data, mensagem));
        }

        protected IActionResult BadRequestModelState()
        {
            var detalhes = ModelStateExtensions.ToErrorDictionary(ModelState);
            return BadRequest(EnvelopeResult.Error("Requisição inválida", detalhes));
        }

        protected IActionResult NotFoundError(string mensagem)
        {
            return NotFound(EnvelopeResult.Error(mensagem));
        }

        protected IActionResult UnauthorizedError(string mensagem)
        {
            return Unauthorized(EnvelopeResult.Error(mensagem));
        }

        protected IActionResult Error(string mensagem, int status = 500, object? detalhes = null)
        {
            return StatusCode(status, EnvelopeResult.Error(mensagem, detalhes));
        }
    }
}
