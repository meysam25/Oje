using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure;
using Oje.ProposalFormService.Interfaces;
using Oje.ProposalFormService.Models.DB;
using Oje.ProposalFormService.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Oje.ProposalFormService.Services
{
    public class ProposalFormRequiredDocumentService : IProposalFormRequiredDocumentService
    {
        readonly ProposalFormDBContext db = null;
        static Dictionary<string, List<ProposalFormRequiredDocument>> rdCache = new ();
        static DateTime? cacheDate = null;
        static object lockObj = new object();

        public ProposalFormRequiredDocumentService(ProposalFormDBContext db)
        {
            this.db = db;
        }

        public object GetLightList(int? siteSettingID, int? proposalFormId)
        {
            return db.ProposalFormRequiredDocuments
                .Where(t =>
                        t.ProposalFormRequiredDocumentType.ProposalForm.Id == proposalFormId && t.IsActive == true && t.ProposalFormRequiredDocumentType.IsActive == true &&
                        (t.ProposalFormRequiredDocumentType.ProposalForm.SiteSettingId == null || t.ProposalFormRequiredDocumentType.ProposalForm.SiteSettingId == siteSettingID) &&
                        (t.ProposalFormRequiredDocumentType.SiteSettingId == null || t.ProposalFormRequiredDocumentType.SiteSettingId == siteSettingID)
                      )
                .Select(t => new
                {
                    title = t.Title,
                    isRequired = t.IsRequired,
                    sample = t.DownloadFile
                })
                .ToList()
                .Select(t => new
                {
                    t.title,
                    t.isRequired,
                    sample = !string.IsNullOrEmpty(t.sample) ? (GlobalConfig.FileAccessHandlerUrl + t.sample) : ""
                })
                .ToList();
        }

        public List<ProposalFormRequiredDocument> GetProposalFormRequiredDocuments(int? proposalFormId, int? siteSettingId)
        {
            if (lockObj == null)
                lockObj = new();
            string curKey = proposalFormId + "_" + siteSettingId;

            if (rdCache == null)
                rdCache = new();

            if (cacheDate != null && (DateTime.Now - cacheDate.Value).TotalMinutes >= 5)
                rdCache.Clear();

            if (rdCache.Keys.Any(t => t == curKey) && rdCache[curKey] != null)
                return rdCache[curKey];

            lock(lockObj)
            {
                if (rdCache.Keys.Any(t => t == curKey) && rdCache[curKey] != null)
                    return rdCache[curKey];
                rdCache[curKey] = db.ProposalFormRequiredDocuments
                .Where(t => t.ProposalFormRequiredDocumentType.ProposalFormId == proposalFormId && t.IsActive == true)
                .AsNoTracking()
                .ToList();
                cacheDate = DateTime.Now;
            }
            

            return rdCache[curKey];
        }
    }
}
