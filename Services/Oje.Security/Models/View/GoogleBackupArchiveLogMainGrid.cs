using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;

namespace Oje.Security.Models.View
{
    public class GoogleBackupArchiveLogMainGrid: GlobalGrid
    {
        public string createDate { get; set; }
        public string message { get; set; }
        public GoogleBackupArchiveLogType? type { get; set; }
    }
}
