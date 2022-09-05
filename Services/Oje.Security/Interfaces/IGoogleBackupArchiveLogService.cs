using Oje.Infrastructure.Models;
using Oje.Security.Models.View;

namespace Oje.Security.Interfaces
{
    public interface IGoogleBackupArchiveLogService
    {
        GridResultVM<GoogleBackupArchiveLogMainGridResultVM> GetList(GoogleBackupArchiveLogMainGrid searchInput);
    }
}
