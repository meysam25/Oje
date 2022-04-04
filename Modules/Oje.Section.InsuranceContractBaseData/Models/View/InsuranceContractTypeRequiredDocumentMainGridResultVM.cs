using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InsuranceContractBaseData.Models.View
{
    public class InsuranceContractTypeRequiredDocumentMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public int id { get; set; }
        [Display(Name = "عنوان")]
        public string title { get; set; }
        [Display(Name = "قرارداد")]
        public string cid { get; set; }
        [Display(Name = "نوع قرارداد")]
        public string ctId { get; set; }
        [Display(Name = "اجباری")]
        public string isRequired { get; set; }
        [Display(Name = "وضعیت")]
        public string isActive { get; set; }
    }
}
