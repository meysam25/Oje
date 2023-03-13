using Oje.Infrastructure.Models;
using Oje.ValidatedSignature.Models.View;

namespace Oje.ValidatedSignature.Interfaces
{
    public interface IBankAccountService
    {
        object GetBy(long? id);
        GridResultVM<BankAccountMainGridResultVM> GetList(BankAccountMainGrid searchInput);
    }
}
