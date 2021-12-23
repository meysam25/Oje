using Oje.Infrastructure.Enums;
using Oje.PaymentService.Models.View.SizPay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.PaymentService.Interfaces
{
    public interface IBankAccountSizpayPaymentService
    {
        Task<GenerateTokenMethodResultVM> GenerateToken(string bankAccountFactorId, int? siteSettingId, long? loginUserId);
        Task<string> ConfirmPayment(SizpayConfirmPaymentInput input, int? siteSettingId);
    }
}
