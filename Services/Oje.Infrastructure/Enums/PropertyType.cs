using System.ComponentModel.DataAnnotations;

namespace Oje.Infrastructure.Enums
{
    public enum PropertyType
    {
        [Display(Name = "درباره ما صفحه اصلی")]
        AboutUsMainPage = 1,
        [Display(Name = "یادآوری صفحه اصلی")]
        RemindUsMainPage = 2,
        [Display(Name = "افتخارات ما صفحه اصلی")]
        OurPrideMainPage = 3,
        [Display(Name = "توضیحات فوتر")]
        FooterDescrption = 4,
        [Display(Name = "نماد های فوتر")]
        FooterSymbol = 5,
        [Display(Name = "ایکن های بالا سمت چپ صفحه اصلی")]
        MainPageTopLeftIcon = 6,
        [Display(Name = "متا تگ عمومی")]
        GlobalMetaTag = 7,
        [Display(Name = "شبکه های اجتماعی فوتر")]
        FooterIcon = 8,
        [Display(Name = "تماس با ما")]
        ContactUs = 9,
        [Display(Name = "شماره پرداخت کارت به کارت")]
        UserFilledRegisterFormTargetBankCardNo = 10,
    }
}
