using Oje.Infrastructure.Models;
using Oje.Section.RegisterForm.Models.DB;
using Oje.Section.RegisterForm.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.RegisterForm.Interfaces
{
    public interface IUserRegisterFormRequiredDocumentService
    {
        ApiResult Create(UserRegisterFormRequiredDocumentCreateUpdateVM input, int? siteSettingId);
        ApiResult Delete(int? id, int? siteSettingId);
        object GetById(int? id, int? siteSettingId);
        ApiResult Update(UserRegisterFormRequiredDocumentCreateUpdateVM input, int? siteSettingId);
        GridResultVM<UserRegisterFormRequiredDocumentMainGridResultVM> GetList(UserRegisterFormRequiredDocumentMainGrid searchInput, int? siteSettingId);
        object GetLightList(int? siteSettingId, int? formId);
        List<UserRegisterFormRequiredDocument> GetRequiredDocuments(int formId, int? siteSettingId);
    }
}
