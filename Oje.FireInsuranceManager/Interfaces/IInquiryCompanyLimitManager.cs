using Oje.FireInsuranceService.Models.DB;
using Oje.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.FireInsuranceService.Interfaces
{
    public interface IInquiryCompanyLimitService
    {
        List<Company> GetCompanies(int? siteSettingId, InquiryCompanyLimitType type);
    }
}
