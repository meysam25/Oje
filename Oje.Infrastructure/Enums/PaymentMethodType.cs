using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Infrastructure.Enums
{
    public enum PaymentMethodType
    {
        [Display(Name = "نقدی")]
        Cash = 1,
        [Display(Name = "غیر نقدی")]
        Debit = 2
    }
}
