using System.ComponentModel.DataAnnotations;

namespace Oje.ValidatedSignature.Models.View
{
    public class TenderFilledFormMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public long? id { get; set; }
        [Display(Name = "تاریخ")]
        public string createDate { get; set; }
        [Display(Name = "کاربر")]
        public string user { get; set; }
        [Display(Name = "مجاز")]
        public string isValid { get; set; }
        [Display(Name = "وب سایت")]
        public string website { get; set; }
    }
}
