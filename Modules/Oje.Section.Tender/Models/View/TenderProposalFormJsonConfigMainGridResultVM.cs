using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Oje.Section.Tender.Models.View
{
    public class TenderProposalFormJsonConfigMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public int id { get; set; }
        [Display(Name = "فرم پیشنهاد")]
        public string ppfTitle { get; set; }
        [Display(Name = "وضعیت")]
        public string isActive { get; set; }
    }
}
