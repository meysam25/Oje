using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Infrastructure.Enums
{
    public enum PriceDiscountStatus
    {
        [Display(Name = "نقدی و غیر نقدی", Prompt = "1")]
        CashAndNotCash = 1,
        [Display(Name = "فقط نقدی", Prompt = "2")]
        JustCash = 2,
        [Display(Name = "فقط غیر نقدی", Prompt = "3")]
        JustNotCash = 3
    }
}
