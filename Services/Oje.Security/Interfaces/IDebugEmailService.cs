using Oje.Infrastructure.Models;
using Oje.Security.Models.View;

namespace Oje.Security.Interfaces
{
    public interface IDebugEmailService
    {
        ApiResult CreateUpdate(DebugEmailCreateUpdateVM input);
        object Get();
        Task SendEmail();
    }
}
