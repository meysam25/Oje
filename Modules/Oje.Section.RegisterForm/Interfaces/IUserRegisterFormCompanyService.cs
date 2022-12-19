using Oje.Infrastructure.Models;
using Oje.Section.RegisterForm.Models.DB;
using Oje.Section.RegisterForm.Models.View;

namespace Oje.Section.RegisterForm.Interfaces
{
    public interface IUserRegisterFormCompanyService
    {
        ApiResult Create(UserRegisterFormCompanyCreateUpdateVM input, int? siteSettingId);
        ApiResult Delete(int? id, int? siteSettingId);
        object GetById(int? id, int? siteSettingId);
        ApiResult Update(UserRegisterFormCompanyCreateUpdateVM input, int? siteSettingId);
        GridResultVM<UserRegisterFormCompanyMainGridResultVM> GetList(UserRegisterFormCompanyMainGrid searchInput, int? siteSettingId);
        object GetLightList(int? formId, int? siteSettingId, bool? all);
        Company GetCompanyBy(int companyId, int? siteSettingId);
    }
}
