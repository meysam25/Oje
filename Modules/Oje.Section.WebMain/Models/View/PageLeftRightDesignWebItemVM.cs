using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;

namespace Oje.Section.WebMain.Models.View
{
    public class PageLeftRightDesignWebItemVM
    {
        public string title { get; set; }
        [IgnoreStringEncode]
        public MyHtmlString description { get; set; }
        public string image { get; set; }
        public int order { get; set; }
        public string bTitle { get; set; }
        public string bLink { get; set; }
    }
}
