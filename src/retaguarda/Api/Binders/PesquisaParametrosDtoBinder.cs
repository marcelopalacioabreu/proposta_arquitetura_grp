using Microsoft.AspNetCore.Mvc.ModelBinding;
using Retaguarda.DTO.Parametros;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Retaguarda.Api.Binders
{
    public class PesquisaParametrosDtoBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType != typeof(PesquisaParametrosDto))
                return Task.CompletedTask;

            var parametros = new PesquisaParametrosDto();
            var query = bindingContext.HttpContext.Request.Query;

            // Parâmetros conhecidos
            var parametrosConhecidos = new[]
            {
                "page", "pagina", "pageSize", "tamanhoPagina",
                "sortField", "sortDir", "nome",
                "campo", "operador", "valor", "valor_de", "valor_ate",
                "inativo"
            };

            // Extrair paginação
            if (query.TryGetValue("page", out var pageVal) && int.TryParse(pageVal, out var p))
                parametros.Pagina = p;
            else if (query.TryGetValue("pagina", out var paginaVal) && int.TryParse(paginaVal, out var pg))
                parametros.Pagina = pg;

            if (query.TryGetValue("pageSize", out var psVal) && int.TryParse(psVal, out var ps))
                parametros.TamanhoPagina = ps;
            else if (query.TryGetValue("tamanhoPagina", out var tpVal) && int.TryParse(tpVal, out var tp))
                parametros.TamanhoPagina = tp;

            // Extrair ordenação
            if (query.TryGetValue("sortField", out var sfVal) && !string.IsNullOrWhiteSpace(sfVal))
                parametros.SortField = sfVal.ToString();
            if (query.TryGetValue("sortDir", out var sdVal) && !string.IsNullOrWhiteSpace(sdVal))
                parametros.SortDir = sdVal.ToString();

            // Extrair filtro de nome
            if (query.TryGetValue("nome", out var nomeVal) && !string.IsNullOrWhiteSpace(nomeVal))
                parametros.Nome = nomeVal.ToString();

            // Extrair inativo
            if (query.TryGetValue("inativo", out var inativoVal) && int.TryParse(inativoVal, out var inativo))
                parametros.Inativo = inativo;

            // Extrair filtros customizados (campo/operador/valor)
            parametros.Filtros = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            if (query.TryGetValue("campo", out var campoVal) && !string.IsNullOrWhiteSpace(campoVal))
                parametros.Filtros["campo"] = campoVal.ToString();
            if (query.TryGetValue("operador", out var opVal) && !string.IsNullOrWhiteSpace(opVal))
                parametros.Filtros["operador"] = opVal.ToString();
            if (query.TryGetValue("valor", out var valorVal) && !string.IsNullOrWhiteSpace(valorVal))
                parametros.Filtros["valor"] = valorVal.ToString();
            if (query.TryGetValue("valor_de", out var vdeVal) && !string.IsNullOrWhiteSpace(vdeVal))
                parametros.Filtros["valor_de"] = vdeVal.ToString();
            if (query.TryGetValue("valor_ate", out var vateVal) && !string.IsNullOrWhiteSpace(vateVal))
                parametros.Filtros["valor_ate"] = vateVal.ToString();

            // Extrair TODOS os query parameters não-conhecidos e adicioná-los ao Filtros
            foreach (var key in query.Keys)
            {
                if (!parametrosConhecidos.Contains(key, StringComparer.OrdinalIgnoreCase))
                {
                    var value = query[key].ToString();
                    if (!string.IsNullOrWhiteSpace(value))
                        parametros.Filtros[key] = value;
                }
            }

            bindingContext.Result = ModelBindingResult.Success(parametros);
            return Task.CompletedTask;
        }
    }
}
