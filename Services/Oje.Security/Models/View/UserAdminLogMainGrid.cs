using Oje.Infrastructure.Models;

namespace Oje.Security.Models.View
{
    public class UserAdminLogMainGrid: GlobalGrid
    {
        public string userFullname { get; set; }
        public string action { get; set; }
        public bool? isSuccess { get; set; }
        public string ip { get; set; }
        public string createDate { get; set; }
    }
}
