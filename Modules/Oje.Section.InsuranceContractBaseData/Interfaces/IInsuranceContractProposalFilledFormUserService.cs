using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;
using Oje.Section.InsuranceContractBaseData.Models.View;
using System.Collections.Generic;

namespace Oje.Section.InsuranceContractBaseData.Interfaces
{
    public interface IInsuranceContractProposalFilledFormUserService
    {
        void Create(List<IdTitle> familyRelations, List<IdTitle> familyCTypes, long insuranceContractProposalFilledFormId, contractUserInput contractInfo, int? siteSettingId, string fileName, Microsoft.AspNetCore.Http.IFormCollection form, long? loginUserId, List<string> descriptions);
        ApiResult UpdateStatus(InsuranceContractProposalFilledFormChangeStatusVM input, int? siteSettingId, long? loginUserId, InsuranceContractProposalFilledFormType status);
        ApiResult UpdatePrice(InsuranceContractProposalFilledFormChangePriceVM input, int? siteSettingId, long? loginUserId, InsuranceContractProposalFilledFormType status);
        object GetPrice(long? id, int? siteSettingId, InsuranceContractProposalFilledFormType status);
        object GetList(InsuranceContractProposalFilledFormDetailesMainGrid searchInput, int? siteSettingId, InsuranceContractProposalFilledFormType status);
        long GetBy(long filledFormId, long currUserId);
        object GetAddress(long? id, int? siteSettingId, InsuranceContractProposalFilledFormType status);
        List<FamilyMember> GetByFormId(long insuranceContractProposalFilledFormId);
    }
}
