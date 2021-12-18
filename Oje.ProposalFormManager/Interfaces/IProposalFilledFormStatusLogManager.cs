using Oje.Infrastructure.Enums;
using Oje.ProposalFormService.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormService.Interfaces
{
    public interface IProposalFilledFormStatusLogService
    {
        void Create(long? proposalFilledFormId, ProposalFilledFormStatus? status, DateTime now, long? userId, string description);
        object GetList(ProposalFilledFormLogMainGrid searchInput, int? siteSettingId, long? userId);
    }
}
