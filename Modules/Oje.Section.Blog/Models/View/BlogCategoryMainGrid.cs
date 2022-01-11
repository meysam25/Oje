using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.Blog.Models.View
{
    public class BlogCategoryMainGrid: GlobalGrid
    {
        public string title { get; set; }
        public bool? isActive { get; set; }
    }
}
