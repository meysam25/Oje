using Oje.Infrastructure.Models;

namespace Oje.AccountService.Models.View
{
    public class UserRequestActionMainGrid: GlobalGrid
    {
        public string user { get; set; }
        public string role { get; set; }
        public string createDate { get; set; }
        public string action { get; set; }
    }
}
