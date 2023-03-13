using Oje.Infrastructure.Models;
using Oje.ValidatedSignature.Models.View;

namespace Oje.ValidatedSignature.Interfaces
{
    public interface IBankAccountFactorService
    {
        object GetBy(string id);
        GridResultVM<BankAccountFactorMainGridResultVM> GetList(BankAccountFactorMainGrid searchInput);
    }
}
