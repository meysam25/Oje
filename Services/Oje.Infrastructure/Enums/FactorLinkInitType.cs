
using System.ComponentModel.DataAnnotations;

namespace Oje.Infrastructure.Enums
{
    public enum FactorLinkInitType
    {
        [Display(Name = "ارسال لینک پرداخت از طریق پیامک")]
        SendSMS = 0,
        [Display(Name = "دریافت لینک پرداخت به صورت بارکد")]
        Barcode = 1,
        [Display(Name = "دریافت لینک پرداخت به صورت لینک")]
        LinkText = 2
    }
}
