using Oje.ProposalFormManager.Models.View;

namespace Oje.ProposalFormManager.Interfaces
{
    public interface IThirdPartyRateManager
    {
        object Inquiry(int? siteSettingId, CarThirdPartyInquiryVM input);
    }
}
