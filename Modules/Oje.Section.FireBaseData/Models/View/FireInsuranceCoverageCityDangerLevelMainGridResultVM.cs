using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.FireBaseData.Models.View
{
    public class FireInsuranceCoverageCityDangerLevelMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public int id { get; set; }
        [Display(Name = "پوشش")]
        public string cover { get; set; }
        [Display(Name = "نرخ")]
        public decimal rate { get; set; }
        [Display(Name = "طبقه خطر")]
        public int danger { get; set; }
        [Display(Name = "وضعیت")]
        public string isActive { get; set; }
    }
}
