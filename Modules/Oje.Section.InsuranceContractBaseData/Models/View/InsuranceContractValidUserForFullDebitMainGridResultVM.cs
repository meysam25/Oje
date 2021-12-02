using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InsuranceContractBaseData.Models.View
{
    public class InsuranceContractValidUserForFullDebitMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public long id { get; set; }
        [Display(Name = "قرارداد")]
        public string contract { get; set; }
        [Display(Name = "کاربر")]
        public string createUser { get; set; }
        [Display(Name = "تاریخ ایجاد")]
        public string createDate { get; set; }
        [Display(Name = "شماره همراه")]
        public string mobile { get; set; }
        [Display(Name = "کد ملی")]
        public string nationalCode { get; set; }
        [Display(Name = "تعداد مورد استفاده")]
        public int? countUse { get; set; }
        [Display(Name = "وضعیت")]
        public string isActive { get; set; }
    }
}
