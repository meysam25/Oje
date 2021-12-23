using Oje.Infrastructure.Models;
using Oje.PaymentService.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.PaymentService.Interfaces
{
    public interface IBankService
    {
        ApiResult Create(BankCreateUpdateVM input, long? userId);
        ApiResult Delete(int? id);
        object GetById(int? id);
        ApiResult Update(BankCreateUpdateVM input, long? userId);
        GridResultVM<BankMainGridResultVM> GetList(BankMainGrid searchInput);
        object GetLightList();
    }
}
