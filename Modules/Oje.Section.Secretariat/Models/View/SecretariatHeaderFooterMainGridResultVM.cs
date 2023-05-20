using System.ComponentModel.DataAnnotations;

namespace Oje.Section.Secretariat.Models.View
{
    public class SecretariatHeaderFooterMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public int id { get; set; }
        [Display(Name = "عنوان")]
        public string title { get; set; }
    }
}
