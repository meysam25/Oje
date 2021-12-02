using Oje.Infrastructure.Models;
using Oje.Section.InquiryBaseData.Interfaces;
using Oje.Section.InquiryBaseData.Services.EContext;
using System.Collections.Generic;
using System.Linq;

namespace Oje.Section.InquiryBaseData.Services
{
    public class ProposalFormManager: IProposalFormManager
    {
        readonly InquiryBaseDataDBContext db = null;
        readonly AccountManager.Interfaces.ISiteSettingManager SiteSettingManager = null;
        public ProposalFormManager(
                InquiryBaseDataDBContext db,
                AccountManager.Interfaces.ISiteSettingManager SiteSettingManager
            )
        {
            this.db = db;
            this.SiteSettingManager = SiteSettingManager;
        }

        public bool Exist(int id, int? siteSettingId)
        {
            return db.ProposalForms.Any(t => t.Id == id && (t.SiteSettingId == siteSettingId || t.SiteSettingId == null));
        }

        public object GetSelect2List(Select2SearchVM searchInput)
        {
            List<object> result = new List<object>();

            var hasPagination = false;
            int take = 50;

            if (searchInput == null)
                searchInput = new Select2SearchVM();
            if (searchInput.page == null || searchInput.page <= 0)
                searchInput.page = 1;

            int? siteSettingId = SiteSettingManager.GetSiteSetting()?.Id;

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
