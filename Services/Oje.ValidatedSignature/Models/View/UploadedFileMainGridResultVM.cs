using System.ComponentModel.DataAnnotations;

namespace Oje.ValidatedSignature.Models.View
{
    public class UploadedFileMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public long? id { get; set; }
        [Display(Name = "نوع فایل")]
        public string ft { get; set; }
        [Display(Name = "کاربر")]
        public string user { get; set; }
        [Display(Name = "دسترسی")]
        public string rAccess { get; set; }
        [Display(Name = "نوع محتوی فایل")]
        public string fct { get; set; }
        [Display(Name = "وب سایت")]
        public string website { get; set; }
        [Display(Name = "اندازه")]
        public string fs { get; set; }
        [Display(Name = "مجاز")]
        public string isValid { get; set; }
    }
}
