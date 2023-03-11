using Oje.Infrastructure.Models;
using Oje.ValidatedSignature.Models.View;

namespace Oje.ValidatedSignature.Interfaces
{
    public interface ITenderFilledFormService
    {
        object GetBy(long? id);
        GridResultVM<TenderFilledFormMainGridResultVM> GetList(TenderFilledFormMainGrid searchInput);
    }
}
