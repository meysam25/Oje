using Oje.Infrastructure.Models;
using Oje.Section.Tender.Models.View;

namespace Oje.Section.Tender.Interfaces
{
    public interface ITenderConfigService
    {
        ApiResult CreateUpdate(TenderConfigCreateUpdateVM input, int? siteSettingId);
        TenderConfigCreateUpdateVM GetBy(int? siteSettingId);
    }
}
