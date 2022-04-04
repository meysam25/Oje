using Oje.Infrastructure.Enums;
using Oje.Section.InsuranceContractBaseData.Models.View;
using System;

namespace Oje.Section.InsuranceContractBaseData.Interfaces
{
    public interface IInsuranceContractProposalFilledFormStatusLogService
    {
        void Create(long? insuranceContractProposalFilledFormId, InsuranceContractProposalFilledFormType? status, DateTime now, long? loginUserId, string description);
        object GetList(InsuranceContractProposalFilledFormStatusLogGrid searchInput, int? siteSettingId);
    }
}
