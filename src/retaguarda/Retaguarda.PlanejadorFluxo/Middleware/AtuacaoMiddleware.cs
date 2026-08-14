using System.Text.Json;
using Retaguarda.Servicos;

namespace Retaguarda.PlanejadorFluxo.Middleware
{
    /// <summary>
    /// Middleware que extrai e popula contexto multilocatário (tenant) a partir de:
    /// 1. Cookie "atuacao" (prioridade)
    /// 2. Header "X-Atuacao" (fallback)
    /// 3. Dados do usuário autenticado (UltimoAcesso*)
    /// 
    /// Preenche EscopoEmExecucao e HttpContext.Items para acesso em camadas inferiores.
    /// </summary>
    public class AtuacaoMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AtuacaoMiddleware> _logger;

        public AtuacaoMiddleware(RequestDelegate next, ILogger<AtuacaoMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Armazenar contexto de tenant em HttpContext.Items para camadas inferiores
            long? orgId = null;
            long? unidadeId = null;
            long? setorId = null;

            // Obter EscopoEmExecucao (injetado via DI) e RequisicaoUsuario
            var escopo = context.RequestServices.GetService(typeof(EscopoEmExecucao)) as EscopoEmExecucao;
            var reqUsuario = context.RequestServices.GetService(typeof(Retaguarda.Servicos.RequisicaoUsuario)) as Retaguarda.Servicos.RequisicaoUsuario;

            if (escopo != null)
            {
                string? raw = null;
                
                // Estratégia 1: Verificar cookie "atuacao" (prioridade)
                if (context.Request.Cookies.ContainsKey("atuacao")) 
                    raw = context.Request.Cookies["atuacao"];
                
                // Estratégia 2: Fallback para header "X-Atuacao"
                if (string.IsNullOrEmpty(raw) && context.Request.Headers.ContainsKey("X-Atuacao")) 
                    raw = context.Request.Headers["X-Atuacao"].ToString();

                // Parse JSON ou key=value
                if (!string.IsNullOrEmpty(raw))
                {
                    try
                    {
                        if (raw.TrimStart().StartsWith("{"))
                        {
                            // JSON: {"organizacaoId": 123, "organizacaoUnidadeId": 456, "setorId": 789}
                            using var doc = JsonDocument.Parse(raw);
                            if (doc.RootElement.TryGetProperty("organizacaoId", out var o) && o.ValueKind == JsonValueKind.Number)
                            {
                                escopo.OrganizacaoId = o.GetInt64();
                                orgId = o.GetInt64();
                            }
                            if (doc.RootElement.TryGetProperty("organizacaoUnidadeId", out var u) && u.ValueKind == JsonValueKind.Number)
                            {
                                escopo.OrganizacaoUnidadeId = u.GetInt64();
                                unidadeId = u.GetInt64();
                            }
                            if (doc.RootElement.TryGetProperty("setorId", out var s) && s.ValueKind == JsonValueKind.Number)
                            {
                                escopo.SetorId = s.GetInt64();
                                setorId = s.GetInt64();
                            }
                        }
                        else
                        {
                            // key=value format: organizacaoId=1;organizacaoUnidadeId=2
                            var parts = raw.Split(new[] {';', '&'}, StringSplitOptions.RemoveEmptyEntries);
                            foreach (var p in parts)
                            {
                                var kv = p.Split('=', 2);
                                if (kv.Length != 2) continue;
                                var k = kv[0].Trim().ToLowerInvariant();
                                var v = kv[1].Trim();
                                if (k == "organizacaoid" && long.TryParse(v, out var ov)) 
                                {
                                    escopo.OrganizacaoId = ov;
                                    orgId = ov;
                                }
                                else if (k == "organizacaounidadeid" && long.TryParse(v, out var uv)) 
                                {
                                    escopo.OrganizacaoUnidadeId = uv;
                                    unidadeId = uv;
                                }
                                else if (k == "setorid" && long.TryParse(v, out var sv)) 
                                {
                                    escopo.SetorId = sv;
                                    setorId = sv;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning("AtuacaoMiddleware: Erro ao parsear contexto: {Message}", ex.Message);
                    }
                }

                // Fallback: Se nenhum contexto foi fornecido, usar dados do usuário autenticado
                if (string.IsNullOrEmpty(raw) && reqUsuario?.Usuario != null)
                {
                    var usuario = reqUsuario.Usuario;
                    
                    // Preferir UltimoAcesso* para manter histórico do que o usuário acessava
                    if (usuario.UltimoAcessoOrganizacaoId.HasValue)
                    {
                        escopo.OrganizacaoId = usuario.UltimoAcessoOrganizacaoId;
                        orgId = usuario.UltimoAcessoOrganizacaoId;
                    }
                    else if (usuario.OrganizacaoId.HasValue)
                    {
                        escopo.OrganizacaoId = usuario.OrganizacaoId;
                        orgId = usuario.OrganizacaoId;
                    }

                    if (usuario.UltimoAcessoOrganizacaoUnidadeId.HasValue)
                    {
                        escopo.OrganizacaoUnidadeId = usuario.UltimoAcessoOrganizacaoUnidadeId;
                        unidadeId = usuario.UltimoAcessoOrganizacaoUnidadeId;
                    }
                    else if (usuario.OrganizacaoUnidadeId.HasValue)
                    {
                        escopo.OrganizacaoUnidadeId = usuario.OrganizacaoUnidadeId;
                        unidadeId = usuario.OrganizacaoUnidadeId;
                    }

                    if (usuario.UltimoAcessoSetorId.HasValue)
                    {
                        escopo.SetorId = usuario.UltimoAcessoSetorId;
                        setorId = usuario.UltimoAcessoSetorId;
                    }
                    else if (usuario.SetorId.HasValue)
                    {
                        escopo.SetorId = usuario.SetorId;
                        setorId = usuario.SetorId;
                    }

                    _logger.LogDebug("AtuacaoMiddleware: Contexto carregado do usuário {UserId}: OrgId={OrgId}", 
                        usuario.Id, orgId);
                }
            }

            // Persistir IDs no HttpContext.Items para acesso em camadas inferiores sem refs de projeto
            if (orgId.HasValue) context.Items["escopo.organizacaoId"] = orgId.Value;
            if (unidadeId.HasValue) context.Items["escopo.organizacaoUnidadeId"] = unidadeId.Value;
            if (setorId.HasValue) context.Items["escopo.setorId"] = setorId.Value;

            _logger.LogDebug("AtuacaoMiddleware: Contexto preenchido - OrgId={OrgId}, UnidadeId={UnidadeId}, SetorId={SetorId}", 
                orgId, unidadeId, setorId);

            await _next(context);
        }
    }
}
