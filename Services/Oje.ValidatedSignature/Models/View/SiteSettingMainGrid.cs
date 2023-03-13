using Oje.Infrastructure.Models;

namespace Oje.ValidatedSignature.Models.View
{
    public class SiteSettingMainGrid: GlobalGrid
    {
        public int id { get; set; }
        public string title { get; set; }
        public string domain { get; set; }
        public string createDate { get; set; }
        public bool? isActive { get; set; }
    }
}
