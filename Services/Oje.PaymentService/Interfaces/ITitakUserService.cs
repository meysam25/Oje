using Oje.Infrastructure.Models;
using Oje.PaymentService.Models.DB;
using Oje.PaymentService.Models.View;

namespace Oje.PaymentService.Interfaces
{
    public interface ITitakUserService
    {
        ApiResult CreateUpdate(TitakUserCreateUpdateVM input, int? siteSettingId);
        object GetBy(int? siteSettingId);
        TitakUser GetDbModelBy(int? siteSettingId);
    }
}
