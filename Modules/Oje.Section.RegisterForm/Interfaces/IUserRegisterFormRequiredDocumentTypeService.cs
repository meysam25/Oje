using Oje.Infrastructure.Models;
using Oje.Section.RegisterForm.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.RegisterForm.Interfaces
{
    public interface IUserRegisterFormRequiredDocumentTypeService
    {
        ApiResult Create(UserRegisterFormRequiredDocumentTypeCreateUpdateVM input, int? siteSettingId);
        ApiResult Delete(int? id, int? siteSettingId);
        object GetById(int? id, int? siteSettingId);
        ApiResult Update(UserRegisterFormRequiredDocumentTypeCreateUpdateVM input, int? siteSettingId);
        GridResultVM<UserRegisterFormRequiredDocumentTypeMainGridResultVM> GetList(UserRegisterFormRequiredDocumentTypeMainGrid searchInput, int? siteSettingId);
        object GetLightList(int? siteSettingId);
    }
}
