using Microsoft.AspNetCore.Http;

namespace Oje.Section.Tender.Models.View
{
    public class TenderFilledFormIssueCreateUpdateVM
    {
        public long? id { get; set; }
        public long? pKey { get; set; }
        public int? pfId { get; set; }
        public string issueDate { get; set; }
        public string insuranceNumber { get; set; }
        public string description { get; set; }
        public IFormFile minPic { get; set; }
    }
}
