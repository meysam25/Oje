using Oje.AccountService.Interfaces;
using Oje.AccountService.Models.View;
using Oje.Infrastructure.Services;

namespace Oje.AccountService.Services
{
    public class SiteInfoService: ISiteInfoService
    {
        readonly IUserService UserService = null;
        readonly ISiteSettingService SiteSettingService = null;
        public SiteInfoService(
                IUserService UserService,
                ISiteSettingService SiteSettingService
            )
        {
            this.UserService = UserService;
            this.SiteSettingService = SiteSettingService;
        }

        public SiteInfoVM GetInfo()
        {
            long? loginUserId = UserService.GetLoginUser()?.UserId;
            return new()
            {
                siteSettingId = SiteSettingService.GetSiteSetting()?.Id,
                loginUserId = loginUserId,
                childUserIds = loginUserId != null && loginUserId > 0 ? UserService.GetChildsUserId(loginUserId.ToLongReturnZiro()) : null 
            };
        }
    }
}
