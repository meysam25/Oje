using Oje.FireInsuranceService.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.FireInsuranceService.Interfaces
{
    public interface IInquiryDurationService
    {
        object GetLightList(int? siteSettingId, int? proposalFormId);
        InquiryDuration GetBy(int? siteSettingId, int? proposalFormId, int? id);
    }
}
