using Oje.Infrastructure.Models;
using Oje.Section.ProposalFormBaseData.Models.View;

namespace Oje.Section.ProposalFormBaseData.Interfaces
{
    public interface IPaymentMethodManager
    {
        ApiResult Create(CreateUpdatePaymentMethodVM input);
        ApiResult Delete(int? id);
        object GetById(int? id);
        ApiResult Update(CreateUpdatePaymentMethodVM input);
        GridResultVM<PaymentMethodMainGridResult> GetList(PaymentMethodMainGrid searchInput);
        object GetLightList();
    }
}
