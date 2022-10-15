using System.ComponentModel.DataAnnotations;

namespace Oje.Infrastructure.Enums
{
    public enum SmsConfigType
    {
        [Display(Name = "مگفا")]
        Magfa = 1,
        [Display(Name = "ایده پردازان")]
        IdePardazan = 2
    }
}
