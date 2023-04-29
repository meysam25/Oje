using Oje.Infrastructure.Models;
using Oje.Section.InsuranceContractBaseData.Models.View;

namespace Oje.Section.InsuranceContractBaseData.Interfaces
{
    public interface ITreatmentMainSliderService
    {
        ApiResult Create(TreatmentMainSliderCreateUpdateVM input, int? siteSettingId, long? loginUserId);
        ApiResult Delete(int? id, int? siteSettingId);
        object GetById(int? id, int? siteSettingId);
        GridResultVM<TreatmentMainSliderMainGridResultVM> GetList(TreatmentMainSliderMainGrid searchInput, int? siteSettingId);
        ApiResult Update(TreatmentMainSliderCreateUpdateVM input, int? siteSettingId, long? loginUserId);
        object GetListFormWebsite(int? siteSettingId);

    }
}
