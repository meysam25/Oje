using Oje.AccountManager.Interfaces;
using Oje.AccountManager.Models.View;
using Oje.Infrastructure.Services;

namespace Oje.AccountManager.Services
{
    public class SiteInfoManager: ISiteInfoManager
    {
        readonly IUserManager UserManager = null;
        readonly ISiteSettingManager SiteSettingManager = null;
        public SiteInfoManager(
                IUserManager UserManager,
                ISiteSettingManager SiteSettingManager
            )
        {
            this.UserManager = UserManager;
            this.SiteSettingManager = SiteSettingManager;
        }

        public SiteInfoVM GetInfo()
        {
            long? loginUserId = UserManager.GetLoginUser()?.UserId;
            return new()
            {
                siteSettingId = SiteSettingManager.GetSiteSetting()?.Id,
                loginUserId = loginUserId,
                childUserIds = loginUserId != null && loginUserId > 0 ? UserManager.GetChildsUserId(loginUserId.ToLongReturnZiro()) : null 
            };
        }
    }
}
