using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.AccountService.Models.View
{
    public class UserNotificationTrigerURUVM
    {
        public int? RoleId { get; set; }
        public long? UserId { get; set; }
        public string userFullname { get; set; }
    }
}
