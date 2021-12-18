using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Enums;
using Oje.ProposalFormService.Interfaces;
using Oje.ProposalFormService.Models.DB;
using Oje.ProposalFormService.Services.EContext;
using System.Collections.Generic;
using System.Linq;

namespace Oje.ProposalFormService.Services
{
    public class InquiryCompanyLimitService : IInquiryCompanyLimitService
    {
        readonly ProposalFormDBContext db = null;
        public InquiryCompanyLimitService(ProposalFormDBContext db)
        {
            this.db = db;
        }

        public List<Company> GetCompanies(int? siteSettingId, InquiryCompanyLimitType type)
        {
            var result = db.InquiryCompanyLimits.Where(t => t.SiteSettingId == siteSettingId && t.Type == type).SelectMany(t => t.InquiryCompanyLimitCompanies).Select(t => t.Company).AsNoTracking().ToList();
            if (result.Count == 0)
                result = db.Companies.AsNoTracking().ToList();

            return result;
        }
    }
}
