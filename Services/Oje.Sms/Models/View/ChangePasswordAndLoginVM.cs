using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Sms.Models.View
{
    public class ChangePasswordAndLoginVM
    {
        public string username { get; set; }
        public string codeId { get; set; }
        public string password { get; set; }
        public string confirmPassword { get; set; }
    }
}
