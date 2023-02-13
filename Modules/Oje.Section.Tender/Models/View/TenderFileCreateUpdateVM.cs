using Microsoft.AspNetCore.Http;

namespace Oje.Section.Tender.Models.View
{
    public class TenderFileCreateUpdateVM
    {
        public int? id { get; set; }
        public string title { get; set; }
        public bool? isActive { get; set; }
        public IFormFile mainFile { get; set; }
    }
}
