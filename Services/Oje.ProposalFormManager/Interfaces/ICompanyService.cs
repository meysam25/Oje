using Oje.Infrastructure.Enums;
using Oje.ProposalFormService.Models.DB;

namespace Oje.ProposalFormService.Interfaces
{
    public interface ICompanyService
    {
        object GetLightList(long? userId);
        Company GetById(int? id);
        object GetLightListForInquiryDD();
        object GetLightListForType(int? siteSettingId, InquiryCompanyLimitType type);
    }
}
