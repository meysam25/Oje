using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Services;
using Oje.ProposalFormManager.Interfaces;
using Oje.ProposalFormManager.Models.DB;
using Oje.ProposalFormManager.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var allChildUserId = UserManager.GetChildsUserId(userId.ToLongReturnZiro());
            return db.ProposalFilledForms
                .Where(t => t.IsDelete != true && t.Status == status && t.SiteSettingId == siteSettingId && (allChildUserId == null || t.ProposalFilledFormUsers.Any(tt => allChildUserId.Contains(tt.UserId))));
        }
    }
}
