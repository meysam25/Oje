using System.ComponentModel.DataAnnotations;

namespace Oje.ValidatedSignature.Models.View
{
    public class UserMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public long? id { get; set; }
        [Display(Name = "نام کاربری")]
        public string username { get; set; }
        [Display(Name = "نام")]
        public string firstname { get; set; }
        [Display(Name = "نام خانوادگی")]
        public string lastname { get; set; }
        [Display(Name = "وضعیت")]
        public string isActive { get; set; }
        [Display(Name = "حذف")]
        public string isDelete { get; set; }
        [Display(Name = "تاریخ ایجاد")]
        public string createDate { get; set; }
        [Display(Name = "کد ملی")]
        public string nationalCode { get; set; }
        [Display(Name = "وب سایت")]
        public string website { get; set; }
        [Display(Name = "مجاز")]
        public string isValid { get; set; }
    }
}
