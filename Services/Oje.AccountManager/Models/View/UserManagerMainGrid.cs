using Oje.Infrastructure.Models;

namespace Oje.AccountService.Models.View
{
    public class UserServiceMainGrid: GlobalGrid
    {
        public string parent { get; set; }
        public string username { get; set; }
        public string fistname { get; set; }
        public string lastname { get; set; }
        public string mobile { get; set; }
        public bool? isActive { get; set; }
        public int? roleIds { get; set; }
    }
}
