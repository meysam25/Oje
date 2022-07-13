using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;
using Oje.Section.RegisterForm.Models.DB;
using System;
using System.Collections.Generic;

namespace Oje.Section.RegisterForm.Interfaces
{
    public interface IUserService
    {
        object GetSelect2List(Select2SearchVM searchInput, int? siteSettingId);
        bool Exist(string userName, int? siteSettingId);
        ApiResult CreateNewUser(UserFilledRegisterForm UserFilledRegisterForm, int? siteSettingId,  long? parentUserId, List<int> roleIds);
        void TemproryLogin(long? userId, int? siteSettingId, DateTime expireDate);
        PPFUserTypes GetUserTypePPFInfo(long? loginUserId, ProposalFilledFormUserType resultType);
    }
}
