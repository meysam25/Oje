using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using Oje.ProposalFormService.Interfaces;
using Oje.ProposalFormService.Models.DB;
using Oje.ProposalFormService.Services.EContext;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Oje.ProposalFormService.Services
{
    public class CompanyService : ICompanyService
    {
        readonly ProposalFormDBContext db = null;
        public CompanyService(ProposalFormDBContext db)
        {
            this.db = db;
        }

        public Company GetById(int? id)
        {
            return db.Companies.Where(t => t.Id == id).AsNoTracking().FirstOrDefault();
        }

        public object GetLightList(long? userId)
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetAttribute<DisplayAttribute>()?.Name } };

            result.AddRange(db.Users.Where(t => t.Id == userId).SelectMany(t => t.UserCompanies).Select(t => new { id = t.CompanyId, title = t.Company.Title, src = GlobalConfig.FileAccessHandlerUrl + t.Company.Pic32 }).ToList());

            return result;
        }

        public object GetLightListForInquiryDD()
        {
            List<object> result = new List<object>();// { new { id = "", title = BMessages.Please_Select_One_Item.GetAttribute<DisplayAttribute>()?.Name } };
            result.Add(new { id = "0", title = BMessages.I_Dont_Have_One.GetAttribute<DisplayAttribute>()?.Name });

            result.AddRange(db.Companies.Where(t => t.IsActive == true).Select(t => new { id = t.Id, title = t.Title }).ToList());

            return result;
        }

        public object GetLightListForType(int? siteSettingId, InquiryCompanyLimitType type)
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetAttribute<DisplayAttribute>()?.Name } };
            //result.Add(new { id = "0", title = BMessages.I_Dont_Have_One.GetAttribute<DisplayAttribute>()?.Name });

            var allThirdPartyCompany = db.InquiryCompanyLimits.Where(t => t.SiteSettingId == siteSettingId && t.Type == type).SelectMany(t => t.InquiryCompanyLimitCompanies).Where(t => t.Company.IsActive == true).Select(t => new { id = t.CompanyId, title = t.Company.Title, src = GlobalConfig.FileAccessHandlerUrl + t.Company.Pic32 }).ToList();
            if (allThirdPartyCompany.Any())
                result.AddRange(allThirdPartyCompany);
            else
                result.AddRange(db.Companies.Where(t => t.IsActive == true).Select(t => new { id = t.Id, title = t.Title, src = GlobalConfig.FileAccessHandlerUrl + t.Pic32 }).ToList());

            return result;
        }
    }
}
