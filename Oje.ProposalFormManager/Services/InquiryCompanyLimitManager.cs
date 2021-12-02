using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Enums;
using Oje.ProposalFormManager.Interfaces;
using Oje.ProposalFormManager.Models.DB;
using Oje.ProposalFormManager.Services.EContext;
using System.Collections.Generic;
using System.Linq;

namespace Oje.ProposalFormManager.Services
{
    public class InquiryCompanyLimitManager : IInquiryCompanyLimitManager
    {
        readonly ProposalFormDBContext db = null;
        public InquiryCompanyLimitManager(ProposalFormDBContext db)
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
