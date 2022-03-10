using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.WebMain.Models.View
{
    public class FooterSocialIconCreateUpdateVM
    {
        [Display(Name = "لینک این")]
        public string linkIn { get; set; }
        [Display(Name = "انستاگرام")]
        public string instageram { get; set; }
        [Display(Name = "واتساپ")]
        public string watapp { get; set; }
        [Display(Name = "تلگرام")]
        public string telegeram { get; set; }
    }
}
