using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InquiryBaseData.Models.View
{
    public class InquiryCompanyLimitMainGridResult
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public int? id { get; set; }
        [Display(Name = "نوع")]
        public string type { get; set; }
        [Display(Name = "شرکت های بیمه")]
        public string comId { get; set; }
        [Display(Name = "کاربر ایجاد کننده")]
        public string createUser { get; set; }
        [Display(Name = "تاریخ ایجاد")]
        public string createDate { get; set; }
    }
}
