using Oje.ProposalFormService.Models.View;

namespace Oje.ProposalFormService.Interfaces
{
    public interface IThirdPartyRateService
    {
        object Inquiry(int? siteSettingId, CarThirdPartyInquiryVM input, string targetArea);
    }
}
