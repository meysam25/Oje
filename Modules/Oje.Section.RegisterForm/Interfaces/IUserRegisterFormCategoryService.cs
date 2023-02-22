using Oje.Infrastructure.Models;
using Oje.Section.RegisterForm.Models.View;

namespace Oje.Section.RegisterForm.Interfaces
{
    public interface IUserRegisterFormCategoryService
    {
        ApiResult Create(UserRegisterFormCategoryCreateUpdateVM input, int? siteSettingId);
        ApiResult Delete(int? id, int? siteSettingId);
        object GetById(int? id, int? siteSettingId);
        object GetLightList(int? siteSettingId);
        GridResultVM<UserRegisterFormCategoryMainGridResultVM> GetList(UserRegisterFormCategoryMainGrid searchInput, int? siteSettingId);
        ApiResult Update(UserRegisterFormCategoryCreateUpdateVM input, int? siteSettingId);
    }
}
