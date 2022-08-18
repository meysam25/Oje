using Microsoft.AspNetCore.Http;

namespace Oje.Section.BaseData.Models.View
{
    public class CreateUpdateSiteSettingVM
    {
        public int? id { get; set; }
        public string title { get; set; }
        public string websiteUrl { get; set; }
        public string panelUrl { get; set; }
        public long? userId { get; set; }
        public bool? isHttps { get; set; }
        public bool? isActive { get; set; }
        public IFormFile minPic { get; set; }
        public IFormFile textPic { get; set; }
        public string seo { get; set; }
        public int? pKey { get; set; }
    }
}
