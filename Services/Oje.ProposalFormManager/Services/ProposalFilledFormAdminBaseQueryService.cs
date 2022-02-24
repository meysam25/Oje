using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Services;
using Oje.ProposalFormService.Interfaces;
using Oje.ProposalFormService.Models.DB;
using Oje.ProposalFormService.Services.EContext;
using System.Linq;

namespace Oje.ProposalFormService.Services
{
    public class ProposalFilledFormAdminBaseQueryService : IProposalFilledFormAdminBaseQueryService
    {
        readonly ProposalFormDBContext db = null;
        readonly AccountService.Interfaces.IUserService UserService = null;
        public ProposalFilledFormAdminBaseQueryService(ProposalFormDBContext db, AccountService.Interfaces.IUserService UserService)
        {
            this.db = db;
            this.UserService = UserService;
        }

        public IQueryable<ProposalFilledForm> getProposalFilledFormBaseQuery(int? siteSettingId, long? userId, ProposalFilledFormStatus status)
        {
            return getProposalFilledFormBaseQuery(siteSettingId, userId)
                .Where(t => t.Status == status);
        }

        public IQueryable<ProposalFilledForm> getProposalFilledFormBaseQuery(int? siteSettingId, long? userId)
        {
            var allChildUserId = UserService.CanSeeAllItems(userId.ToLongReturnZiro());
            return db.ProposalFilledForms
                .Where(t => t.IsDelete != true && t.SiteSettingId == siteSettingId &&
                (
                    allChildUserId == true || 
                    t.ProposalFilledFormUsers.Any
                     (tt => tt.UserId == userId || 
                        tt.User.Parent.Id == userId ||
                        tt.User.Parent.Parent.Id == userId ||
                        tt.User.Parent.Parent.Parent.Id == userId ||
                        tt.User.Parent.Parent.Parent.Parent.Id == userId ||
                        tt.User.Parent.Parent.Parent.Parent.Parent.Id == userId ||
                        tt.User.Parent.Parent.Parent.Parent.Parent.Parent.Id == userId ||
                        tt.User.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Id == userId ||
                        tt.User.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Id == userId ||
                        tt.User.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Id == userId
                     )
                )
                );
        }

        public string getControllerNameByStatus(ProposalFilledFormStatus status)
        {
            switch (status)
            {
                case ProposalFilledFormStatus.New:
                    return "/ProposalFilledFormNew";
                case ProposalFilledFormStatus.W8ForConfirm:
                    return "/ProposalFilledFormW8ForConfirm";
                case ProposalFilledFormStatus.NeedSpecialist:
                    return "/ProposalFilledFormNeedSpecialist";
                case ProposalFilledFormStatus.Confirm:
                    return "/ProposalFilledFormConfirm";
                case ProposalFilledFormStatus.Issuing:
                    return "/ProposalFilledFormIssue";
                case ProposalFilledFormStatus.NotIssue:
                    return "/ProposalFilledFormNotIssue";
                default:
                    return "";
            }
        }
    }
}
