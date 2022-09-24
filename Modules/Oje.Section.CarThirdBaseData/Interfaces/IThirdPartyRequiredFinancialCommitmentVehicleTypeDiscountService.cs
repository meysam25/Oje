using Oje.Infrastructure.Models;
using Oje.Section.CarThirdBaseData.Models.View;

namespace Oje.Section.CarThirdBaseData.Interfaces
{
    public interface IThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountService
    {
        ApiResult Create(ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountCreateUpdateVM input, int? siteSettingId);
        ApiResult Delete(int? id, int? siteSettingId);
        object GetById(int? id, int? siteSettingId);
        GridResultVM<ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountMainGridResultVM> GetList(ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountMainGrid searchInput, int? siteSettingId);
        ApiResult Update(ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountCreateUpdateVM input, int? siteSettingId);
    }
}
