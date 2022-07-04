using Oje.Infrastructure.Models;

namespace Oje.Security.Models.View
{
    public class AdminBlockClientConfigMainGrid: GlobalGrid
    {
        public string action { get; set; }
        public int? value { get; set; }
        public bool? isActive { get; set; }
    }
}
