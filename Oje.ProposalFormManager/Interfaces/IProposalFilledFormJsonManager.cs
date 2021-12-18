using Oje.ProposalFormService.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormService.Interfaces
{
    public interface IProposalFilledFormJsonService
    {
        void Create(long proposalFilledFormId, string jsonConfig);
        ProposalFilledFormJson GetBy(long proposalFilledFormId);
    }
}
