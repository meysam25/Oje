using Microsoft.AspNetCore.Http;
using Oje.Infrastructure.Enums;

namespace Oje.Section.BaseData.Models.View
{
    public class CreateUpdateSiteSettingVM
    {
        public int? id { get; set; }
        public string title { get; set; }
        public string subTitle { get; set; }
        public string websiteUrl { get; set; }
        public string panelUrl { get; set; }
        public long? userId { get; set; }
        public bool? isHttps { get; set; }
        public bool? isActive { get; set; }
        public IFormFile minPic { get; set; }
        public IFormFile minPicInvert { get; set; }
        public IFormFile textPic { get; set; }
        public string seo { get; set; }
        public WebsiteType? websiteType { get; set; }
        public string copyRightTitle { get; set; }
        public int? pKey { get; set; }
    }
}
