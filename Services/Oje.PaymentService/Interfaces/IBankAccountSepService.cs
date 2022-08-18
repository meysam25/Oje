using Oje.Infrastructure.Models;
using Oje.PaymentService.Models.DB;
using Oje.PaymentService.Models.View;

namespace Oje.PaymentService.Interfaces
{
    public interface IBankAccountSepService
    {
        ApiResult Create(BankAccountSepCreateUpdateVM input, int? siteSettingId);
        ApiResult Delete(int? id, int? siteSettingId);
        object GetById(int? id, int? siteSettingId);
        ApiResult Update(BankAccountSepCreateUpdateVM input, int? siteSettingId);
        GridResultVM<BankAccountSepMainGridResultVM> GetList(BankAccountSepMainGrid searchInput, int? siteSettingId);
        BankAccountSep GetBy(int bankAccountId, int? siteSettingId);
        bool Exist(int bankAccountId, int? siteSettingId);
    }
}
