using Oje.Infrastructure.Models;

namespace Oje.Section.WebMain.Models.View
{
    public class PageManifestMainGrid: GlobalGrid
    {
        public string pageTitle { get; set; }
        public string title { get; set; }
        public bool? isActive { get; set; }
    }
}
