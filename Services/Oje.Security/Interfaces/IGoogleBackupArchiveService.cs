using Oje.Infrastructure.Models;
using Oje.Security.Models.View;

namespace Oje.Security.Interfaces
{
    public interface IGoogleBackupArchiveService
    {
        GridResultVM<GoogleBackupArchiveMainGridResultVM> GetList(GoogleBackupArchiveMainGrid searchInput);
    }
}
