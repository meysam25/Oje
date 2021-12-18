using Oje.AccountService.Interfaces;
using Oje.Infrastructure.Models;
using Oje.Section.InsuranceContractBaseData.Services.EContext;
using System.Collections.Generic;
using System.Linq;

namespace Oje.Section.InsuranceContractBaseData.Services
{
    public class ProposalFormService: Interfaces.IProposalFormService
    {
        readonly InsuranceContractBaseDataDBContext db = null;
        readonly ISiteSettingService SiteSettingService = null;
        public ProposalFormService(
                InsuranceContractBaseDataDBContext db,
                ISiteSettingService SiteSettingService,
                IUserService UserService
            )
        {
            this.db = db;
            this.SiteSettingService = SiteSettingService;
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

            int? siteSettingId = SiteSettingService.GetSiteSetting()?.Id;

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
