using System.ComponentModel.DataAnnotations;

namespace Oje.Infrastructure.Enums
{
    public enum BankAccountFactorType
    {
        [Display(Name = "فرم پیشنهاد")]
        ProposalFilledForm = 1,
        [Display(Name = "ثبت نام کاربر")]
        UserRegister = 2,
        [Display(Name = "افزایش موجودی کیف پول")]
        Wallet = 3
    }
}
