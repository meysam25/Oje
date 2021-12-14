using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Services;
using Oje.ProposalFormManager.Interfaces;
using Oje.ProposalFormManager.Models.DB;
using Oje.ProposalFormManager.Services.EContext;
using System.Linq;

namespace Oje.ProposalFormManager.Services
{
    public class ProposalFilledFormAdminBaseQueryManager : IProposalFilledFormAdminBaseQueryManager
    {
        readonly ProposalFormDBContext db = null;
        readonly AccountManager.Interfaces.IUserManager UserManager = null;
        public ProposalFilledFormAdminBaseQueryManager(ProposalFormDBContext db, AccountManager.Interfaces.IUserManager UserManager)
        {
            this.db = db;
            this.UserManager = UserManager;
        }

        public IQueryable<ProposalFilledForm> getProposalFilledFormBaseQuery(int? siteSettingId, long? userId, ProposalFilledFormStatus status)
        {
            return getProposalFilledFormBaseQuery(siteSettingId, userId)
                .Where(t => t.Status == status);
        }

        public IQueryable<ProposalFilledForm> getProposalFilledFormBaseQuery(int? siteSettingId, long? userId)
        {
            var allChildUserId = UserManager.GetChildsUserId(userId.ToLongReturnZiro());
            return db.ProposalFilledForms
                .Where(t => t.IsDelete != true && t.SiteSettingId == siteSettingId && (allChildUserId == null || t.ProposalFilledFormUsers.Any(tt => allChildUserId.Contains(tt.UserId))));
        }
    }
}
