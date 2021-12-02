using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.ProposalFormBaseData.Models.View
{
    public class ProposalFormPostPriceMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public int id { get; set; }
        [Display(Name = "فرم پیشنهاد")]
        public string ppf { get; set; }
        [Display(Name = "عنوان")]
        public string title { get; set; }
        [Display(Name = "هزینه")]
        public string price { get; set; }
        [Display(Name = "وضعیت")]
        public string isActive { get; set; }
    }
}
