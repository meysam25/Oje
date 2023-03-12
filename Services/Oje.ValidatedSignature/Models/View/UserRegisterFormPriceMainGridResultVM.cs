using System.ComponentModel.DataAnnotations;

namespace Oje.ValidatedSignature.Models.View
{
    public class UserRegisterFormPriceMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public int id { get; set; }
        [Display(Name = "عنوان")]
        public string title { get; set; }
        [Display(Name = "مبلغ")]
        public string price { get; set; }
        [Display(Name = "وضعیت")]
        public string isActive { get; set; }
        [Display(Name = "مجاز")]
        public string isValid { get; set; }
    }
}
