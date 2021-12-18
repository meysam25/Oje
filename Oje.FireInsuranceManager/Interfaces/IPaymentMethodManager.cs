using Oje.FireInsuranceService.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.FireInsuranceService.Interfaces
{
    public interface IPaymentMethodService
    {
        List<PaymentMethod> GetBy(int? proposalFormId, int? siteSettingId);
    }
}
