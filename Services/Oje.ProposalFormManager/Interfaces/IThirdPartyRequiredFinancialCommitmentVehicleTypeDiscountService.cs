using Oje.ProposalFormService.Models.DB;
using System.Collections.Generic;

namespace Oje.ProposalFormService.Interfaces
{
    public interface IThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountService
    {
        List<ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscount> GetListBy(int? vehicleTypeId, int? siteSettingId);
    }
}
