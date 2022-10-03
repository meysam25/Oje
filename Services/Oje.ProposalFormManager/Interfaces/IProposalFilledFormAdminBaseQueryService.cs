using Oje.Infrastructure.Enums;
using Oje.ProposalFormService.Models.DB;
using System.Linq;

namespace Oje.ProposalFormService.Interfaces
{
    public interface IProposalFilledFormAdminBaseQueryService
    {
        IQueryable<ProposalFilledForm> getProposalFilledFormBaseQuery(int? siteSettingId, long? userId, ProposalFilledFormStatus status, bool? canSeeOtherWebsites);
        IQueryable<ProposalFilledForm> getProposalFilledFormBaseQuery(int? siteSettingId, long? userId, bool? canSeeOtherWebsites);
        string getControllerNameByStatus(ProposalFilledFormStatus status);
    }
}
