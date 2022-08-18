using Oje.Infrastructure.Models;
using Oje.Section.RegisterForm.Models.DB;
using Oje.Section.RegisterForm.Models.View;

namespace Oje.Section.RegisterForm.Interfaces
{
    public interface IUserRegisterFormPriceService
    {
        ApiResult Create(UserRegisterFormPriceCreateUpdateVM input, int? siteSettingId);
        ApiResult Delete(int? id, int? siteSettingId);
        object GetById(int? id, int? siteSettingId);
        ApiResult Update(UserRegisterFormPriceCreateUpdateVM input, int? siteSettingId);
        GridResultVM<UserRegisterFormPriceMainGridResultVM> GetList(UserRegisterFormPriceMainGrid searchInput, int? siteSettingId);
        object GetLightList(int? siteSettingId, int? formId, string groupKey, string groupKey2);
        UserRegisterFormPrice GetPriceBy(int formId, int? siteSettingId, int id, string groupTitle, string groupTitle2);
        UserRegisterFormPrice GetById(int id);
    }
}
