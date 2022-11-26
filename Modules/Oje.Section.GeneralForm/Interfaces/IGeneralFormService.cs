using Oje.Infrastructure.Models;
using Oje.Section.GlobalForms.Models.DB;
using Oje.Section.GlobalForms.Models.View;

namespace Oje.Section.GlobalForms.Interfaces
{
    public interface IGeneralFormService
    {
        ApiResult Create(GeneralFormCreateUpdateVM input);
        ApiResult Delete(long? id);
        object GetById(long? id);
        GridResultVM<GeneralFormMainGridResultVM> GetList(GeneralFormMainGrid searchInput);
        object GetSelect2List(Select2SearchVM searchInput, int? siteSettingId = null);
        ApiResult Update(GeneralFormCreateUpdateVM input);

        string GetJSonConfigFile(int id, int? siteSettingId);
        bool Exist(long id, int? siteSettingId);
        GeneralForm GetByIdNoTracking(long id, int? siteSettingId);
    }
}
