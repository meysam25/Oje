using Oje.Infrastructure.Enums;
using Oje.ProposalFormService.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormService.Interfaces
{
    public interface IInquiryCompanyLimitService
    {
        List<Company> GetCompanies(int? siteSettingId, InquiryCompanyLimitType thirdParty);
    }
}
