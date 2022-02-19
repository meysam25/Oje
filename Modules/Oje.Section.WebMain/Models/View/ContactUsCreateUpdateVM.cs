using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.WebMain.Models.View
{
    public class ContactUsCreateUpdateVM
    {
        [Display(Name = "عنوان")]
        public string title { get; set; }
        [Display(Name = "زیر عنوان")]
        public string subTitle { get; set; }
        [Display(Name = "موقعیت نقشه لت")]
        public string mapLat { get; set; }
        [Display(Name = "موقعیت نقشه لن")]
        public string mapLon { get; set; }
        [Display(Name = "زوم نقشه")]
        public string mapZoom { get; set; }
        [Display(Name = "توضیحات")]
        public string description { get; set; }
    }
}
