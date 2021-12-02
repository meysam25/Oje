using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.ProposalFormBaseData.Models.View
{
    public class PaymentMethodMainGridResult
    {
        [Display(Name = "ردیف")]
        public int? row { get; set; }
        [Display(Name = "شناسه")]
        public int? id { get; set; }
        [Display(Name = "فرم پیشنهاد")]
        public string form { get; set; }
        [Display(Name = "عنوان")]
        public string title { get; set; }
        [Display(Name = "شرکت بیمه")]
        public string comId { get; set; }
        [Display(Name = "نوع")]
        public string type { get; set; }
        [Display(Name = "وضعیت")]
        public string isActive { get; set; }
        [Display(Name = "پیشفرض")]
        public string isDefault { get; set; }
    }
}
