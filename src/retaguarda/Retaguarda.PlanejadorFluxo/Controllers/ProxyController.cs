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
            // If the request is for the Studio and Elsa is hosted in the same app,
            // redirect the browser to the local /studio fallback page so static assets
            // and the Blazor WASM host are served directly by this app instead of proxying.
            if (!string.IsNullOrEmpty(path) && path.TrimStart('/').StartsWith("studio", System.StringComparison.OrdinalIgnoreCase))
            {
                return Redirect("/studio" + Request.QueryString);
            }

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
