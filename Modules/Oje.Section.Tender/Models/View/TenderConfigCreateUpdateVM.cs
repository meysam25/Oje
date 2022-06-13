
using Microsoft.AspNetCore.Http;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;

namespace Oje.Section.Tender.Models.View
{
    public class TenderConfigCreateUpdateVM
    {
        [IgnoreStringEncode]
        public MyHtmlString desctpion { get; set; }
        public IFormFile generallow { get; set; }
        public string generallow_address { get; set; }
    }
}
