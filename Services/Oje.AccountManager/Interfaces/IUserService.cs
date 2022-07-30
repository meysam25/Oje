using Oje.AccountService.Models.DB;
using Oje.AccountService.Models.View;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;
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
        bool CanSeeAllItems(long userId);
        ApiResult CreateForUser(CreateUpdateUserForUserVM input, long? loginUserId, LoginUserVM loginUserVM, int? siteSettingId);
        ApiResult DeleteForUser(long? id, LoginUserVM loginUserVM, int? siteSettingId);
        CreateUpdateUserForUserVM GetByIdForUser(long? id, LoginUserVM loginUserVM, int? siteSettingId);
        ApiResult UpdateForUser(CreateUpdateUserForUserVM input, long? loginUserId, LoginUserVM loginUserVM, int? siteSettingId);
        GridResultVM<UserServiceForUserMainGridResultVM> GetListForUser(UserServiceForUserMainGrid searchInput, LoginUserVM loginUserVM, int? siteSettingId);
        User GetBy(string username, int? siteSettingId);
        object GetBy(long? userId, int? siteSettingId);
        object GetUserInfoByUserId(long? userId);
        void UpdateHashPassword();
        long GetUserIdByNationalEmailMobleEcode(string nationalCode, string mobile, string eCode, long? loginUserId, int? siteSettingId);
        ApiResult DeleteUserActionRequest(string id);
        GridResultVM<UserRequestActionMainGridResultVM> GetRequestUserAccessList(UserRequestActionMainGrid searchInput);
        ApiResult ConfirmUserActionRequest(string id);
        void TsetRemoveMe();
        void DeleteFlag(long? userId, int? siteSettingId, long? childIds);
        bool IsValidUser(long userId, int? siteSettingId, List<long> childUserIds, RoleType? Type);
        bool HasCompany(long? userId, int? companyId);
        object GetSelect2ListByType(Select2SearchVM searchInput, RoleType? rType);
        object GetSelect2ListByPPFAndCompanyId(Select2SearchVM searchInput, int? siteSettingId, int proposalFormId, int companyId, ProvinceAndCityVM provinceAndCityInput, string mapLat, string mapLon);
        ApiResult UpdateUserProfile(UpdateUserForUserVM input, long? userId, int? siteSettingId);
        string GetUserFullName(int? siteSettingId, long? userId);
        bool IsValidAgent(long id, int? siteSettingId, int proposalFormId, int companyId);
        bool IsValidAgent(long id, int? siteSettingId, int proposalFormId);
        object GetSelect2List(Select2SearchVM searchInput, int? siteSettingId);
        void setCookieForThisUser(User newUser, LoginVM input, bool hasAutoRefres);
        void UpdatePassword(User user, string password);
        void UpdateUserSessionFileName(long? id, string lastSessionFileName);
        object CreateForUserFromJson(GlobalExcelFile input, long? userId, LoginUserVM loginUserVM, int? siteSettingId, string websiteUrl);
        object GetUserInfoBy(long? userId);
        void CreateTempTable();
        PPFUserTypes GetUserTypePPFInfo(long? loginUserId, ProposalFilledFormUserType resultType);
        bool isWebsiteUser(long userId);
        (int? cityId, int? provinceId) GetCityAndProvince(long? loginUserId);
        (int? province, int? cityid, List<int> companyIds) GetUserCityCompany(long? userId);
        object GetAgentInfo(long userId);
        void CreateUserAccessRequest(long userId, string requestPath);
    }
}
