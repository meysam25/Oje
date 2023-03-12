using Oje.Infrastructure.Models;
using Oje.ValidatedSignature.Models.View;

namespace Oje.ValidatedSignature.Interfaces
{
    public interface ISmsValidationHistoryService
    {
        object GetBy(string id);
        GridResultVM<SmsValidationHistoryMainGridResultVM> GetList(SmsValidationHistoryMainGrid searchInput);
    }
}
