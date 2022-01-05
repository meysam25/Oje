using Oje.FireInsuranceService.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.FireInsuranceService.Interfaces
{
    public interface IFireInsuranceRateService
    {
        object Inquiry(int? siteSettingId, FireInsuranceInquiryVM input, string targetArea);
    }
}
