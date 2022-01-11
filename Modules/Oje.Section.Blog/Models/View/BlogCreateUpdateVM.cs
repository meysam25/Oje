using Microsoft.AspNetCore.Http;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.Blog.Models.View
{
    public class BlogCreateUpdateVM
    {
        public long? id { get; set; }
        public int? catId { get; set; }
        public string title { get; set; }
        public string publishDate { get; set; }
        public string summery { get; set; }
        [IgnoreStringEncode]
        public MyHtmlString description { get; set; }
        public string aparatUrl { get; set; }
        public bool? isActive { get; set; }
        public IFormFile mainImage { get; set; }
        public IFormFile mainSound { get; set; }
        public List<string> tags { get; set; }
        public List<long> rBlogs { get; set; }
    }
}
