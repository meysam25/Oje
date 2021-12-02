using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarBodyBaseData.Models.View
{
    public class CarSpecificationAmountMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int? row { get; set; }
        [Display(Name = "شناسه")]
        public int? id { get; set; }
        [Display(Name = "شرکت")]
        public string company { get; set; }
        [Display(Name = "خصوصیت خودرو")]
        public string carSpecId { get; set; }
        [Display(Name = "حداقل")]
        public string minValue { get; set; }
        [Display(Name = "حداکثر")]
        public string maxValue { get; set; }
        [Display(Name = "نرخ")]
        public string amount { get; set; }
        [Display(Name = "وضعیت")]
        public string isActive { get; set; }
    }
}
