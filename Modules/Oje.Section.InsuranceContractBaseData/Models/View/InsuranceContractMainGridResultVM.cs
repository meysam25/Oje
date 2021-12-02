using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InsuranceContractBaseData.Models.View
{
    public class InsuranceContractMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public int id { get; set; }
        [Display(Name = "شرکت بیمه گذار حقوقی")]
        public string contractCompany { get; set; }
        [Display(Name = "نوع قرارداد")]
        public string contractType { get; set; }
        [Display(Name = "فرم پیشنهاد")]
        public string ppfTitle { get; set; }
        [Display(Name = "عنوان")]
        public string title { get; set; }
        [Display(Name = "کاربر")]
        public string createUser { get; set; }
        [Display(Name = "تاریخ ایجاد")]
        public string createDate { get; set; }
        [Display(Name = "کد")]
        public string code { get; set; }
        [Display(Name = "از تاریخ")]
        public string fromDate { get; set; }
        [Display(Name = "تا تاریخ")]
        public string toDate { get; set; }
        [Display(Name = "وضعیت")]
        public string isActive { get; set; }
    }
}
