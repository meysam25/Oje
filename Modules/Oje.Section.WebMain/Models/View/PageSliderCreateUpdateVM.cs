using Microsoft.AspNetCore.Http;
using Oje.Infrastructure.Models;

namespace Oje.Section.WebMain.Models.View
{
    public class PageSliderCreateUpdateVM: GlobalSiteSetting
    {
        public long? id { get; set; }
        public long? pid { get; set; }
        public string title { get; set; }
        public IFormFile mainImage { get; set; }
        public bool? isActive { get; set; }
    }
}
