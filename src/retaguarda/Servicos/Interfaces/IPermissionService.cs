using System.Collections.Generic;
using System.Threading.Tasks;

namespace Retaguarda.Servicos.Interfaces
{
    public interface IPermissionService
    {
        Task<IList<string>> GetPermissionsForUserAsync(long userId);
        Task<bool> IsUserAdministratorAsync(long userId);
    }
}
