using Oje.Infrastructure.Models;

namespace Oje.Security.Models.View
{
    public class InValidRangeIpMainGrid: GlobalGrid
    {
        public string ip { get; set; }
        public string lastDate { get; set; }
        public int? count { get; set; }
        public bool? isSuccess { get; set; }
        public string message { get; set; }
        public bool? iB { get; set; }
    }
}
