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

        public IQueryable<ProposalFilledForm> getProposalFilledFormBaseQuery(int? siteSettingId, long? userId, ProposalFilledFormStatus status, bool? canSeeOtherWebsites)
        {
            return getProposalFilledFormBaseQuery(siteSettingId, userId, canSeeOtherWebsites)
                .Where(t => t.Status == status);
        }

        public IQueryable<ProposalFilledForm> getProposalFilledFormBaseQuery(int? siteSettingId, long? userId, bool? canSeeOtherWebsites)
        {
            var allChildUserId = UserService.CanSeeAllItems(userId.ToLongReturnZiro());

            var quiryResult = db.ProposalFilledForms.Where(t => t.IsDelete != true);

            if (canSeeOtherWebsites != true)
                quiryResult = quiryResult.Where(t => t.SiteSettingId == siteSettingId);

            return quiryResult
                .Where(t =>
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
