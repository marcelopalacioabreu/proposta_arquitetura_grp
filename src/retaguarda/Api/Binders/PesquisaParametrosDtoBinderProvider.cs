using Microsoft.AspNetCore.Mvc.ModelBinding;
using Retaguarda.DTO.Parametros;

namespace Retaguarda.Api.Binders
{
    public class PesquisaParametrosDtoBinderProvider : IModelBinderProvider
    {
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (context.Metadata.ModelType == typeof(PesquisaParametrosDto))
            {
                return new PesquisaParametrosDtoBinder();
            }

            return null;
        }
    }
}
