using Oje.Infrastructure.Models;

namespace Oje.Security.Models.View
{
    public class ValidRangeIpMainGrid : GlobalGrid
    {
        public string title { get; set; }
        public string fromIp { get; set; }
        public string toIp { get; set; }
        public bool? isActive { get; set; }
    }
}
