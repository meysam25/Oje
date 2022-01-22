using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.Blog.Models.View
{
    public class BlogReviewMainGridResultVM
    {
        public int row { get; set; }
        public string id { get; set; }
        public string blogTitle { get; set; }
        public string userFullname { get; set; }
        public string userMobile { get; set; }
        public string userEmail { get; set; }
        public string isActive { get; set; }
        public bool iA{ get; set; }
    }
}
