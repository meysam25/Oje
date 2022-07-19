using Oje.AccountService.Interfaces;
using Oje.AccountService.Services.EContext;
using Oje.Infrastructure;
using System;
using System.Linq;

namespace Oje.AccountService.Services
{
    public class LoginBackgroundImageService : ILoginBackgroundImageService
    {
        readonly AccountDBContext db = null;
        public LoginBackgroundImageService(AccountDBContext db)
        {
            this.db = db;
        }

        public object GetRandom(int? siteSettingId)
        {
            return db.LoginBackgroundImages
                .Where(t => t.SiteSettingId == siteSettingId && t.IsActive == true)
                .OrderBy(t => Guid.NewGuid())
                .Select(t => new
                {
                    src = GlobalConfig.FileAccessHandlerUrl + t.ImageUrl
                })
                .FirstOrDefault();
        }
    }
}
