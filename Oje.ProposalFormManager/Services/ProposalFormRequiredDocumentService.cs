using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.ProposalFormService.Interfaces;
using Oje.ProposalFormService.Models.DB;
using Oje.ProposalFormService.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormService.Services
{
    public class ProposalFormRequiredDocumentService : IProposalFormRequiredDocumentService
    {
        readonly ProposalFormDBContext db = null;
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
                    sample = GlobalConfig.FileAccessHandlerUrl + t.sample
                })
                .ToList();
        }

        public List<ProposalFormRequiredDocument> GetProposalFormRequiredDocuments(int? proposalFormId, int? siteSettingId)
        {
            return db.ProposalFormRequiredDocuments
                .Where(t => t.ProposalFormRequiredDocumentType.ProposalFormId == proposalFormId && t.IsActive == true)
                .AsNoTracking()
                .ToList();
        }
    }
}
