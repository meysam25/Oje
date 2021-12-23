using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;
using Oje.PaymentService.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.PaymentService.Interfaces
{
    public interface IBankAccountFactorService
    {
        BankAccountFactor GetById(string bankAccountFactorId, int? siteSettingId);
        bool ExistBy(string traceNo);
        void UpdatePaymentInfor(BankAccountFactor foundAccount, string traceNo);
        string Create(int? bankAccountId, PaymentFactorVM payModel, int? siteSettingId, long? loginUserId);
        List<ProposalFilledFormPaymentVM> GetListBy(BankAccountFactorType type, long objectId, int? siteSettingId);
    }
}
