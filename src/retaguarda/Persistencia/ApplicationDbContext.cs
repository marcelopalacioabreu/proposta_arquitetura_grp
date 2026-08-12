using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace Retaguarda.Persistencia
{
    // Base abstract DbContext type used by provider-specific implementations.
    public abstract class ApplicationDbContext : DbContext
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;

        protected ApplicationDbContext(DbContextOptions options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor; // Can be null during design-time
        }
    }
}
