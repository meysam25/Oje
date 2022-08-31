using System.Collections.Generic;

namespace Oje.Infrastructure.Models.Pdf.ProposalFilledForm
{
    public class ProposalFilledFormPdfVM
    {
        public string ppfTitle { get; set; }
        public string ppfCreateDate { get; set; }
        public string id { get; set; }
        public long proposalFilledFormId { get; set; }
        public string createUserFullname { get; set; }
        public string traceCode { get; set; }
        public long? price { get; set; }
        public long agentUserId { get; set; }
        public List<ProposalFilledFormPdfGroupVM> ProposalFilledFormPdfGroupVMs { get; set; }
        public string companyTitle { get; set; }
        public string companyImage { get; set; }
        public long loginUserWalletBalance { get; set; }
        public List<ProposalFormPrintDescrptionVM> printDescriptions { get; set; }
        public string issueUploadFile { get; set; }
    }
}
