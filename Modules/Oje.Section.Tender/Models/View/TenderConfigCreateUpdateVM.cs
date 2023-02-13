using Microsoft.AspNetCore.Http;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;

namespace Oje.Section.Tender.Models.View
{
    public class TenderConfigCreateUpdateVM
    {
        public int id { get; set; }
        [IgnoreStringEncode]
        public MyHtmlString desctpion { get; set; }
        public IFormFile generallow { get; set; }
        public string generallow_address { get; set; }
        public string title { get; set; }
        public string subTitle { get; set; }
    }
}
