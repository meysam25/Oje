using System.ComponentModel.DataAnnotations;

namespace Oje.Sanab.Models.View
{
    public class SanabVehicleTypeMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public int id { get; set; }
        [Display(Name = "عنوان")]
        public string title { get; set; }
        [Display(Name = "نوع خودرو")]
        public string vtTitle { get; set; }
        [Display(Name = "کد")]
        public string code { get; set; }
    }
}
