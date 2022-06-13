using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.Tender.Interfaces;
using Oje.Section.Tender.Services.EContext;
using System.Collections.Generic;
using System.Linq;

namespace Oje.Section.Tender.Services
{
    public class ProposalFormService : IProposalFormService
    {
        readonly TenderDBContext db = null;
        public ProposalFormService
            (
                TenderDBContext db
            )
        {
            this.db = db;
        }

        public bool Exist(int? id, int? siteSettingId)
        {
            return db.ProposalForms.Any(t => t.Id == id && (t.SiteSettingId == null || t.SiteSettingId == siteSettingId));
        }

        public object GetSelect2List(Select2SearchVM searchInput, int? siteSettingId)
        {
            List<object> result = new List<object>();

            var hasPagination = false;
            int take = 50;

            if (searchInput == null)
                searchInput = new Select2SearchVM();
            if (searchInput.page == null || searchInput.page <= 0)
                searchInput.page = 1;

            var qureResult = db.ProposalForms.OrderByDescending(t => t.Id).Where(t => t.SiteSettingId == siteSettingId || t.SiteSettingId == null);
            if (!string.IsNullOrEmpty(searchInput.search))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.search));
            qureResult = qureResult.Skip((searchInput.page.Value - 1) * take).Take(take);
            if (qureResult.Count() >= 50)
                hasPagination = true;

            result.AddRange(qureResult.Select(t => new { id = t.Id, text = t.Title }).ToList());

            return new { results = result, pagination = new { more = hasPagination } };
        }

        public object GetSelect2List(Select2SearchVM searchInput, int? siteSettingId, int? insuranceCatId)
        {
            List<object> result = new List<object>();

            var hasPagination = false;
            int take = 50;

            if (searchInput == null)
                searchInput = new Select2SearchVM();
            if (searchInput.page == null || searchInput.page <= 0)
                searchInput.page = 1;

            var qureResult = db.ProposalForms
                .OrderByDescending(t => t.Id)
                .Where(t => t.TenderProposalFormJsonConfigs.Any(tt => tt.IsActive == true))
                .Where(t => t.SiteSettingId == siteSettingId || t.SiteSettingId == null);
            if (insuranceCatId.ToIntReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.ProposalFormCategoryId == insuranceCatId);
            if (!string.IsNullOrEmpty(searchInput.search))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.search));
            qureResult = qureResult.Skip((searchInput.page.Value - 1) * take).Take(take);
            if (qureResult.Count() >= 50)
                hasPagination = true;

            result.AddRange(qureResult.Select(t => new { id = t.Id, text = t.Title }).ToList());

            return new { results = result, pagination = new { more = hasPagination } };
        }
    }
}
