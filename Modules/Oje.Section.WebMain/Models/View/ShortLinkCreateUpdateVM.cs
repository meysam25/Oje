using Oje.Infrastructure.Models;

namespace Oje.Section.WebMain.Models.View
{
    public class ShortLinkCreateUpdateVM: GlobalSiteSetting
    {
        public long? id { get; set; }
        public string code { get; set; }
        public string link { get; set; }
        public bool? isActive { get; set; }
    }
}
