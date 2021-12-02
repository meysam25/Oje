using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Enums;
using Oje.ProposalFormManager.Interfaces;
using Oje.ProposalFormManager.Models.DB;
using Oje.ProposalFormManager.Services.EContext;
using System.Linq;

namespace Oje.ProposalFormManager.Services
{
    public class ProposalFormManager: IProposalFormManager
    {
        readonly ProposalFormDBContext db = null;
        public ProposalFormManager(ProposalFormDBContext db)
        {
            this.db = db;
        }

        public bool Exist(int proposalFormId, int? siteSettingId)
        {
            return db.ProposalForms.Any(t => t.Id == proposalFormId && (t.SiteSettingId == null || t.SiteSettingId == siteSettingId));
        }

        public ProposalForm GetById(int id, int? siteSettingId)
        {
            return db.ProposalForms.Where(t => t.Id == id && (t.SiteSettingId == null || t.SiteSettingId == siteSettingId)).AsNoTracking().FirstOrDefault();
        }

        public ProposalForm GetByType(ProposalFormType type, int? siteSettingId)
        {
            return db.ProposalForms.Where(t => t.Type == type && t.IsActive == true && (t.SiteSettingId == siteSettingId || t.SiteSettingId == null)).AsNoTracking().FirstOrDefault();
        }

        public string GetJSonConfigFile(int proposalFormId, int? siteSettingId)
        {
            return db.ProposalForms.Where(t => t.Id == proposalFormId && (t.SiteSettingId == null || t.SiteSettingId == siteSettingId)).Select(t => t.JsonConfig).FirstOrDefault();
        }
    }
}
