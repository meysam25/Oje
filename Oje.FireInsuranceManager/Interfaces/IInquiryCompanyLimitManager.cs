using Oje.FireInsuranceManager.Models.DB;
using Oje.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.FireInsuranceManager.Interfaces
{
    public interface IInquiryCompanyLimitManager
    {
        List<Company> GetCompanies(int? siteSettingId, InquiryCompanyLimitType type);
    }
}
