using System.ComponentModel.DataAnnotations;

namespace Oje.Section.Secretariat.Models.View
{
    public class SecretariatLetterReciveMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public long id { get; set; }
        [Display(Name = "تاریخ")]
        public string date { get; set; }
        [Display(Name = "شماره")]
        public string number { get; set; }
        [Display(Name = "همراه")]
        public string mobile { get; set; }
        [Display(Name = "تاریخ ایجاد")]
        public string createDate { get; set; }
        [Display(Name = "کاربر")]
        public string user { get; set; }
    }
}
