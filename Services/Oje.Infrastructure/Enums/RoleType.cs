using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Infrastructure.Enums
{
    public enum RoleType
    {
        [Display(Name ="کاربر وب سایت")]
        User = 1,
        [Display(Name = "بازاریاب")]
        Marketer = 2
    }
}
