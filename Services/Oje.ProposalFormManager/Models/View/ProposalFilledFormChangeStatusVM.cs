using Oje.Infrastructure.Enums;
using System.Collections.Generic;

namespace Oje.ProposalFormService.Models.View
{
    public class ProposalFilledFormChangeStatusVM
    {
        public long? id { get; set; }
        public ProposalFilledFormStatus? status { get; set; }
        public string description { get; set; }
        public string fullname { get; set; }

        public List<ProposalFilledFormChangeStatusFileVM> fileList { get; set; }

    }
}
