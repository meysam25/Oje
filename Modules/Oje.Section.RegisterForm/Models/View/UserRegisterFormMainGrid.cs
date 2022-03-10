using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.RegisterForm.Models.View
{
    public class UserRegisterFormMainGrid: GlobalGrid
    {
        public string title { get; set; }
        public string name { get; set; }
        public bool? isActive { get; set; }
        public string userfullname { get; set; }
    }
}
