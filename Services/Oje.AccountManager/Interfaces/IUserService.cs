using Microsoft.AspNetCore.Http;
using Oje.AccountService.Models.DB;
using Oje.AccountService.Models.View;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Models.PageForms;
using System.Collections.Generic;

namespace Oje.AccountService.Interfaces
{
    public interface IUserService
    {
        ApiResult Login(LoginVM input, int? siteSettingId);
        ApiResult Create(CreateUpdateUserVM input, long? userId);
        ApiResult Delete(long? id);
        CreateUpdateUserVM GetById(long? id);
        ApiResult Update(CreateUpdateUserVM input, long? userId);
        GridResultVM<AdminUserGridResult> GetList(UserServiceMainGrid searchInput);
        List<Models.DB.Action> GetUserSections(long userId);
        LoginUserVM GetLoginUser();
        void Logout(LoginUserVM loginUserVM);
        List<long> GetUserIdByRoleIds(List<int> roleIds);
        List<long> GetChildsUserId(long userId);
        ApiResult CreateForUser(CreateUpdateUserForUserVM input, long? loginUserId, LoginUserVM loginUserVM, int? siteSettingId);
        ApiResult DeleteForUser(long? id, LoginUserVM loginUserVM, int? siteSettingId);
        CreateUpdateUserForUserVM GetByIdForUser(long? id, LoginUserVM loginUserVM, int? siteSettingId);
        ApiResult UpdateForUser(CreateUpdateUserForUserVM input, long? loginUserId, LoginUserVM loginUserVM, int? siteSettingId);
        GridResultVM<UserServiceForUserMainGridResultVM> GetListForUser(UserServiceForUserMainGrid searchInput, LoginUserVM loginUserVM, int? siteSettingId);
        User GetBy(string username, int? siteSettingId);
        object GetUserInfoByUserId(long? userId);
        long GetUserIdByNationalEmailMobleEcode(string nationalCode, string mobile, string email, string eCode, List<long?> childUserIds, int? siteSettingId);
        void DeleteFlag(long userId, int? siteSettingId, List<long> childUserIds);
        bool IsValidUser(long userId, int? siteSettingId, List<long> childUserIds, RoleType? Type);
        object GetSelect2ListByType(Select2SearchVM searchInput, RoleType? rType);
        object GetSelect2ListByPPFAndCompanyId(Select2SearchVM searchInput, int? siteSettingId, int proposalFormId, int companyId, ProvinceAndCityVM provinceAndCityInput);
        string GetUserFullName(int? siteSettingId, long? userId);
        bool IsValidAgent(long id, int? siteSettingId, int proposalFormId, int companyId);
        bool IsValidAgent(long id, int? siteSettingId, int proposalFormId);
        object GetSelect2List(Select2SearchVM searchInput, int? siteSettingId);
        void setCookieForThisUser(User newUser, LoginVM input);
        void UpdatePassword(User user, string password);
        object GetUserInfoBy(long? userId);
    }
}
