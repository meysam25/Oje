using Microsoft.EntityFrameworkCore;
using Oje.FireInsuranceService.Interfaces;
using Oje.FireInsuranceService.Models.DB;
using Oje.FireInsuranceService.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.FireInsuranceService.Services
{
    public class PaymentMethodService: IPaymentMethodService
    {
        readonly FireInsuranceServiceDBContext db = null;
        public PaymentMethodService(FireInsuranceServiceDBContext db)
        {
            this.db = db;
        }

        public List<PaymentMethod> GetBy(int? proposalFormId, int? siteSettingId)
        {
            return db.PaymentMethods.Where(t => t.SiteSettingId == siteSettingId && t.ProposalFormId == proposalFormId).AsNoTracking().ToList();
        }
    }
}
