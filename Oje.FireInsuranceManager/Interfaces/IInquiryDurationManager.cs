using Oje.FireInsuranceManager.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.FireInsuranceManager.Interfaces
{
    public interface IInquiryDurationManager
    {
        object GetLightList(int? siteSettingId, int? proposalFormId);
        InquiryDuration GetBy(int? siteSettingId, int? proposalFormId, int? id);
    }
}
