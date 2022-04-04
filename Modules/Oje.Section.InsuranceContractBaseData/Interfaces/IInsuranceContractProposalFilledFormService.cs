using Microsoft.AspNetCore.Http;
using Oje.Infrastructure.Models;
using Oje.Section.InsuranceContractBaseData.Models.View;

namespace Oje.Section.InsuranceContractBaseData.Interfaces
{
    public interface IInsuranceContractProposalFilledFormService
    {
        ApiResult Create(long? loginUserId, int? siteSettingId, IFormCollection request);
        InsuranceContractProposalFilledFormDetaileVM Detaile(long? id, long? loginUserId, int? siteSettingId, bool ignoreLoginUserId = false);
        ApiResult Delete(long? id, int? siteSettingId);
        GridResultVM<InsuranceContractProposalFilledFormMainGridResultVM> GetList(InsuranceContractProposalFilledFormMainGrid searchInput, int? siteSettingId);
        object GetPPFImageList(GlobalGridParentLong input, int? siteSettingId);
        object GetStatus(long? id, int? siteSettingId);
        ApiResult UpdateStatus(InsuranceContractProposalFilledFormChangeStatusVM input, int? siteSettingId, long? loginUserId);
        ApiResult UpdatePrice(InsuranceContractProposalFilledFormChangePriceVM input, int? siteSettingId, long? loginUserId);
        object GetPrice(long? id, int? siteSettingId);
    }
}
