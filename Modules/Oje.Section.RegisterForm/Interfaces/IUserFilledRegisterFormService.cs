using Microsoft.AspNetCore.Http;
using Oje.Infrastructure.Models;
using Oje.Section.RegisterForm.Models.View;
using System.Collections.Generic;

namespace Oje.Section.RegisterForm.Interfaces
{
    public interface IUserFilledRegisterFormService
    {
        object Create(int? siteSettingId, IFormCollection form, IpSections ipSections, long? parentUserId, long? userId);
        userFilledRegisterFormDetailesVM PdfDetailes(long? id, int? siteSettingId, long? loginUserId, bool isLoginRequired = false, bool? isPayed = null, bool? isDone = null);
        GridResultVM<UserFilledRegisterFormMainGridResultVM> GetList(UserFilledRegisterFormMainGrid searchInput, int? siteSettingId, bool? isPayed = null, bool? isDone = null);
        object Delete(long? id, int? siteSettingId, bool? isPayed = null, bool? isDone = null);
        object CreateNewUser(long? id, int? siteSettingId, long? parentId, List<int> roleIds, long? loginUserId, bool? isPayed = null, bool? isDone = null);
        object GetUploadImages(GlobalGridParentLong input, int? siteSettingId, bool? isPayed = null, bool? isDone = null);
    }
}
