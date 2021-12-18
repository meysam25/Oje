using Microsoft.EntityFrameworkCore;
using Oje.FireInsuranceService.Interfaces;
using Oje.FireInsuranceService.Models.DB;
using Oje.FireInsuranceService.Services.EContext;
using Oje.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.FireInsuranceService.Services
{
    public class InquiryCompanyLimitService: IInquiryCompanyLimitService
    {
        readonly FireInsuranceServiceDBContext db = null;
        public InquiryCompanyLimitService(FireInsuranceServiceDBContext db)
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
