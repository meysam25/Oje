using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;
using Oje.PaymentService.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.PaymentService.Interfaces
{
    public interface IBankAccountService
    {
        ApiResult Create(BankAccountCreateUpdateVM input, int? siteSettingId);
        ApiResult Delete(int? id, int? siteSettingId);
        object GetById(int? id, int? siteSettingId);
        ApiResult Update(BankAccountCreateUpdateVM input, int? siteSettingId);
        GridResultVM<BankAccountMainGridResultVM> GetList(BankAccountMainGrid searchInput, int? siteSettingId);
        object GetSelect2List(Select2SearchVM searchInput, int? siteSettingId);
        bool Exist(int? siteSettingId, int? id);
        List<BankAccountPaymentUserVM> GetAllAcountForPayment(long? userId, int? siteSettingId);
        bool Exist(long? userId, long? bankAccountId, int? siteSettingId);
    }
}
