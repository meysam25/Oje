using Microsoft.AspNetCore.Http;

namespace Oje.Section.Tender.Models.View
{
    public class TenderFilledFormIssueVM
    {
        public long? id { get; set; }
        public string issueDate { get; set; }
        public string insuranceNumber { get; set; }
        public IFormFile minPic { get; set; }
        public string description { get; set; }
    }
}
