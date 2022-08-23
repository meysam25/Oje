using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.PaymentService.Interfaces;
using Oje.PaymentService.Models.DB;
using Oje.PaymentService.Models.View;
using Oje.PaymentService.Services.EContext;

namespace Oje.PaymentService.Services
{
    public class TitakUserService : ITitakUserService
    {
        readonly PaymentDBContext db = null;
        public TitakUserService(PaymentDBContext db)
        {
            this.db = db;
        }

        public ApiResult CreateUpdate(TitakUserCreateUpdateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);

            var foundItem = db.TitakUsers.Where(t => t.SiteSettingId == siteSettingId).FirstOrDefault();
            if (foundItem == null)
            {
                foundItem = new TitakUser() { SiteSettingId = siteSettingId.Value };
                db.Entry(foundItem).State = EntityState.Added;
            }

            foundItem.Username = input.username;
            foundItem.Password = input.password.Encrypt();

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(TitakUserCreateUpdateVM input, int? siteSettingId)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (string.IsNullOrEmpty(input.username))
                throw BException.GenerateNewException(BMessages.Please_Enter_Username);
            if (string.IsNullOrEmpty(input.password))
                throw BException.GenerateNewException(BMessages.Please_Enter_Password);
            if (input.password.Length > 30)
                throw BException.GenerateNewException(BMessages.Password_Can_Not_Be_More_Then_30_Chars);
            if (input.username.Length > 50)
                throw BException.GenerateNewException(BMessages.Username_Can_Not_Be_More_Then_50_chars);
        }

        public object GetBy(int? siteSettingId)
        {
            return db.TitakUsers
                .OrderByDescending(t => t.SiteSettingId)
                .Where(t => t.SiteSettingId == siteSettingId)
                .Select(t => new
                {
                    username = t.Username
                })
                .Take(1)
                .ToList()
                .Select(t => new 
                {
                    t.username
                })
                .FirstOrDefault();
        }

        public TitakUser GetDbModelBy(int? siteSettingId)
        {
            return db.TitakUsers.Where(t => t.SiteSettingId == siteSettingId).AsNoTracking().FirstOrDefault();
        }
    }
}
