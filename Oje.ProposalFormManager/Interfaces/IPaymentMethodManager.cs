using Oje.Infrastructure.Models;
using Oje.ProposalFormService.Models.DB;
using Oje.ProposalFormService.Models.View;
using System;
using System.Collections.Generic;

namespace Oje.ProposalFormService.Interfaces
{
    public interface IPaymentMethodService
    {
        List<PaymentMethod> GetList(int? siteSettingId, int? proposalFormId);
        bool Exist(int? siteSettingId, int proposalFormId, int companyId);
        List<IdTitle> GetLightList(int proposalFormId, int? siteSettingId, int companyId);
        PaymentMethodDetailesVM GetItemDetailes(int paymentMethodId, int? siteSettingId, long inquiryPrice, int proposalFormId);
        bool IsValid(int id, int? siteSettingId, int proposalFormId, int companyId);
    }
}
