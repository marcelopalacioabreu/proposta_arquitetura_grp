using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ElsaStudio;

/// <summary>
/// Fetches the JWT from /identity/token (server reads the HttpOnly cookie) and adds it as
/// Authorization: Bearer in every outgoing request. Token is kept in memory only — never
/// written to localStorage or any JS-accessible storage.
/// </summary>
public class CookieTokenHandler : DelegatingHandler
{
    private readonly IHttpClientFactory _factory;
    private string? _cachedToken;

    public CookieTokenHandler(IHttpClientFactory factory) => _factory = factory;

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken ct)
    {
        _cachedToken ??= await FetchTokenAsync(ct);

        if (!string.IsNullOrEmpty(_cachedToken))
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _cachedToken);

        return await base.SendAsync(request, ct);
    }

    private async Task<string?> FetchTokenAsync(CancellationToken ct)
    {
        try
        {
            var http = _factory.CreateClient("identity");
            var resp = await http.GetAsync("/identity/token", ct);
            if (!resp.IsSuccessStatusCode) return null;
            using var doc = JsonDocument.Parse(await resp.Content.ReadAsStringAsync(ct));
            return doc.RootElement.TryGetProperty("token", out var t) ? t.GetString() : null;
        }
        catch { return null; }
    }
}
