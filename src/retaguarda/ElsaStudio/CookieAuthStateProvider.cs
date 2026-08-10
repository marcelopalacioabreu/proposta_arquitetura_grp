using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;

namespace ElsaStudio;

/// <summary>
/// Determines auth state by calling /identity/token on the PlanejadorFluxo server.
/// The server reads the HttpOnly access_token cookie and returns the JWT + user info.
/// No token is ever stored in JavaScript-accessible storage.
/// </summary>
public class CookieAuthStateProvider : AuthenticationStateProvider
{
    private readonly IHttpClientFactory _factory;

    public CookieAuthStateProvider(IHttpClientFactory factory) => _factory = factory;

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var http = _factory.CreateClient("identity");
            var response = await http.GetAsync("/identity/token");
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
