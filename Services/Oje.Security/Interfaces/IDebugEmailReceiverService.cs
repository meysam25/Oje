using Oje.Infrastructure.Models;
using Oje.Security.Models.View;

namespace Oje.Security.Interfaces
{
    public interface IDebugEmailReceiverService
    {
        ApiResult Create(DebugEmailReceiverCreateUpdateVM input);
        ApiResult Delete(int? id);
        object GetById(int? id);
        ApiResult Update(DebugEmailReceiverCreateUpdateVM input);
        GridResultVM<DebugEmailReceiverMainGridResultVM> GetList(DebugEmailReceiverMainGrid searchInput);
    }
}
