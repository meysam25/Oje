using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Infrastructure.Models
{
    public class LoginUserVM
    {
        public long UserId { get; set; }
        public string Username { get; set; }
        public string Fullname { get; set; }
        public string Ip { get; set; }
        public int? siteSettingId { get; set; }
    }
}
