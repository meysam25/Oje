using Oje.Infrastructure.Models;
using Oje.PaymentService.Models.DB;
using Oje.PaymentService.Models.View;

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
        int? GetByCode(int? code);
        Bank GetBy(int id);
    }
}
