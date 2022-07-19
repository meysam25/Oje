using Oje.AccountService.Interfaces;
using Oje.AccountService.Models.View;
using Oje.AccountService.Services.EContext;
using System.Linq;
using System.Net;

namespace Oje.AccountService.Services
{
    public class LoginDescrptionService : ILoginDescrptionService
    {
        readonly AccountDBContext db = null;
        public LoginDescrptionService
            (
                AccountDBContext db
            )
        {
            this.db = db;
        }

        public object GetBy(int? siteSettingId, string returnUrl)
        {
            returnUrl = WebUtility.UrlEncode(returnUrl);
            return db.LoginDescrptions
                .Where(t => t.SiteSettingId == siteSettingId && t.ReturnUrl == returnUrl && t.IsActive == true)
                .Select(t => new LoginDescrptionResultVM
                {
                    desc = t.Description
                })
                .FirstOrDefault();
        }
    }
}
