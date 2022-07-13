using Microsoft.AspNetCore.Http;

namespace Oje.ProposalFormService.Models.View
{
    public class ProposalFilledFormIssueVM
    {
        public long? id { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public string insuranceNumber { get; set; }
        public string description { get; set; }
        public IFormFile mainFile { get; set; }
    }
}
