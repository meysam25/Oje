using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;

namespace Oje.Security.Models.View
{
    public class GoogleBackupArchiveMainGrid: GlobalGrid
    {
        public GoogleBackupArchiveType? location { get; set; }
        public string createDate { get; set; }
    }
}
