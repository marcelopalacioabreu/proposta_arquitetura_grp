using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Retaguarda.Api.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : BaseController
    {
        private readonly IConfiguration _config;
        private readonly Retaguarda.Servicos.Interfaces.IUsuarioServico _usuarioServico;

        public AuthController(IConfiguration config, Retaguarda.Servicos.Interfaces.IUsuarioServico usuarioServico)
        {
            _config = config;
            _usuarioServico = usuarioServico;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            // Validate email + password against user store
            if (string.IsNullOrWhiteSpace(req.Email) || string.IsNullOrWhiteSpace(req.Password))
                return UnauthorizedError("Email e senha são obrigatórios");

            var db = HttpContext.RequestServices.GetService(typeof(Retaguarda.Persistencia.IApplicationDbContext)) as Retaguarda.Persistencia.IApplicationDbContext;
            if (db == null) return Error("Serviço de banco de dados indisponível");

            var u = await db.Usuarios.FirstOrDefaultAsync(x => x.Email == req.Email && x.Ativo);
            if (u == null || !Retaguarda.Servicos.Util.PasswordHasher.Verify(req.Password, u.SenhaHash))
                return UnauthorizedError("Credenciais inválidas");

            var jwtKey = _config["Jwt:Key"] ?? "change_this_secret_for_prod";
            var jwtIssuer = _config["Jwt:Issuer"] ?? "Retaguarda";
            var keyBytes = Encoding.UTF8.GetBytes(jwtKey);
            if (keyBytes.Length < 32)
            {
                using var sha = SHA256.Create();
                keyBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(jwtKey));
            }
            var key = new SymmetricSecurityKey(keyBytes);
            key.KeyId = "default-key";
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, u.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, u.Id.ToString()),
                new Claim("name", u.Nome),
                new Claim(ClaimTypes.Email, u.Email),
                new Claim(ClaimTypes.Role, "user"),
                new Claim("permissions", "*"),
            };

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: null,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: creds);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            if (req.AsCookie)
            {
                var cookieName = _config["Jwt:Cookie:Name"] ?? "access_token";
                var cookieDomain = _config["Jwt:Cookie:Domain"];
                var sameSiteCfg = _config["Jwt:Cookie:SameSite"] ?? "Lax";
                var secureCfg = _config["Jwt:Cookie:Secure"];

                SameSiteMode sameSite = SameSiteMode.Lax;
                if (Enum.TryParse<SameSiteMode>(sameSiteCfg, true, out var parsed)) sameSite = parsed;

                var secure = false;
                if (!string.IsNullOrEmpty(secureCfg) && bool.TryParse(secureCfg, out var parsedSecure)) secure = parsedSecure;

                var cookieOptions = new CookieOptions { HttpOnly = true, SameSite = sameSite, Secure = secure };
                if (!string.IsNullOrEmpty(cookieDomain)) cookieOptions.Domain = cookieDomain;

                Response.Cookies.Append(cookieName, tokenString, cookieOptions);
                return OkData(null);
            }

            return OkData(new { token = tokenString });
        }

        public class LoginRequest
        {
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
            public bool AsCookie { get; set; } = true;
        }

        [HttpGet("me")]
        public IActionResult Me()
        {
            var reqUsuario = HttpContext.RequestServices.GetService(typeof(Retaguarda.Servicos.RequisicaoUsuario)) as Retaguarda.Servicos.RequisicaoUsuario;
            var u = reqUsuario?.Usuario;
            if (u == null)
            {
                // Allow anonymous callers to probe /auth/me without triggering 401.
                // This avoids noisy global error handling for UI components (e.g., navbar) that
                // always call this endpoint. Do NOT include any user details when unauthenticated.
                return OkData(null);
            }

            // Include permissions summary for the UI
            var db = HttpContext.RequestServices.GetService(typeof(Retaguarda.Persistencia.IApplicationDbContext)) as Retaguarda.Persistencia.IApplicationDbContext;
            var permissoes = new System.Collections.Generic.List<string>();
            var isAdmin = false;
            if (db != null)
            {
                var pu = db.PerfilUsuarios.Where(x => x.UsuarioId == u.Id).Select(x => x.PerfilId).ToList();
                if (pu.Any())
                {
                    var perfis = db.Perfis.Where(p => pu.Contains(p.Id)).Include(p => p.Permissoes).ToList();
                    isAdmin = perfis.Any(p => p.AdministradorDoSistema);
                    permissoes = perfis.SelectMany(p => p.Permissoes.Select(pp => pp.Chave)).Distinct().ToList();
                }
            }

            return OkData(new { id = u.Id, nome = u.Nome, email = u.Email, administrador = isAdmin, permissoes });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // Remove authentication cookie if present
            if (Request.Cookies.ContainsKey("access_token"))
            {
                Response.Cookies.Delete("access_token");
            }
            //return OkMessage("Desconectado");
            return OkData(null);
        }
    }
}
