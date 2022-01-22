using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.Blog.Models.View
{
    public class BlogReviewMainGrid: GlobalGrid
    {
        public string blogTitle { get; set; }
        public string userFullname { get; set; }
        public string userMobile { get; set; }
        public string userEmail { get; set; }
        public bool? isActive { get; set; }
    }
}
