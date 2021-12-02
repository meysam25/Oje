using Microsoft.EntityFrameworkCore;
using Oje.FireInsuranceManager.Interfaces;
using Oje.FireInsuranceManager.Models.DB;
using Oje.FireInsuranceManager.Services.EContext;
using Oje.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.FireInsuranceManager.Services
{
    public class InquiryCompanyLimitManager: IInquiryCompanyLimitManager
    {
        readonly FireInsuranceManagerDBContext db = null;
        public InquiryCompanyLimitManager(FireInsuranceManagerDBContext db)
        {
            this.db = db;
        }

        public List<Company> GetCompanies(int? siteSettingId, InquiryCompanyLimitType type)
        {
            var result = db.InquiryCompanyLimits.Where(t => t.SiteSettingId == siteSettingId && t.Type == type).SelectMany(t => t.InquiryCompanyLimitCompanies).Select(t => t.Company).AsNoTracking().ToList();
            if (result.Count() == 0)
                result = db.Companies.AsNoTracking().ToList();
            return result;
        }
    }
}
