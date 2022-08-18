using Oje.Infrastructure.Models;
using Oje.Sanab.Models.View;

namespace Oje.Sanab.Interfaces
{
    public interface ISanabCarThirdPartyPlaqueInquiryService
    {
        Task<List<KeyValue>> Discount(int? siteSettingId, CarThirdPartyPlaqueVM input, IpSections ipSections);
    }
}
