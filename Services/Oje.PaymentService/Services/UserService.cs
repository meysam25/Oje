using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Models;
using Oje.PaymentService.Interfaces;
using Oje.PaymentService.Models.DB;
using Oje.PaymentService.Services.EContext;

namespace Oje.PaymentService.Services
{
    public class UserService : IUserService
    {
        readonly PaymentDBContext db = null;
        public UserService(PaymentDBContext db)
        {
            this.db = db;
        }

        public User GetBy(long? loginUserId, int? siteSettingId)
        {
            return db.Users.Where(t => t.Id == loginUserId && t.SiteSettingId == siteSettingId).AsNoTracking().FirstOrDefault();
        }

        public long GetMainPaymentUserId(int? siteSettingId)
        {
            var parentUserId = db.Users.Where(t => t.ParentId == null).Select(t => t.Id).FirstOrDefault();
            return db.Users.Where(t => t.ParentId == parentUserId && t.SiteSettingId == siteSettingId).OrderBy(t => t.Id).Select(t => t.Id).FirstOrDefault();
        }

        public PPFUserTypes GetUserType(long? userId)
        {
            return db.Users.Where(t => t.Id == userId).Select(t => new PPFUserTypes 
            {
                emaile = t.Email,
                fullUserName = t.Firstname + " " + t.Lastname,
                mobile = t.Username,
                userId = userId > 0 ? userId.Value : 0,
                ProposalFilledFormUserType  = Infrastructure.Enums.ProposalFilledFormUserType.OwnerUser
            }).FirstOrDefault();
        }
    }
}
