using System.ComponentModel;
using Elsa.Extensions;
using Elsa.Workflows;
using Elsa.Workflows.Attributes;
using Elsa.Workflows.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Retaguarda.Repositorios.Interfaces;

namespace Retaguarda.PlanejadorFluxo.Atividades;

/// <summary>
/// Atividade de exemplo: registra um evento de fluxo com contexto multilocatário.
/// Demonstra como injetar serviços da aplicação principal em atividades Elsa.
/// </summary>
[Activity("Retaguarda", "Utilitários", "Registra um evento de fluxo vinculado ao locatário atual.")]
[DisplayName("Registrar Evento")]
public class RegistrarEventoAtividade : Activity
{
    [Input(Description = "Descrição do evento a ser registrado.")]
    public Input<string> Descricao { get; set; } = default!;

    [Input(Description = "Categoria do evento (ex.: Iniciado, Concluído, Erro).")]
    public Input<string> Categoria { get; set; } = new("Geral");

    [Output(Description = "Identificador gerado para o evento.")]
    public Output<string> EventoId { get; set; } = default!;

    protected override async ValueTask ExecuteAsync(ActivityExecutionContext context)
    {
        var logger = context.GetRequiredService<ILogger<RegistrarEventoAtividade>>();
        var httpContextAccessor = context.GetRequiredService<IHttpContextAccessor>();
        var organizacaoRepo = context.GetService<IOrganizacaoRepositorio>();

        var descricao = context.Get(Descricao);
        var categoria = context.Get(Categoria) ?? "Geral";

        // Lê o contexto multilocatário do HTTP request corrente
        var httpContext = httpContextAccessor.HttpContext;
        var organizacaoId = httpContext?.User.FindFirst("organizacaoId")?.Value ?? "N/A";
        var unidadeId     = httpContext?.User.FindFirst("organizacaoUnidadeId")?.Value ?? "N/A";
        var setorId       = httpContext?.User.FindFirst("setorId")?.Value ?? "N/A";

        var eventoId = Guid.NewGuid().ToString("N")[..10].ToUpper();

        // Consulta o nome da organização via repositório da aplicação principal
        var nomeOrg = organizacaoId;
        if (organizacaoRepo != null && long.TryParse(organizacaoId, out var orgIdLong))
        {
            var org = await organizacaoRepo.ObterPorIdAsync(orgIdLong);
            if (org != null) nomeOrg = org.Nome;
        }

        logger.LogInformation(
            "[Fluxo:{Categoria}] [{EventoId}] Org={NomeOrg} Unidade={UnidadeId} Setor={SetorId} — {Descricao}",
            categoria, eventoId, nomeOrg, unidadeId, setorId, descricao);

        context.Set(EventoId, eventoId);

        await context.CompleteActivityAsync();
    }
}
