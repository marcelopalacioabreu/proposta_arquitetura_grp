using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Elsa.Workflows;
using Elsa.Workflows.Models;
using Microsoft.AspNetCore.Http;

namespace Retaguarda.PlanejadorFluxo.Atividades
{
    /// <summary>
    /// Base helper para atividades que precisam resolver o contexto multilocatário (tenant).
    /// Estratégia de resolução (ordem):
    /// 1. Variáveis do workflow (`OrganizacaoId`, `OrganizacaoUnidadeId`, `SetorId`)
    /// 2. Claims do `HttpContext.User` (quando a execução ocorre em contexto HTTP)
    /// 3. Valores nulos quando não encontrados — atividades devem tratar a ausência.
    /// </summary>
    public abstract class TenantAwareActivity : Activity
    {
        protected (long? OrganizacaoId, long? OrganizacaoUnidadeId, long? SetorId) ResolveTenant(ActivityExecutionContext context)
        {
            long? ParseLong(object? v)
            {
                if (v == null) return null;
                if (v is long l) return l;
                if (v is int i) return Convert.ToInt64(i);
                if (long.TryParse(v.ToString(), out var parsed)) return parsed;
                return null;
            }

            // 1) From workflow variables (use reflection because ActivityExecutionContext shape
            // may vary across Elsa versions). Try to read WorkflowInstance.Variables as a
            // dictionary-like object.
            object? orgVar = null;
            object? unidadeVar = null;
            object? setorVar = null;

            try
            {
                var wfProp = context.GetType().GetProperty("WorkflowInstance");
                if (wfProp != null)
                {
                    var wf = wfProp.GetValue(context);
                    if (wf != null)
                    {
                        var varsProp = wf.GetType().GetProperty("Variables");
                        if (varsProp != null)
                        {
                            var vars = varsProp.GetValue(wf);
                            if (vars != null)
                            {
                                // Try IDictionary-style access
                                if (vars is System.Collections.IDictionary dict)
                                {
                                    if (dict.Contains("OrganizacaoId")) orgVar = dict["OrganizacaoId"];
                                    if (dict.Contains("OrganizacaoUnidadeId")) unidadeVar = dict["OrganizacaoUnidadeId"];
                                    if (dict.Contains("SetorId")) setorVar = dict["SetorId"];
                                }
                                else
                                {
                                    // Try TryGetValue<string,object> or GetValueOrDefault via reflection
                                    var tryGet = vars.GetType().GetMethod("TryGetValue");
                                    if (tryGet != null)
                                    {
                                        var args = new object?[] { "OrganizacaoId", null };
                                        if ((bool)tryGet.Invoke(vars, args)!) orgVar = args[1];
                                        args = new object?[] { "OrganizacaoUnidadeId", null };
                                        if ((bool)tryGet.Invoke(vars, args)!) unidadeVar = args[1];
                                        args = new object?[] { "SetorId", null };
                                        if ((bool)tryGet.Invoke(vars, args)!) setorVar = args[1];
                                    }
                                    else
                                    {
                                        var getVal = vars.GetType().GetMethod("GetValueOrDefault", new[] { typeof(string) });
                                        if (getVal != null)
                                        {
                                            orgVar = getVal.Invoke(vars, new object[] { "OrganizacaoId" });
                                            unidadeVar = getVal.Invoke(vars, new object[] { "OrganizacaoUnidadeId" });
                                            setorVar = getVal.Invoke(vars, new object[] { "SetorId" });
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                // ignore reflection failures and fallback to claims
            }

            var orgId = ParseLong(orgVar);
            var unidadeId = ParseLong(unidadeVar);
            var setorId = ParseLong(setorVar);

            if (orgId.HasValue || unidadeId.HasValue || setorId.HasValue)
                return (orgId, unidadeId, setorId);

            // 2) From HttpContext claims (if available)
            var httpAccessor = context.GetService<IHttpContextAccessor>();
            var httpContext = httpAccessor?.HttpContext;
            if (httpContext != null)
            {
                var user = httpContext.User;
                long? claimOrNull(string name)
                {
                    var c = user?.FindFirst(name)?.Value;
                    return long.TryParse(c, out var x) ? x : null;
                }

                orgId = orgId ?? claimOrNull("organizacaoId");
                unidadeId = unidadeId ?? claimOrNull("organizacaoUnidadeId");
                setorId = setorId ?? claimOrNull("setorId");
            }

            return (orgId, unidadeId, setorId);
        }
    }
}
