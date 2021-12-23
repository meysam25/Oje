using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Infrastructure.Enums
{
    public enum BankAccountType
    {
        [Display(Name = "سیزپی")]
        sizpay = 1,

        [Display(Name = "نامشخص")]
        unknown = 999
    }
}
