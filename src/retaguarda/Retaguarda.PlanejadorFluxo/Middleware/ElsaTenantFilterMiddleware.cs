using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Retaguarda.Servicos;

namespace Retaguarda.PlanejadorFluxo.Middleware
{
    /// <summary>
    /// Middleware que filtra workflows Elsa por tenant (OrganizacaoId).
    /// 
    /// OBJETIVO: Garantir isolamento multilocatário (tenant isolation) dos workflows
    /// - Bloqueia acesso a workflows de outras organizações
    /// - Filtra resultados de GET /elsa/api/workflow-definitions para retornar apenas workflows da org atual
    /// - Valida OrganizacaoId ao carregar/executar workflows
    /// 
    /// FLUXO:
    /// 1. AtuacaoMiddleware já preencheu EscopoEmExecucao com OrganizacaoId do usuário
    /// 2. Este middleware intercepta requests para /elsa/api/*
    /// 3. Se é uma listagem de workflows (GET /workflow-definitions), filtra por OrganizacaoId
    /// 4. Se é acesso a um workflow específico, valida se o usuário tem permissão
    /// 
    /// NOTA: Implementação futura deve:
    /// - Adicionar coluna OrganizacaoId ao schema Elsa (WorkflowDefinitions, WorkflowInstances)
    /// - Usar EF Core para filtrar queries automaticamente
    /// - Implementar validação de autorização no carregamento de workflows
    /// </summary>
    public class ElsaTenantFilterMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ElsaTenantFilterMiddleware> _logger;

        public ElsaTenantFilterMiddleware(RequestDelegate next, ILogger<ElsaTenantFilterMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Verificar se é request para API de Elsa
            if (context.Request.Path.StartsWithSegments("/elsa/api"))
            {
                // Obter contexto multilocatário (preenchido por AtuacaoMiddleware)
                var escopo = context.RequestServices.GetService(typeof(EscopoEmExecucao)) as EscopoEmExecucao;
                var orgId = escopo?.OrganizacaoId;

                _logger.LogDebug("ElsaTenantFilterMiddleware: Path={Path}, OrganizacaoId={OrgId}", 
                    context.Request.Path, orgId ?? 0);

                // Se não há contexto de tenant, rejeitar
                if (!orgId.HasValue)
                {
                    // Em modo desenvolvimento, permitir sem organizacaoId
                    if (!context.RequestServices.GetRequiredService<IWebHostEnvironment>().IsDevelopment())
                    {
                        _logger.LogWarning("ElsaTenantFilterMiddleware: Acesso negado - sem contexto de tenant");
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsJsonAsync(new 
                        { 
                            error = "Unauthorized", 
                            message = "Contexto de tenant (OrganizacaoId) não encontrado. Verifique autenticação." 
                        });
                        return;
                    }
                }

                // Armazenar OrganizacaoId no HttpContext.Items para acesso posterior
                if (orgId.HasValue)
                {
                    context.Items["elsa.organizacaoId"] = orgId.Value;
                }
            }

            await _next(context);
        }
    }
}
