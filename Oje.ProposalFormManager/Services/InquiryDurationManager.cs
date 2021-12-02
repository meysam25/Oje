using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using Oje.ProposalFormManager.Interfaces;
using Oje.ProposalFormManager.Models.DB;
using Oje.ProposalFormManager.Services.EContext;
using System.Collections.Generic;
using System.Linq;

namespace Oje.ProposalFormManager.Services
{
    public class InquiryDurationManager : IInquiryDurationManager
    {
        readonly ProposalFormDBContext db = null;
        public InquiryDurationManager(ProposalFormDBContext db)
        {
            this.db = db;
        }

        public InquiryDuration GetById(int? siteSettingId, int? proposalFormId, int? id)
        {
            return db.InquiryDurations
                .Include(t => t.InquiryDurationCompanies)
                .Where(t => t.SiteSettingId == siteSettingId && t.ProposalFormId == proposalFormId && t.Id == id && t.IsActive == true)
                .AsNoTracking()
                .FirstOrDefault();
        }

        public object GetLightList(int? siteSettingId, int? proposalFormId)
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetEnumDisplayName() } };

            result.AddRange(db.InquiryDurations.Where(t => t.IsActive == true && t.SiteSettingId == siteSettingId && t.ProposalFormId == proposalFormId).Select(t => new { id = t.Id, title = t.Title }).ToList());

            return result;
        }
    }
}
