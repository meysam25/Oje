using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;

namespace Oje.Security.Models.View
{
    public class UserLoginLogoutLogMainGrid: GlobalGrid
    {
        public string userfullname { get; set; }
        public string createDate { get; set; }
        public UserLoginLogoutLogType? type { get; set; }
        public string ip { get; set; }
        public string message { get; set; }
        public bool? isSuccess { get; set; }
    }
}
