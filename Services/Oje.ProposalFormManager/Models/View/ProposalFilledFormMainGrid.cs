using Oje.Infrastructure.Models;

namespace Oje.ProposalFormService.Models.View
{
    public class ProposalFilledFormMainGrid : GlobalGrid
    {
        public int? cId { get; set; }
        public string ppfTitle { get; set; }
        public string createDate { get; set; }
        public long? price { get; set; }
        public string agentFullname { get; set; }
        public string targetUserfullname { get; set; }
        public string targetUserMobileNumber { get; set; }
        public string createUserfullname { get; set; }
        public string fromCreateDate { get; set; }
        public string toCreateDate { get; set; }
        public string fromIssueDate { get; set; }
        public string toIssueDate { get; set; }
        public string targetUserNationalCode { get; set; }
    }
}
