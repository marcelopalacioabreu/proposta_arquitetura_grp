using System.Text.Json;
using Retaguarda.Servicos;

namespace Retaguarda.Api.Middleware
{
    // Middleware que popula EscopoEmExecucao a partir de cookie ou header.
    public class AtuacaoMiddleware
    {
        private readonly RequestDelegate _next;

        public AtuacaoMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // We'll populate HttpContext.Items so lower layers (persistencia) can read without project refs
            long? orgId = null;
            long? unidadeId = null;
            long? setorId = null;

            // also try to populate EscopoEmExecucao in DI for other consumers
            var req = context.RequestServices.GetService(typeof(EscopoEmExecucao)) as EscopoEmExecucao;
            var reqUsuario = context.RequestServices.GetService(typeof(Retaguarda.Servicos.RequisicaoUsuario)) as Retaguarda.Servicos.RequisicaoUsuario;

            if (req != null)
            {
                string? raw = null;
                // Prefer cookie named 'atuacao'
                if (context.Request.Cookies.ContainsKey("atuacao")) raw = context.Request.Cookies["atuacao"];
                // Fallback header 'X-Atuacao'
                if (string.IsNullOrEmpty(raw) && context.Request.Headers.ContainsKey("X-Atuacao")) raw = context.Request.Headers["X-Atuacao"].ToString();

                if (!string.IsNullOrEmpty(raw))
                {
                    try
                    {
                        // If raw looks like JSON, parse it
                        if (raw.TrimStart().StartsWith("{"))
                        {
                            using var doc = JsonDocument.Parse(raw);
                            if (doc.RootElement.TryGetProperty("organizacaoId", out var o) && o.ValueKind == JsonValueKind.Number)
                            {
                                req.OrganizacaoId = o.GetInt64();
                                orgId = o.GetInt64();
                            }
                            if (doc.RootElement.TryGetProperty("organizacaoUnidadeId", out var u) && u.ValueKind == JsonValueKind.Number)
                            {
                                req.OrganizacaoUnidadeId = u.GetInt64();
                                unidadeId = u.GetInt64();
                            }
                            if (doc.RootElement.TryGetProperty("setorId", out var s) && s.ValueKind == JsonValueKind.Number)
                            {
                                req.SetorId = s.GetInt64();
                                setorId = s.GetInt64();
                            }
                        }
                        else
                        {
                            // Parse key=value;key2=value2 or urlencoded form
                            var parts = raw.Split(new[] {';', '&'}, StringSplitOptions.RemoveEmptyEntries);
                            foreach (var p in parts)
                            {
                                var kv = p.Split('=', 2);
                                if (kv.Length != 2) continue;
                                var k = kv[0].Trim().ToLowerInvariant();
                                var v = kv[1].Trim();
                                if (k == "organizacaoid" && long.TryParse(v, out var ov)) req.OrganizacaoId = ov;
                                else if ((k == "organizacaounidadeid" || k == "organizacaounidadeid") && long.TryParse(v, out var uv)) req.OrganizacaoUnidadeId = uv;
                                else if (k == "setorid" && long.TryParse(v, out var sv)) req.SetorId = sv;
                                // also fill local vars
                                if (k == "organizacaoid" && long.TryParse(v, out var ov2)) orgId = ov2;
                                if ((k == "organizacaounidadeid" || k == "organizacaounidadeid") && long.TryParse(v, out var uv2)) unidadeId = uv2;
                                if (k == "setorid" && long.TryParse(v, out var sv2)) setorId = sv2;
                            }
                        }
                    }
                    catch
                    {
                        // ignore parse errors
                    }
                }
            }

            // If no cookie/header provided, but we have authenticated user info, use their last-access defaults
            if (req != null && string.IsNullOrEmpty((context.Request.Cookies.ContainsKey("atuacao") ? context.Request.Cookies["atuacao"] : null)) && reqUsuario?.Usuario != null)
            {
                var u = reqUsuario.Usuario;
                if (u.UltimoAcessoOrganizacaoId.HasValue) { req.OrganizacaoId = u.UltimoAcessoOrganizacaoId; orgId = u.UltimoAcessoOrganizacaoId; }
                else if (u.OrganizacaoId.HasValue) { req.OrganizacaoId = u.OrganizacaoId; orgId = u.OrganizacaoId; }

                if (u.UltimoAcessoOrganizacaoUnidadeId.HasValue) { req.OrganizacaoUnidadeId = u.UltimoAcessoOrganizacaoUnidadeId; unidadeId = u.UltimoAcessoOrganizacaoUnidadeId; }
                else if (u.OrganizacaoUnidadeId.HasValue) { req.OrganizacaoUnidadeId = u.OrganizacaoUnidadeId; unidadeId = u.OrganizacaoUnidadeId; }

                if (u.UltimoAcessoSetorId.HasValue) { req.SetorId = u.UltimoAcessoSetorId; setorId = u.UltimoAcessoSetorId; }
                else if (u.SetorId.HasValue) { req.SetorId = u.SetorId; setorId = u.SetorId; }
            }

            // persist the parsed ids into HttpContext.Items for lower layers to consume without compile-time refs
            if (orgId.HasValue) context.Items["escopo.organizacaoId"] = orgId.Value;
            if (unidadeId.HasValue) context.Items["escopo.organizacaoUnidadeId"] = unidadeId.Value;
            if (setorId.HasValue) context.Items["escopo.setorId"] = setorId.Value;

            await _next(context);
        }
    }
}
