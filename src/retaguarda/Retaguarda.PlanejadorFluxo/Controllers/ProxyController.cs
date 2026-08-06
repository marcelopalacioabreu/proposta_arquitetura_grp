using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Retaguarda.PlanejadorFluxo.Controllers
{
    [ApiController]
    [Route("planejadorDeFluxo/{**path}")]
    public class ProxyController : ControllerBase
    {
        private readonly IHttpClientFactory _httpFactory;

        public ProxyController(IHttpClientFactory httpFactory)
        {
            _httpFactory = httpFactory;
        }

        [HttpGet]
        [HttpPost]
        [HttpPut]
        [HttpDelete]
        public async Task<IActionResult> Proxy(string path)
        {
            var client = _httpFactory.CreateClient("Elsa");

            var targetUri = path ?? string.Empty;
            var requestMessage = new HttpRequestMessage(new HttpMethod(Request.Method), targetUri + Request.QueryString);

            // Copy headers
            foreach (var header in Request.Headers)
            {
                if (!requestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray()))
                {
                    // ignore
                }
            }

            // Forward body
            if (Request.ContentLength > 0)
            {
                requestMessage.Content = new StreamContent(Request.Body);
                if (Request.ContentType != null)
                    requestMessage.Content.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse(Request.ContentType);
            }

            // Forward cookies: copy access_token cookie if present
            if (Request.Cookies.ContainsKey("access_token"))
            {
                requestMessage.Headers.Add("Cookie", $"access_token={Request.Cookies["access_token"]}");
            }

            var response = await client.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead);

            var content = await response.Content.ReadAsStreamAsync();
            Response.StatusCode = (int)response.StatusCode;
            return new FileStreamResult(content, response.Content.Headers.ContentType?.ToString() ?? "application/octet-stream");
        }
    }
}
