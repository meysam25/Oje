using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using System.Collections.Generic;

namespace Oje.Section.Blog.Models.View
{
    public class BlogVM
    {
        public long id { get; internal set; }
        public int catId { get; internal set; }
        public string title { get; internal set; }
        public string publishDate { get; internal set; }
        public string publishDateEn { get; set; }
        public string createDateEn { get; set; }
        public string summery { get; internal set; }
        [IgnoreStringEncode]
        public MyHtmlString description { get; internal set; }
        [IgnoreStringEncode]
        public MyHtmlString aparatUrl { get; internal set; }
        public string mainImage_address { get; internal set; }
        public string mainSound_address { get; internal set; }
        public bool isActive { get; internal set; }
        public List<BlogTagVM> tags { get; set; }
        public List<BlogVM> rBlogs { get; set; }
        public string catTitle { get; internal set; }
        public int commCount { get; set; }
        public int likeCount { get; set; }
        public bool didILikeIt { get; set; }
        public string url { get; set; }
        public string user { get; set; }
        public int cSOWSiteSettingId { get;  set; }
        public string cSOWSiteSettingId_Title { get;  set; }
    }
}
