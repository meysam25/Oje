using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarBaseData.Models.View
{
    public class VehicleSpecMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public int id { get; set; }
        [Display(Name = "عنوان")]
        public string title { get; set; }
        [Display(Name = "گروه بندی")]
        public string specCat { get; set; }
        [Display(Name = "وضعیت")]
        public string isActive { get; set; }
        [Display(Name = "برند خودرو")]
        public string vSystem { get; set; }
    }
}
