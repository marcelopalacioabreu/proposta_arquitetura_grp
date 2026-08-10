using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;

namespace ElsaStudio;

/// <summary>
/// Determines auth state by calling /identity/me on the PlanejadorFluxo server.
/// The browser sends the HttpOnly access_token cookie automatically (same-origin proxy).
/// No token is ever stored in JavaScript-accessible storage.
/// </summary>
public class CookieAuthStateProvider : AuthenticationStateProvider
{
    private readonly HttpClient _http;

    public CookieAuthStateProvider(HttpClient http) => _http = http;

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var response = await _http.GetAsync("/identity/me");
            if (response.IsSuccessStatusCode)
            {
                var body   = await response.Content.ReadAsStringAsync();
                using var doc  = JsonDocument.Parse(body);
                var name   = doc.RootElement.TryGetProperty("name", out var n) ? n.GetString() : "Usuário";
                var claims = new[] { new Claim(ClaimTypes.Name, name ?? "Usuário") };
                var id     = new ClaimsIdentity(claims, "Cookie");
                return new AuthenticationState(new ClaimsPrincipal(id));
            }
        }
        catch { }

        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }
}
