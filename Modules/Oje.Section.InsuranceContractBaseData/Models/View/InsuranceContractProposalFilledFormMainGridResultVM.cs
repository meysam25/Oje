using System.ComponentModel.DataAnnotations;

namespace Oje.Section.InsuranceContractBaseData.Models.View
{
    public class InsuranceContractProposalFilledFormMainGridResultVM
    {
        [Display(Name = "شناسه")]
        public long id { get; set; }
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "نام کاربر")]
        public string createUserfullname { get; set; }
        [Display(Name = "تاریخ ایجاد")]
        public string createDate { get; set; }
        [Display(Name = "وضعیت")]
        public string status { get; set; }
        [Display(Name = "قرارداد")]
        public string contractTitle { get; set; }
        [Display(Name = "اعضای خانواده")]
        public string familyMemers { get; set; }
        [Display(Name = "وب سایت")]
        public string siteTitleMN2 { get; set; }
    }
}
