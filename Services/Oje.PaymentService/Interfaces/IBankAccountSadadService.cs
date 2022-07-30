using Oje.Infrastructure.Models;
using Oje.PaymentService.Models.DB;
using Oje.PaymentService.Models.View;

namespace Oje.PaymentService.Interfaces
{
    public interface IBankAccountSadadService
    {
        ApiResult Create(BankAccountSadadCreateUpdateVM input, int? siteSettingId);
        ApiResult Delete(int? id, int? siteSettingId);
        object GetById(int? id, int? siteSettingId);
        ApiResult Update(BankAccountSadadCreateUpdateVM input, int? siteSettingId);
        GridResultVM<BankAccountSadadMainGridResultVM> GetList(BankAccountSadadMainGrid searchInput, int? siteSettingId);
        bool Exist(int bankAccountId, int? siteSettingId);
        BankAccountSadad GetBy(int bankAccountId, int? siteSettingId);
    }
}
