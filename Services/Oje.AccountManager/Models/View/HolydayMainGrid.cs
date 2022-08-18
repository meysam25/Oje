using Oje.Infrastructure.Models;

namespace Oje.AccountService.Models.View
{
    public class HolydayMainGrid: GlobalGrid
    {
        public string targetDate { get; set; }
        public bool? isActive { get; set; }
    }
}
