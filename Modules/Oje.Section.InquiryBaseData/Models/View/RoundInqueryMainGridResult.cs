using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InquiryBaseData.Models.View
{
    public class RoundInqueryMainGridResult
    {
        
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public int id { get; set; }
        [Display(Name = "فرمت")]
        public string format { get; set; }
        [Display(Name = "فرم پیشنهاد")]
        public string proposalFormTitle { get; set; }
        [Display(Name = "نوع")]
        public string type { get; set; }
    }
}
