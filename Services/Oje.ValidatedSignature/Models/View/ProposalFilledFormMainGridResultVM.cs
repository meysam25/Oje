using System.ComponentModel.DataAnnotations;

namespace Oje.ValidatedSignature.Models.View
{
    public class ProposalFilledFormMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public string id { get; set; }
        [Display(Name = "شرکت")]
        public string companyTitle { get; set; }
        [Display(Name = "فرم پیشنهاد")]
        public string formTitle { get; set; }
        [Display(Name = "تاریخ ایجاد")]
        public string createDate { get; set; }
        [Display(Name = "وب سایت")]
        public string website { get; set; }
        [Display(Name = "مجاز")]
        public string isValid { get; set; }
    }
}
