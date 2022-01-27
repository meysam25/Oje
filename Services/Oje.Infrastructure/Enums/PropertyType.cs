using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        FooterSymbol = 5
    }
}
