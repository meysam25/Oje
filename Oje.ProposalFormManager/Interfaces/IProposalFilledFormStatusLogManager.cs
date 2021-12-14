using Oje.Infrastructure.Enums;
using Oje.ProposalFormManager.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormManager.Interfaces
{
    public interface IProposalFilledFormStatusLogManager
    {
        void Create(long? proposalFilledFormId, ProposalFilledFormStatus? status, DateTime now, long? userId, string description);
        object GetList(ProposalFilledFormLogMainGrid searchInput, int? siteSettingId, long? userId);
    }
}
