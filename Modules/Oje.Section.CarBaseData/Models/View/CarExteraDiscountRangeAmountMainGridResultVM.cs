using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarBaseData.Models.View
{
    public class CarExteraDiscountRangeAmountMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; internal set; }
        [Display(Name = "شناسه")]
        public int id { get; internal set; }
        [Display(Name = "شرکت ها")]
        public string company { get; internal set; }
        [Display(Name = "تخفیف اضافه")]
        public string exteraDiscountTitle { get; internal set; }
        [Display(Name = "عنوان")]
        public string title { get; internal set; }
        [Display(Name = "حداقل")]
        public string minValue { get; internal set; }
        [Display(Name = "حداکثر")]
        public string maxValue { get; internal set; }
        [Display(Name = "وضعیت")]
        public string isActive { get; internal set; }
    }
}
