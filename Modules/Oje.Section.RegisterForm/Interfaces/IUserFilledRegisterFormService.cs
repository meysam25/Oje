using Microsoft.AspNetCore.Http;
using Oje.Infrastructure.Models;
using Oje.Section.RegisterForm.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.RegisterForm.Interfaces
{
    public interface IUserFilledRegisterFormService
    {
        object Create(int? siteSettingId, IFormCollection form, Infrastructure.Models.IpSections ipSections, long? parentUserId, long? userId);
        userFilledRegisterFormDetailesVM PdfDetailes(long? id, int? siteSettingId, long? loginUserId);
        GridResultVM<UserFilledRegisterFormMainGridResultVM> GetList(UserFilledRegisterFormMainGrid searchInput, int? siteSettingId);
        object Delete(long? id, int? siteSettingId);
        object CreateNewUser(long? id, int? siteSettingId, long? parentId, List<int> roleIds);
        object GetUploadImages(GlobalGridParentLong input, int? siteSettingId);
    }
}
