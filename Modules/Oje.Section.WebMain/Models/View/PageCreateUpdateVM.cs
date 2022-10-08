using Microsoft.AspNetCore.Http;
using Oje.Infrastructure.Models;

namespace Oje.Section.WebMain.Models.View
{
    public class PageCreateUpdateVM: GlobalSiteSetting
    {
        public long? id { get; set; }
        public string title { get; set; }
        public string subTitle { get; set; }
        public string stColor { get; set; }
        public string summery { get; set; }
        public IFormFile mainImage { get; set; }
        public IFormFile mainImageSmall { get; set; }
        public string bTitle { get; set; }
        public string bLink { get; set; }
    }
}
