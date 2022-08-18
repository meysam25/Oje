using System.ComponentModel.DataAnnotations;

namespace Oje.Sanab.Models.View
{
    public class SanabTypeFieldVehicleSpecMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public int id { get; set; }
        [Display(Name = "عنوان")]
        public string title { get; set; }
        [Display(Name = "کد")]
        public string code { get; set; }
        [Display(Name = "عنوان برند")]
        public string vTitle { get; set; }
        [Display(Name = "عنوان نوع خودرو")]
        public string vSTitle { get; set; }
    }
}
