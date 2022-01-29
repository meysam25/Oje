using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Security.Models.View
{
    public class IpLimitationBlackListMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public int id { get; set; }
        [Display(Name = "ای پی")]
        public string ip { get; set; }
        [Display(Name = "وضعیت")]
        public string isActive { get; set; }
    }
}
