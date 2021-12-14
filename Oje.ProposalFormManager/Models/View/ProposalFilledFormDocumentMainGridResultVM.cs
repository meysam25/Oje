using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormManager.Models.View
{
    public class ProposalFilledFormDocumentMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public long id { get; set; }
        [Display(Name = "نوع")]
        public string type { get; set; }
        [Display(Name = "بانک")]
        public string bankId { get; set; }
        [Display(Name = "مبلغ")]
        public string price { get; set; }
        [Display(Name = "تاریخ ایجاد")]
        public string createDate { get; set; }
        [Display(Name = "تاریخ رسید")]
        public string arriveDate { get; set; }
        [Display(Name = "تاریخ وصول")]
        public string cashDate { get; set; }
        [Display(Name = "وضعیت")]
        public string status { get; set; }
        [Display(Name = "شناسه فرم پیشنهاد")]
        public long ppfId { get; set; }
        
    }
}
