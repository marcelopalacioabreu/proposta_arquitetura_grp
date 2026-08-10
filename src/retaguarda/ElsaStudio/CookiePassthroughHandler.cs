using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ElsaStudio;

/// <summary>
/// HTTP handler that passes requests through unmodified.
/// The browser automatically includes the HttpOnly access_token cookie for same-origin requests,
/// so no Authorization header manipulation is needed here.
/// </summary>
public class CookiePassthroughHandler : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
        => base.SendAsync(request, cancellationToken);
}
