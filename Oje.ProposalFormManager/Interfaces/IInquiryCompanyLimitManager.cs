using Oje.Infrastructure.Enums;
using Oje.ProposalFormManager.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormManager.Interfaces
{
    public interface IInquiryCompanyLimitManager
    {
        List<Company> GetCompanies(int? siteSettingId, InquiryCompanyLimitType thirdParty);
    }
}
