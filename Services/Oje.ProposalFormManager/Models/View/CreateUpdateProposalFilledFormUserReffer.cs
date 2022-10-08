using Oje.Infrastructure.Models;
using System.Collections.Generic;

namespace Oje.ProposalFormService.Models.View
{
    public class CreateUpdateProposalFilledFormUserReffer: GlobalSiteSetting
    {
        public long? id { get; set; }
        public List<long> userIds { get; set; }
    }
}
