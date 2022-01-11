using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Infrastructure.Enums
{
    public enum SmsValidationHistoryType
    {
        [Display(Name = "ثبت نام یا لاگین")]
        RegisterLogin = 1,
        [Display(Name = "فراموشی کلمه عبور")]
        ForgetPassword = 2
    }
}
