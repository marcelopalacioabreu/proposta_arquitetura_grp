using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
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
            // Validate against user store
            var u = await _usuarioServico.AutenticarAsync(req.Username, req.Password);
            if (u == null) return UnauthorizedError("Credenciais inválidas");

            var jwtKey = _config["Jwt:Key"] ?? "change_this_secret_for_prod";
            var jwtIssuer = _config["Jwt:Issuer"] ?? "Retaguarda";
            var keyBytes = Encoding.UTF8.GetBytes(jwtKey);
            if (keyBytes.Length < 32)
            {
                using var sha = SHA256.Create();
                keyBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(jwtKey));
            }
            var key = new SymmetricSecurityKey(keyBytes);
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, u.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, u.Id.ToString()),
                new Claim("name", u.Nome ?? u.Username),
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
                Response.Cookies.Append("access_token", tokenString, new CookieOptions { HttpOnly = true, SameSite = SameSiteMode.Lax });
                return OkMessage("Autenticado");
            }

            return OkData(new { token = tokenString });
        }

        public class LoginRequest
        {
            public string Username { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
            public bool AsCookie { get; set; } = true;
        }

        [HttpGet("me")]
        public IActionResult Me()
        {
            var reqUsuario = HttpContext.RequestServices.GetService(typeof(Retaguarda.Servicos.RequisicaoUsuario)) as Retaguarda.Servicos.RequisicaoUsuario;
            var u = reqUsuario?.Usuario;
            if (u == null) return UnauthorizedError("Não autenticado");

            return OkData(new { id = u.Id, nome = u.Nome, username = u.Username, email = u.Email });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // Remove authentication cookie if present
            if (Request.Cookies.ContainsKey("access_token"))
            {
                Response.Cookies.Delete("access_token");
            }
            return OkMessage("Desconectado");
        }
    }
}
