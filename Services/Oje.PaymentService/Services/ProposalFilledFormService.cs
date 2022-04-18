using Oje.Infrastructure.Exceptions;
using Oje.PaymentService.Interfaces;
using Oje.PaymentService.Services.EContext;

namespace Oje.PaymentService.Services
{
    public class ProposalFilledFormService: IProposalFilledFormService
    {
        readonly PaymentDBContext db = null;
        public ProposalFilledFormService(PaymentDBContext db)
        {
            this.db = db;
        }

        public void UpdateTraceCode(long objectId, string traceNo)
        {
            var foundItem = db.ProposalFilledForms.Where(t => t.Id == objectId).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.PaymentTraceCode = traceNo;
            db.SaveChanges();
        }

        public long ValidateForPayment(int? siteSettingId, long id)
        {
            var foundItem = db.ProposalFilledForms.Where(t => t.SiteSettingId == siteSettingId && t.Id == id && t.IsDelete != true).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);
            if (!string.IsNullOrEmpty(foundItem.PaymentTraceCode))
                throw BException.GenerateNewException(BMessages.Validation_Error);
            if (foundItem.Price <= 0)
                throw BException.GenerateNewException(BMessages.Validation_Error);
            return foundItem.Price;
        }
    }
}
