using Microsoft.EntityFrameworkCore;
using Oje.AccountManager.Interfaces;
using Oje.AccountManager.Services.EContext;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.AccountManager.Services
{
    public class ProposalFormManager : IProposalFormManager
    {
        readonly AccountDBContext db = null;
        public ProposalFormManager(AccountDBContext db)
        {
            this.db = db;
        }

        public bool Exist(int? sitesettingId, int formId)
        {
            return db.ProposalForms.Any(t => t.Id == formId && (t.SiteSettingId == null || t.SiteSettingId == sitesettingId));
        }

        public object GetightListForSelect2(Select2SearchVM searchInput, int? siteSettingId)
        {
            List<object> result = new List<object>();

            var hasPagination = false;
            int take = 50;

            if (searchInput == null)
                searchInput = new Select2SearchVM();
            if (searchInput.page == null || searchInput.page <= 0)
                searchInput.page = 1;

            var qureResult = db.ProposalForms.OrderByDescending(t => t.Id).Where(t => t.SiteSettingId == null || t.SiteSettingId == siteSettingId);
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
