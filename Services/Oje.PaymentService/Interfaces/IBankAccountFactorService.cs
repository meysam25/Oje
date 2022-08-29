using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;
using Oje.PaymentService.Models.DB;
using Oje.PaymentService.Models.View;

namespace Oje.PaymentService.Interfaces
{
    public interface IBankAccountFactorService
    {
        BankAccountFactor GetById(string bankAccountFactorId, int? siteSettingId);
        bool ExistBy(string traceNo);
        void UpdatePaymentInfor(BankAccountFactor foundAccount, string traceNo, int? siteSettingId, DateTime? payDate = null);
        string Create(int? bankAccountId, PaymentFactorVM payModel, int? siteSettingId, long? loginUserId);
        List<ProposalFilledFormPaymentVM> GetListBy(BankAccountFactorType type, long objectId, int? siteSettingId);
        BankAccountFactor GetBy(string keyHash, int? siteSettingId);
        void Save();
        BankAccountFactorVM GetByIdView(string bankAccountFactorId, int? siteSettingId);
    }
}
