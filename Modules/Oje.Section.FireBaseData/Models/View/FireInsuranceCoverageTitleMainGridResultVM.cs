using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.FireBaseData.Models.View
{
    public class FireInsuranceCoverageTitleMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public int id { get; set; }
        [Display(Name = "عنوان")]
        public string title { get; set; }
        [Display(Name = "نوع محاسبه")]
        public string effect { get; set; }
        [Display(Name = "وضعیت")]
        public string isActive { get; set; }
    }
}
