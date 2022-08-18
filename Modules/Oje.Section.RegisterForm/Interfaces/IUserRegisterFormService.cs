using Oje.Infrastructure.Models;
using Oje.Section.RegisterForm.Models.DB;
using Oje.Section.RegisterForm.Models.View;

namespace Oje.Section.RegisterForm.Interfaces
{
    public interface IUserRegisterFormService
    {
        ApiResult Create(UserRegisterFormCreateUpdateVM input, int? siteSettingId);
        ApiResult Delete(int? id, int? siteSettingId);
        UserRegisterFormCreateUpdateVM GetById(int? id, int? siteSettingId);
        ApiResult Update(UserRegisterFormCreateUpdateVM input, int? siteSettingId);
        GridResultVM<UserRegisterFormMainGridResultVM> GetList(UserRegisterFormMainGrid searchInput, int? siteSettingId);
        object GetLightList(int? siteSettingId);
        bool Exist(int? id, int? siteSettingId);
        string GetConfigJson(int? id, int? isiteSettingId);
        UserRegisterForm GetBy(string formName, int? id, int? iisiteSettingId);
        UserRegisterForm GetTermInfo(int id, int? siteSettingId);
        string GetSecoundFileUrl(int? id, int? siteSettingId);
        string GetAnotherFileUrl(int? id, int? siteSettingId);
        object GetLightList2(int? siteSettingId);
        string GetAnotherFile2Url(int? id, int? siteSettingId);
    }
}
