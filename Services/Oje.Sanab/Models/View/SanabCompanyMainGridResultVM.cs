using System.ComponentModel.DataAnnotations;

namespace Oje.Sanab.Models.View
{
    public class SanabCompanyMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public int id { get; set; }
        [Display(Name = "شرکت")]
        public string company { get; set; }
        [Display(Name = "کد")]
        public string code { get; set; }
    }
}
