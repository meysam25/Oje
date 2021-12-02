using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InsuranceContractBaseData.Models.View
{
    public class InsuranceContractUserMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public long id { get; set; }
        [Display(Name = "قرارداد")]
        public string contract { get; set; }
        [Display(Name = "نام")]
        public string fistname { get; set; }
        [Display(Name = "نام خانوادگی")]
        public string lastname { get; set; }
        [Display(Name = "کد ملی")]
        public string nationalcode { get; set; }
        [Display(Name = "کد ملی نفر اصلی")]
        public string mainPersonNationalcode { get; set; }
        [Display(Name = "تاریخ تولد")]
        public string birthDate { get; set; }
        [Display(Name = "نصبت")]
        public string familyRelation { get; set; }
        [Display(Name = "کاربر ثبت شده")]
        public string createUser { get; set; }
        [Display(Name = "کد الکترونکی")]
        public string eCode { get; set; }
        [Display(Name = "کد الکترونکی نفر اصلی")]
        public string mainECode { get; set; }
        [Display(Name = "وضعیت")]
        public string isActive { get; set; }
    }
}
