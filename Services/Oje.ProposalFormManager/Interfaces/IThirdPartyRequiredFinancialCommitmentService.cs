using Oje.ProposalFormService.Models.DB;
using System.Collections.Generic;

namespace Oje.ProposalFormService.Interfaces
{
    public interface IThirdPartyRequiredFinancialCommitmentService
    {
        List<ThirdPartyRequiredFinancialCommitment> GetByIds(List<int> coverIds);
        object GetLightList(int? siteSettingId);
        object GetLightListShortTitle(int? siteSettingId);
        List<int> GetAllAcitve();
    }
}
