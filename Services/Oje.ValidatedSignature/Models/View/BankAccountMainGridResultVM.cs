using System.ComponentModel.DataAnnotations;

namespace Oje.ValidatedSignature.Models.View
{
    public class BankAccountMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public int id { get; set; }
        [Display(Name = "عنوان")]
        public string title { get; set; }
        [Display(Name = "شماره کارت")]
        public long? cardno { get; set; }
        [Display(Name = "شبا")]
        public string shabaNo { get; set; }
        [Display(Name = "شماره حساب")]
        public long? hesabNo { get; set; }
        [Display(Name = "کاربر")]
        public string user { get; set; }
        [Display(Name = "وضعیت")]
        public string isActive { get; set; }
        [Display(Name = "وب سایت")]
        public string website { get; set; }
        [Display(Name = "مجاز")]
        public string isValid { get; set; }

    }
}
