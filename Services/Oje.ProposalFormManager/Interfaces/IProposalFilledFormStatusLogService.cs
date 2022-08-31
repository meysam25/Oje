using Oje.Infrastructure.Enums;
using Oje.ProposalFormService.Models.View;
using System;
using System.Collections.Generic;

namespace Oje.ProposalFormService.Interfaces
{
    public interface IProposalFilledFormStatusLogService
    {
        void Create(long? proposalFilledFormId, ProposalFilledFormStatus? status, DateTime now, long? userId, string description, string fullname, List<ProposalFilledFormChangeStatusFileVM> fileList, int? siteSettingId);
        object GetList(ProposalFilledFormLogMainGrid searchInput, int? siteSettingId, long? userId, List<ProposalFilledFormStatus> validStatus = null);
    }
}
