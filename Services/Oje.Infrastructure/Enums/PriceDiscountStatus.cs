using System.ComponentModel.DataAnnotations;

namespace Oje.Infrastructure.Enums
{
    public enum PriceDiscountStatus
    {
        [Display(Name = "نقدی و اقساطی", Prompt = "1")]
        CashAndNotCash = 1,
        [Display(Name = "فقط نقدی", Prompt = "2")]
        JustCash = 2,
        [Display(Name = "فقط اقساطی", Prompt = "3")]
        JustNotCash = 3
    }
}
