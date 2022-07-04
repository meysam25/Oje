using Oje.Infrastructure.Models;

namespace Oje.Security.Models.View
{
    public class BlockLoginUserMainGrid: GlobalGrid
    {
        public string startDate { get; set; }
        public string endDate { get; set; }
        public bool? isActive { get; set; }
    }
}
