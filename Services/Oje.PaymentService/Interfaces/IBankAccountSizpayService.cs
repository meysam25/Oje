using Oje.Infrastructure.Models;
using Oje.PaymentService.Models.DB;
using Oje.PaymentService.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.PaymentService.Interfaces
{
    public interface IBankAccountSizpayService
    {
        ApiResult Create(BankAccountSizpayCreateUpdateVM input, int? siteSettingId);
        ApiResult Delete(int? id, int? siteSettingId);
        object GetById(int? id, int? siteSettingId);
        ApiResult Update(BankAccountSizpayCreateUpdateVM input, int? siteSettingId);
        GridResultVM<BankAccountSizpayMainGridResultVM> GetList(BankAccountSizpayMainGrid searchInput, int? siteSettingId);
        BankAccountSizpay GetBy(int bankAccountId, int? siteSettingId);
        bool Exist(int bankAccountId, int? siteSettingId);
    }
}
