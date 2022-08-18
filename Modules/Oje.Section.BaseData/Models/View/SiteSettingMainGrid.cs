using Oje.Infrastructure.Models;

namespace Oje.Section.BaseData.Models.View
{
    public class SiteSettingMainGrid: GlobalGridParentLong
    {
        public string website { get; set; }
        public string title { get; set; }
        public string userfirstname { get; set; }
        public string userlastname { get; set; }
        public bool? isHttps { get; set; }
        public bool? isActive { get; set; }
    }
}
