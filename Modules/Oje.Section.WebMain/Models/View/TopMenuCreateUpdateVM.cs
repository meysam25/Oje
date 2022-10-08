using Oje.Infrastructure.Models;

namespace Oje.Section.WebMain.Models.View
{
    public class TopMenuCreateUpdateVM: GlobalSiteSetting
    {
        public long? id { get; set; }
        public long? pKey { get; set; }
        public string title { get; set; }
        public string link { get; set; }
        public int? order { get; set; }
        public bool? isActive { get; set; }

    }
}
