using Oje.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormManager.Interfaces
{
    public interface IProposalFilledFormUseManager
    {
        void Create(long? userId, ProposalFilledFormUserType type, long? fromUserId, long proposalFilledFormId);
        void Update(long? userId, ProposalFilledFormUserType type, long? fromUserId, long proposalFilledFormId);
    }
}
