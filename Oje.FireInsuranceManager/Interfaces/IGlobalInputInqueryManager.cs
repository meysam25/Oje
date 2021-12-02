using Oje.FireInsuranceManager.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.FireInsuranceManager.Interfaces
{
    public interface IGlobalInputInqueryManager
    {
        long Create(FireInsuranceInquiryVM input, int? siteSettingId);
    }
}
