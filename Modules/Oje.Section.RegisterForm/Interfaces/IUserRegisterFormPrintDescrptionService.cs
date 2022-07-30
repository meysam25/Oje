using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;
using Oje.Section.RegisterForm.Models.View;

namespace Oje.Section.RegisterForm.Interfaces
{
    public interface IUserRegisterFormPrintDescrptionService
    {
        ApiResult Create(UserRegisterFormPrintDescrptionCreateUpdateVM input, int? siteSettingId);
        ApiResult Delete(long? id, int? siteSettingId);
        UserRegisterFormPrintDescrptionCreateUpdateVM GetById(long? id, int? siteSettingId);
        ApiResult Update(UserRegisterFormPrintDescrptionCreateUpdateVM input, int? siteSettingId);
        GridResultVM<UserRegisterFormPrintDescrptionMainGridResultVM> GetList(UserRegisterFormPrintDescrptionMainGrid searchInput, int? siteSettingId);
        string GetBy(int? siteSettingId, int formId, ProposalFormPrintDescrptionType type);
    }
}
