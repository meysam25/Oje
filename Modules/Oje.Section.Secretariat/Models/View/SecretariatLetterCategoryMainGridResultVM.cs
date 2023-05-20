using System.ComponentModel.DataAnnotations;

namespace Oje.Section.Secretariat.Models.View
{
    public class SecretariatLetterCategoryMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public int id { get; set; }
        [Display(Name = "عنوان")]
        public string title { get; set; }
        [Display(Name = "وضعیت")]
        public string isActive { get; set; }
        [Display(Name = "کد")]
        public string code { get; set; }
    }
}
