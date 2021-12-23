using Oje.Infrastructure.Exceptions;
using Oje.PaymentService.Interfaces;
using Oje.PaymentService.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
