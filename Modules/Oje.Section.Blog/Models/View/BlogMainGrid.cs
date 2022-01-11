using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.Blog.Models.View
{
    public class BlogMainGrid: GlobalGrid
    {
        public string title { get; set; }
        public string createBy { get; set; }
        public string publishDate { get; set; }
        public bool? isActive { get; set; }
    }
}
