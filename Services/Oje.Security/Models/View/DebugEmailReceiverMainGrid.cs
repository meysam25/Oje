using Oje.Infrastructure.Models;

namespace Oje.Security.Models.View
{
    public class DebugEmailReceiverMainGrid: GlobalGrid
    {
        public string email { get; set; }
        public bool? isActive { get; set; }
    }
}
