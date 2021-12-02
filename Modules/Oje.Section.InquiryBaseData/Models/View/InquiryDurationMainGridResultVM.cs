using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InquiryBaseData.Models.View
{
    public class InquiryDurationMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public int id { get; set; }
        [Display(Name = "شرکت")]
        public string company { get; set; }
        [Display(Name = "فرم پیشنهاد")]
        public string ppfTitle { get; set; }
        [Display(Name = "عنوان")]
        public string title { get; set; }
        [Display(Name = "درصد")]
        public int percent { get; set; }
        [Display(Name = "روز")]
        public int day { get; set; }
        [Display(Name = "وضعیت")]
        public string isActive { get; set; }
    }
}
