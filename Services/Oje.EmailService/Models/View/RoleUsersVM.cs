using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.EmailService.Models.View
{
    public class RoleUsersVM
    {
        public long userId { get; set; }
        public string userFullname { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
    }
}
