using Oje.Infrastructure.Models;

namespace Oje.Section.WebMain.Models.View
{
    public class PageManifestItemMainGrid: GlobalGrid
    {
        public string title { get; set; }
        public string midTitle { get; set; }
        public string pageTitle { get; set; }
        public bool? isActive { get; set; }
    }
}
