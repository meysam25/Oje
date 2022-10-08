using Oje.Infrastructure.Models;

namespace Oje.Section.WebMain.Models.View
{
    public class FooterExteraLinkCreateUpdateVM: GlobalSiteSetting
    {
        public int? id { get; set; }
        public string title { get; set; }
        public string link { get; set; }
        public bool? isActive { get; set; }
    }
}
