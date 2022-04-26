
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;

namespace Oje.Section.WebMain.Models.View
{
    public class PageManifestItemCreateUpdateVM
    {
        public long? id { get; set; }
        public long? mid { get; set; }
        public string mid_Title { get; set; }
        public string title { get; set; }
        [IgnoreStringEncode]
        public MyHtmlString description { get; set; }
        public int? order { get; set; }
        public bool? isActive { get; set; }
    }
}
