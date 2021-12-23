using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    }
}
