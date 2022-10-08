using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.ProposalFormService.Interfaces;
using Oje.ProposalFormService.Models.DB;
using Oje.ProposalFormService.Services.EContext;
using System.Collections.Generic;
using System.Linq;

namespace Oje.ProposalFormService.Services
{
    public class ProposalFilledFormUseService : IProposalFilledFormUseService
    {
        readonly ProposalFormDBContext db = null;
        readonly IProposalFilledFormSiteSettingService ProposalFilledFormSiteSettingService = null;

        public ProposalFilledFormUseService
            (
                ProposalFormDBContext db,
                IProposalFilledFormSiteSettingService ProposalFilledFormSiteSettingService
            )
        {
            this.db = db;
            this.ProposalFilledFormSiteSettingService = ProposalFilledFormSiteSettingService;
        }

        public void Create(long? userId, ProposalFilledFormUserType type, long? fromUserId, long proposalFilledFormId, int? siteSettingId)
        {
            //if (userId.ToLongReturnZiro() > 0 && proposalFilledFormId > 0 &&
            //    !db.ProposalFilledFormUsers.Any(t => t.UserId == userId && t.ProposalFilledFormId == proposalFilledFormId && t.Type == type))
            //{
            db.Entry(new ProposalFilledFormUser()
            {
                FromUserId = fromUserId,
                ProposalFilledFormId = proposalFilledFormId,
                Type = type,
                UserId = userId.ToLongReturnZiro()
            }).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            db.SaveChanges();

            ProposalFilledFormSiteSettingService.Create(proposalFilledFormId, siteSettingId);
            //}
        }

        public List<PPFUserTypes> GetProposalFilledFormUserIds(long proposalFilledFormId)
        {
            return db.ProposalFilledFormUsers
                .Where(t => t.ProposalFilledFormId == proposalFilledFormId)
                .Select(t => new
                {
                    userId = t.UserId,
                    ProposalFilledFormUserType = t.Type,
                    fullUserName = t.User.Firstname + " " + t.User.Lastname,
                    mobile = t.User.Mobile,
                    emaile = t.User.Email
                })
                .ToList()
                .Select(t => new PPFUserTypes
                {
                    userId = t.userId,
                    emaile = t.emaile,
                    mobile = t.mobile,
                    fullUserName = t.fullUserName,
                    ProposalFilledFormUserType = t.ProposalFilledFormUserType
                })
                .ToList();
        }

        public bool HasAny(long proposalFilledFormId, ProposalFilledFormUserType type)
        {
            return db.ProposalFilledFormUsers.Any(t => t.ProposalFilledFormId == proposalFilledFormId && t.Type == type);
        }

        public void Update(long? userId, ProposalFilledFormUserType type, long? fromUserId, long proposalFilledFormId, int? siteSettingId)
        {
            if (userId.ToLongReturnZiro() > 0 && fromUserId.ToLongReturnZiro() > 0 && proposalFilledFormId > 0)
            {
                var foundItem = db.ProposalFilledFormUsers.Where(t => t.ProposalFilledFormId == proposalFilledFormId && t.Type == type).FirstOrDefault();
                if (foundItem != null)
                {
                    db.Entry(foundItem).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                    db.SaveChanges();
                    Create(userId, type, fromUserId, proposalFilledFormId, siteSettingId);
                }
            }
        }
    }
}
