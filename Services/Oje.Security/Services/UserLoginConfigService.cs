using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Security.Interfaces;
using Oje.Security.Models.DB;
using Oje.Security.Models.View;
using Oje.Security.Services.EContext;

namespace Oje.Security.Services
{
    public class UserLoginConfigService : IUserLoginConfigService
    {
        readonly SecurityDBContext db = null;
        static Dictionary<int, UserLoginConfig> UserLoginConfigs = new Dictionary<int, UserLoginConfig>();
        public UserLoginConfigService(SecurityDBContext db)
        {
            this.db = db;
        }

        public ApiResult CreateUpdate(UserLoginConfigCreateUpdateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);

            var foundItem = db.UserLoginConfigs.Where(t => t.SiteSettingId == siteSettingId).FirstOrDefault();
            if (foundItem == null)
            {
                foundItem = new UserLoginConfig() { SiteSettingId = siteSettingId.Value };
                db.Entry(foundItem).State = EntityState.Added;
            }

            foundItem.FailCount = input.failCount.Value;
            foundItem.DeactiveMinute = input.deactiveMinute.Value;
            foundItem.InActiveLogoffMinute = input.inActiveLogoffMinute.Value;

            if (UserLoginConfigs.Keys.Any(t => t == siteSettingId))
                UserLoginConfigs[siteSettingId.Value] = null;


            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetBy(int? siteSettingId)
        {
            return db.UserLoginConfigs.Where(t => t.SiteSettingId == siteSettingId).Select(t => new
            {
                failCount = t.FailCount,
                deactiveMinute = t.DeactiveMinute,
                inActiveLogoffMinute = t.InActiveLogoffMinute
            }).FirstOrDefault();
        }

        public UserLoginConfig GetByCache(int? siteSettingId)
        {
            if (!UserLoginConfigs.Keys.Any(t => t == siteSettingId))
            {
                lock (UserLoginConfigs)
                {
                    if (UserLoginConfigs.Keys.Any(t => t == siteSettingId))
                        return UserLoginConfigs[siteSettingId.ToIntReturnZiro()];
                    var foundUserLoginConfig = db.UserLoginConfigs.Where(t => t.SiteSettingId == siteSettingId).AsNoTracking().FirstOrDefault();
                    if (foundUserLoginConfig == null)
                        foundUserLoginConfig = new UserLoginConfig() { DeactiveMinute = 10, SiteSettingId = siteSettingId.ToIntReturnZiro(), FailCount = 5, InActiveLogoffMinute = 60 };

                    UserLoginConfigs[siteSettingId.ToIntReturnZiro()] = foundUserLoginConfig;
                }

            }

            return UserLoginConfigs[siteSettingId.ToIntReturnZiro()];
        }

        private void createUpdateValidation(UserLoginConfigCreateUpdateVM input, int? siteSettingId)
        {
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (input.failCount.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (input.deactiveMinute.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (input.inActiveLogoffMinute.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
        }
    }
}
