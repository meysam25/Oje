using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Infrastructure.Models
{
    public class RoleUserGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public int id { get; set; }
        [Display(Name = "نام")]
        public string name { get; set; }
        [Display(Name = "عنوان")]
        public string title { get; set; }
        [Display(Name = "صاحب")]
        public string owner { get; set; }

    }
}
