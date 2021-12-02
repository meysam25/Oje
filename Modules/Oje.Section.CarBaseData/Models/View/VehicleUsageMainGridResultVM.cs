using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarBaseData.Models.View
{
    public class VehicleUsageMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public int id { get; set; }
        [Display(Name = "عنوان")]
        public string title { get; set; }
        [Display(Name = "کاربری خودرو")]
        public string carTypes { get; set; }
        [Display(Name = "درصد مورد استفاده بدنه")]
        public string bodyPercent { get; set; }
        [Display(Name = "درصد مورد استفاده ثالث")]
        public string thirdPercent { get; set; }
        [Display(Name = "وضعیت")]
        public string isActive { get; set; }
    }
}
