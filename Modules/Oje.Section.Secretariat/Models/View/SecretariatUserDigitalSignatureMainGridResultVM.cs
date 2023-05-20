using System.ComponentModel.DataAnnotations;

namespace Oje.Section.Secretariat.Models.View
{
    public class SecretariatUserDigitalSignatureMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public int id { get; set; }
        [Display(Name = "نقش")]
        public string role { get; set; }
        [Display(Name = "کاربر")]
        public string user { get; set; }
        [Display(Name = "وضعیت")]
        public string isActive { get; set; }
    }
}
