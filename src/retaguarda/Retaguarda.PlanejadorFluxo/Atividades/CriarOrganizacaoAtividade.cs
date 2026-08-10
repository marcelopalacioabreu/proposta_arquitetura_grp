using System.Threading.Tasks;
using Elsa.Workflows;
using Elsa.Workflows.Attributes;
using Elsa.Workflows.Models;
using Microsoft.Extensions.Logging;
using Retaguarda.Servicos.Interfaces;

namespace Retaguarda.PlanejadorFluxo.Atividades
{
    [Activity("Retaguarda", "Organização", "Cria uma nova organização usando o serviço existente.")]
    public class CriarOrganizacaoAtividade : TenantAwareActivity
    {
        [Input(Description = "Nome da organização a ser criada.")]
        public Input<string> Nome { get; set; } = default!;

        [Output(Description = "Id da organização criada.")]
        public Output<long> OrganizacaoId { get; set; } = default!;

        protected override async ValueTask ExecuteAsync(ActivityExecutionContext context)
        {
            var logger = context.GetRequiredService<ILogger<CriarOrganizacaoAtividade>>();
            var nome = context.Get(Nome) ?? "Nova Organização";

            // Resolve tenant context (ex.: pode influenciar regras de criação, auditoria, etc.)
            var (orgId, unidadeId, setorId) = ResolveTenant(context);

            // Usa o serviço de organização existente para criar a entidade
            var orgService = context.GetService<IOrganizacaoServico>();
            if (orgService == null)
            {
                logger.LogWarning("IOrganizacaoServico não está registrado — atividade não pode criar organização.");
                await context.CompleteActivityAsync();
                return;
            }

            var criada = await orgService.CriarAsync(nome);

            context.Set(OrganizacaoId, criada.Id);
            logger.LogInformation("Organização criada via fluxo: Id={Id} Nome={Nome} (OrgContext={OrgContext})", criada.Id, criada.Nome, orgId?.ToString() ?? "-" );

            await context.CompleteActivityAsync();
        }
    }
}
