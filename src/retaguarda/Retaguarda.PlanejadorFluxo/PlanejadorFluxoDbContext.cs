using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Retaguarda.Persistencia;

namespace Retaguarda.PlanejadorFluxo
{
    // Thin wrapper DbContext that reuses the provider-specific ApplicationDbContext
    public class PlanejadorFluxoDbContext : ApplicationDbContext
    {
        public PlanejadorFluxoDbContext(DbContextOptions<PlanejadorFluxoDbContext> options, IHttpContextAccessor httpContextAccessor)
            : base(options, httpContextAccessor)
        {
        }
    }
}
