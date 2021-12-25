using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Interfac;
using Oje.ProposalFormService.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormService.Interfaces
{
    public interface IProposalFilledFormAdminBaseQueryService
    {
        IQueryable<ProposalFilledForm> getProposalFilledFormBaseQuery(int? siteSettingId, long? userId, ProposalFilledFormStatus status);
        IQueryable<ProposalFilledForm> getProposalFilledFormBaseQuery(int? siteSettingId, long? userId);
        string getControllerNameByStatus(ProposalFilledFormStatus status);
    }
}
