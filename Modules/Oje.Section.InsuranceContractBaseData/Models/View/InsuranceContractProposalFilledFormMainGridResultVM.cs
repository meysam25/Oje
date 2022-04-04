using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        [Display(Name = "تاریخ تایید")]
        public string confirmDate { get; set; }
        [Display(Name = "قرارداد")]
        public string contractTitle { get; set; }
        [Display(Name = "اعضای خانواده")]
        public string familyMemers { get; set; }
    }
}
