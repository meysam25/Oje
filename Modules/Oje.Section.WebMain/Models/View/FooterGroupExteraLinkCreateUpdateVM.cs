using Oje.Infrastructure.Models;

namespace Oje.Section.WebMain.Models.View
{
    public class FooterGroupExteraLinkCreateUpdateVM: GlobalSiteSetting
    {
        public int? id { get; set; }
        public int? pKey { get; set; }
        public string title { get; set; }
        public string link { get; set; }
        public bool? isActive { get; set; }
        public int? order { get; set; }
    }
}
