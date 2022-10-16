using Oje.ProposalFormService.Models.DB;
using System.Collections.Generic;

namespace Oje.ProposalFormService.Interfaces
{
    public interface ICarBodyCreateDatePercentService
    {
        List<CarBodyCreateDatePercent> GetByList();
        List<CarBodyCreateDatePercent> GetByList(int? vehicleTypeId);
    }
}
