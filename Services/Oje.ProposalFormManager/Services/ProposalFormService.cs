using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;
using Oje.ProposalFormService.Interfaces;
using Oje.ProposalFormService.Models.DB;
using Oje.ProposalFormService.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Oje.ProposalFormService.Services
{
    public class ProposalFormService : IProposalFormService
    {
        readonly ProposalFormDBContext db = null;
        static Dictionary<string, ProposalForm> ppfCache = new();
        static DateTime? lastDate = null;
        static object lockObj = new();

        public ProposalFormService(ProposalFormDBContext db)
        {
            this.db = db;
        }

        public bool Exist(int proposalFormId, int? siteSettingId)
        {
            return db.ProposalForms.Any(t => t.Id == proposalFormId && (t.SiteSettingId == null || t.SiteSettingId == siteSettingId));
        }

        public ProposalForm GetById(int id, int? siteSettingId)
        {
            if (lockObj == null)
                lockObj = new();

            var curKey = id + "_" + siteSettingId;
            if (ppfCache == null)
                ppfCache = new Dictionary<string, ProposalForm>();

            if (lastDate != null && (DateTime.Now - lastDate.Value).TotalMinutes >= 5)
            {
                lastDate = DateTime.Now;
                ppfCache.Clear();
            }

            if (ppfCache.Keys.Any(t => t == curKey) && ppfCache[curKey] != null)
                return ppfCache[curKey];

            lock(lockObj)
            {
                if (ppfCache.Keys.Any(t => t == curKey) && ppfCache[curKey] != null)
                    return ppfCache[curKey];
                ppfCache[curKey] = db.ProposalForms.Where(t => t.Id == id && (t.SiteSettingId == null || t.SiteSettingId == siteSettingId)).AsNoTracking().FirstOrDefault();
            }

            return ppfCache[curKey];
        }

        public ProposalForm GetByType(ProposalFormType type, int? siteSettingId)
        {
            return db.ProposalForms.Where(t => t.Type == type && t.IsActive == true && (t.SiteSettingId == siteSettingId || t.SiteSettingId == null)).AsNoTracking().FirstOrDefault();
        }

        public string GetJSonConfigFile(int proposalFormId, int? siteSettingId)
        {
            return db.ProposalForms.Where(t => t.Id == proposalFormId && (t.SiteSettingId == null || t.SiteSettingId == siteSettingId)).Select(t => t.JsonConfig).FirstOrDefault();
        }

        public object GetSelect2List(Select2SearchVM searchInput, int? proposalFormCategoryId)
        {
            List<object> result = new List<object>();

            var hasPagination = false;
            int take = 50;

            if (searchInput == null)
                searchInput = new Select2SearchVM();
            if (searchInput.page == null || searchInput.page <= 0)
                searchInput.page = 1;

            var qureResult = db.ProposalForms.OrderByDescending(t => t.Id).AsQueryable();
            if (!string.IsNullOrEmpty(searchInput.search))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.search));
            if (proposalFormCategoryId != null)
                qureResult = qureResult.Where(t => t.ProposalFormCategoryId == proposalFormCategoryId);
            qureResult = qureResult.Skip((searchInput.page.Value - 1) * take).Take(take);
            if (qureResult.Count() >= 50)
                hasPagination = true;

            result.AddRange(qureResult.Select(t => new { id = t.Id, text = t.Title }).ToList());

            return new { results = result, pagination = new { more = hasPagination } };
        }
    }
}
