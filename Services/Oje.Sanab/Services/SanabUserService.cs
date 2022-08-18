using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Sanab.Interfaces;
using Oje.Sanab.Models.DB;
using Oje.Sanab.Models.View;
using Oje.Sms.Services.EContext;

namespace Oje.Sanab.Services
{
    public class SanabUserService : ISanabUserService
    {
        readonly SanabDBContext db = null;
        public SanabUserService(SanabDBContext db)
        {
            this.db = db;
        }

        public ApiResult CreateUpdate(SanabUserCreateUpdateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);

            var foundItem = db.SanabUsers.Where(t => t.SiteSettingId == siteSettingId).FirstOrDefault();
            if (foundItem == null)
            {
                foundItem = new SanabUser() { SiteSettingId = siteSettingId.Value };
                db.Entry(foundItem).State = EntityState.Added;
            }

            foundItem.Username = input.username;
            if (!string.IsNullOrEmpty(input.password))
                foundItem.Password = input.password;
            foundItem.Token = input.token;
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(SanabUserCreateUpdateVM input, int? siteSettingId)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (string.IsNullOrEmpty(input.username))
                throw BException.GenerateNewException(BMessages.Please_Enter_Username);
            if (input.id.ToIntReturnZiro() <= 0 && string.IsNullOrEmpty(input.password))
                throw BException.GenerateNewException(BMessages.Please_Enter_Password);
            if (string.IsNullOrEmpty(input.token))
                throw BException.GenerateNewException(BMessages.Please_Enter_Token);
        }

        public object GetById(int? siteSettingId)
        {
            return db.SanabUsers
                .Where(t => t.SiteSettingId == siteSettingId)
                .Select(t => new
                {
                    id = t.Id,
                    username = t.Username,
                    isActive = t.IsActive,
                    token = t.Token
                })
                .FirstOrDefault();
        }

        public SanabUser GetActive(int? siteSettingId)
        {
            return db.SanabUsers.Where(t => t.SiteSettingId == siteSettingId && t.IsActive == true).FirstOrDefault();
        }
    }
}
