using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Security.Models.View
{
    public class FileAccessRoleMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public int id { get; set; }
        [Display(Name = "نقش")]
        public string role { get; set; }
        [Display(Name = "نوع فایل")]
        public string fType { get; set; }
    }
}
