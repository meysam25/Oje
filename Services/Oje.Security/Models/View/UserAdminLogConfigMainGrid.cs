using Oje.Infrastructure.Models;

namespace Oje.Security.Models.View
{
    public class UserAdminLogConfigMainGrid: GlobalGrid
    {
        public string title { get; set; }
        public bool? isActive { get; set; }
    }
}
