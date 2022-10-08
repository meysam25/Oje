using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;
using System.Collections.Generic;

namespace Oje.ProposalFormService.Interfaces
{
    public interface IProposalFilledFormUseService
    {
        void Create(long? userId, ProposalFilledFormUserType type, long? fromUserId, long proposalFilledFormId, int? siteSettingId);
        void Update(long? userId, ProposalFilledFormUserType type, long? fromUserId, long proposalFilledFormId, int? siteSettingId);
        bool HasAny(long proposalFilledFormId, ProposalFilledFormUserType type);
        List<PPFUserTypes> GetProposalFilledFormUserIds(long proposalFilledFormId);
    }
}
