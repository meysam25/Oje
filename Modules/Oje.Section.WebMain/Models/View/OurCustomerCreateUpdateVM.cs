using Microsoft.AspNetCore.Http;
using Oje.Infrastructure.Models;

namespace Oje.Section.WebMain.Models.View
{
    public class OurCustomerCreateUpdateVM : GlobalSiteSetting
    {
        public int? id { get; set; }
        public string title { get; set; }
        public string subtitle { get; set; }
        public IFormFile mainImage { get; set; }
        public string url { get; set; }
        public bool? isActive { get; set; }
    }
}
