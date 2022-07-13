using Oje.Infrastructure.Models;
using Oje.Section.Question.Interfaces;
using Oje.Section.Question.Services.EContext;
using System.Collections.Generic;
using System.Linq;

namespace Oje.Section.Question.Services
{
    public class ProposalFormService : IProposalFormService
    {
        readonly QuestionDBContext db = null;

        public ProposalFormService(QuestionDBContext db)
        {
            this.db = db;
        }

        public bool Exist(int? siteSettingId, int? id)
        {
            return db.ProposalForms.Any(t => (t.SiteSettingId == null || t.SiteSettingId == siteSettingId) && t.Id == id);
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
    }
}
