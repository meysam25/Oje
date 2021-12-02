using Oje.Infrastructure.Models;
using Oje.Section.ProposalFormBaseData.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.ProposalFormBaseData.Interfaces
{
    public interface IPaymentMethodFileManager
    {
        ApiResult Create(CreateUpdatePaymentMethodFileVM input);
        ApiResult Delete(int? id);
        object GetById(int? id);
        ApiResult Update(CreateUpdatePaymentMethodFileVM input);
        GridResultVM<PaymentMethodFileMainGridResult> GetList(PaymentMethodFileMainGrid searchInput);
    }
}
