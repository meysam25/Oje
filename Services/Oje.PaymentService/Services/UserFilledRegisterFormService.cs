using Oje.Infrastructure.Exceptions;
using Oje.PaymentService.Interfaces;
using Oje.PaymentService.Services.EContext;

namespace Oje.PaymentService.Services
{
    public class UserFilledRegisterFormService: IUserFilledRegisterFormService
    {
        readonly PaymentDBContext db = null;
        public UserFilledRegisterFormService
            (
                PaymentDBContext db
            )
        {
            this.db = db;
        }

        public void UpdateTraceCode(long id, int? siteSettingId, string traceNo)
        {
            var foundItem = db.UserFilledRegisterForms.Where(t => t.Id == id && t.SiteSettingId == siteSettingId).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.PaymentTraceCode = traceNo;
            db.SaveChanges();
        }
    }
}
