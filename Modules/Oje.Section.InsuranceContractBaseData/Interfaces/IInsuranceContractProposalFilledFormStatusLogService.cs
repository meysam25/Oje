using Oje.Infrastructure.Enums;
using Oje.Section.InsuranceContractBaseData.Models.View;
using System;
using System.Collections.Generic;

namespace Oje.Section.InsuranceContractBaseData.Interfaces
{
    public interface IInsuranceContractProposalFilledFormStatusLogService
    {
        void Create(long? insuranceContractProposalFilledFormId, InsuranceContractProposalFilledFormType? status, DateTime now, long? loginUserId, string description);
        object GetList(InsuranceContractProposalFilledFormStatusLogGrid searchInput, int? siteSettingId, InsuranceContractProposalFilledFormType status);
        object GetListForUser(InsuranceContractProposalFilledFormStatusLogGrid searchInput, int? siteSettingId, long? loginUserId, List<InsuranceContractProposalFilledFormType> validStatus);
    }
}
