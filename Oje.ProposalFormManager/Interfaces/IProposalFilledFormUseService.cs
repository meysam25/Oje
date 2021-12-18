using Oje.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormService.Interfaces
{
    public interface IProposalFilledFormUseService
    {
        void Create(long? userId, ProposalFilledFormUserType type, long? fromUserId, long proposalFilledFormId);
        void Update(long? userId, ProposalFilledFormUserType type, long? fromUserId, long proposalFilledFormId);
        bool HasAny(long proposalFilledFormId, ProposalFilledFormUserType type);
        List<long> GetProposalFilledFormUserIds(long proposalFilledFormId);
    }
}
