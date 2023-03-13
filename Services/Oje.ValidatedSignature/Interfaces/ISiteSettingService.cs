using Oje.Infrastructure.Models;
using Oje.ValidatedSignature.Models.View;

namespace Oje.ValidatedSignature.Interfaces
{
    public interface ISiteSettingService
    {
        object GetBy(long? id);
        GridResultVM<SiteSettingMainGridResultVM> GetList(SiteSettingMainGrid searchInput);
    }
}
