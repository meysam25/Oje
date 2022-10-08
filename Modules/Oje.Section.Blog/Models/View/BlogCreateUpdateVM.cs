using Microsoft.AspNetCore.Http;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using System.Collections.Generic;

namespace Oje.Section.Blog.Models.View
{
    public class BlogCreateUpdateVM: GlobalSiteSetting
    {
        public long? id { get; set; }
        public int? catId { get; set; }
        public string title { get; set; }
        public string publishDate { get; set; }
        public string summery { get; set; }
        [IgnoreStringEncode]
        public MyHtmlString description { get; set; }
        [IgnoreStringEncode]
        public MyHtmlString aparatUrl { get; set; }
        public bool? isActive { get; set; }
        public IFormFile mainImage { get; set; }
        public IFormFile mainSound { get; set; }
        public List<string> tags { get; set; }
        public List<long> rBlogs { get; set; }
    }
}
