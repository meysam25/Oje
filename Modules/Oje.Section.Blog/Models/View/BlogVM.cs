using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.Blog.Models.View
{
    public class BlogVM
    {
        public long id { get; internal set; }
        public int catId { get; internal set; }
        public string title { get; internal set; }
        public string publishDate { get; internal set; }
        public string summery { get; internal set; }
        [IgnoreStringEncode]
        public MyHtmlString description { get; internal set; }
        public string aparatUrl { get; internal set; }
        public string mainImage_address { get; internal set; }
        public string mainSound_address { get; internal set; }
        public bool isActive { get; internal set; }
        public object tags { get; set; }
        public object rBlogs { get; set; }
    }
}
