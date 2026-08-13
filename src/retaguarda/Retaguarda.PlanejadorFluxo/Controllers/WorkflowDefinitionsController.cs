using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Retaguarda.PlanejadorFluxo.Controllers
{
    /// <summary>
    /// Endpoint público para listar definições de workflow
    /// Atualmente retorna dados mock - será substituído por queries reais do Elsa
    /// </summary>
    [ApiController]
    [Route("elsa/api")]
    public class WorkflowDefinitionsController : ControllerBase
    {
        [HttpGet("workflow-definitions")]
        [Authorize]  // Requer autenticação JWT válida
        public IActionResult ListWorkflowDefinitions()
        {
            try
            {
                // TODO: Replace with real Elsa query
                // var workflows = await _elsaStore.ListPublishedWorkflows();
                
                // Mock data for now
                var items = new List<object>
                {
                    new
                    {
                        id = "wf-001",
                        definitionId = "workflow-1",
                        name = "Workflow de Aprovação",
                        description = "Processa aprovações de documentos",
                        version = 1,
                        createdAt = DateTime.UtcNow.AddDays(-10),
                        isPublished = true
                    },
                    new
                    {
                        id = "wf-002",
                        definitionId = "workflow-2",
                        name = "Workflow de Notificação",
                        description = "Envia notificações para usuários",
                        version = 1,
                        createdAt = DateTime.UtcNow.AddDays(-5),
                        isPublished = true
                    }
                };

                return Ok(new
                {
                    total = items.Count,
                    items = items
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    title = "Error retrieving workflow definitions",
                    status = 500,
                    detail = ex.Message
                });
            }
        }
    }
}
