using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormManager.Interfaces
{
    public interface IProposalFilledFormCompanyManager
    {
        void Create(long inquiryId, int? siteSettingId, long proposalFilledFormId, long proposalFilledFormPrice, int companyId);
    }
}
