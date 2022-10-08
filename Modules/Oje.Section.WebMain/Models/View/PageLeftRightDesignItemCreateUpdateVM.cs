using Microsoft.AspNetCore.Http;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;

namespace Oje.Section.WebMain.Models.View
{
    public class PageLeftRightDesignItemCreateUpdateVM: GlobalSiteSetting
    {
        public long? id { get; set; }
        public long? dId { get; set; }
        public string dId_Title { get; set; }
        public string title { get; set; }
        [IgnoreStringEncode]
        public MyHtmlString description { get; set; }
        public IFormFile mainImage { get; set; }
        public string mainImage_address { get; set; }
        public int? order { get; set; }
        public bool? isActive { get; set; }
        public string bTitle { get; set; }
        public string bLink { get; set; }
    }
}
