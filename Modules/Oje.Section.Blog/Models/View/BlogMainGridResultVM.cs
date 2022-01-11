using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.Blog.Models.View
{
    public class BlogMainGridResultVM
    {
        [Display(Name = "شناسه")]
        public long id { get; set; }
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "عنوان")]
        public string title { get; set; }
        [Display(Name = "توسط")]
        public string createBy { get; set; }
        [Display(Name = "تاریخ انتشار")]
        public string publishDate { get; set; }
        [Display(Name = "وضعیت")]
        public string isActive { get; set; }
    }
}
