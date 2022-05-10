using Microsoft.AspNetCore.Http;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;
using Oje.Section.InsuranceContractBaseData.Models.View;
using System.Collections.Generic;

namespace Oje.Section.InsuranceContractBaseData.Interfaces
{
    public interface IInsuranceContractProposalFilledFormService
    {
        ApiResult Create(long? loginUserId, int? siteSettingId, IFormCollection request);
        InsuranceContractProposalFilledFormDetaileVM Detaile(long? id, long? loginUserId, int? siteSettingId, bool ignoreLoginUserId = false, List<InsuranceContractProposalFilledFormType> status = null);
        ApiResult Delete(long? id, int? siteSettingId, InsuranceContractProposalFilledFormType status);
        GridResultVM<InsuranceContractProposalFilledFormMainGridResultVM> GetList(InsuranceContractProposalFilledFormMainGrid searchInput, int? siteSettingId, InsuranceContractProposalFilledFormType status);
        object GetPPFImageList(GlobalGridParentLong input, int? siteSettingId, InsuranceContractProposalFilledFormType status);
        object GetStatus(long? id, int? siteSettingId, InsuranceContractProposalFilledFormType status);
    }
}
