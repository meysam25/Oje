using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Infrastructure.Enums
{
    public enum CarExteraDiscountType
    {
        [Display(Name = "مبلغ")]
        Price = 1,
        [Display(Name = "تاریخ")]
        Date = 2
    }
}
