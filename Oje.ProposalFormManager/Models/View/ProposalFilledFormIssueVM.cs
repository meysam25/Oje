using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormManager.Models.View
{
    public class ProposalFilledFormIssueVM
    {
        public long? id { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public string insuranceNumber { get; set; }
        public string description { get; set; }
    }
}
