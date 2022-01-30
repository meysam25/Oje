using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Security.Models.View
{
    public class BlockClientConfigMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public int id { get;set; }
        [Display(Name = "نوع")]
        public string type { get; set; }
        [Display(Name = "وضعیت")]
        public string isActive { get; set; }
    }
}
