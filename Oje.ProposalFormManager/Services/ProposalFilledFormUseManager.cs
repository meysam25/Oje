using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Services;
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
    public class ProposalFilledFormUseService: IProposalFilledFormUseService
    {
        readonly ProposalFormDBContext db = null;
        public ProposalFilledFormUseService(ProposalFormDBContext db)
        {
            this.db = db;
        }

        public void Create(long? userId, ProposalFilledFormUserType type, long? fromUserId, long proposalFilledFormId)
        {
            if(userId.ToLongReturnZiro() > 0 && proposalFilledFormId > 0 && 
                !db.ProposalFilledFormUsers.Any(t => t.UserId == userId && t.ProposalFilledFormId == proposalFilledFormId && t.Type == type))
            {
                db.Entry(new ProposalFilledFormUser() 
                {
                    FromUserId = fromUserId,
                    ProposalFilledFormId = proposalFilledFormId,
                    Type = type,
                    UserId = userId.ToLongReturnZiro()
                }).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                db.SaveChanges();
            }
        }

        public List<long> GetProposalFilledFormUserIds(long proposalFilledFormId)
        {
            return db.ProposalFilledFormUsers.Where(t => t.ProposalFilledFormId == proposalFilledFormId).Select(t => t.UserId).ToList();
        }

        public bool HasAny(long proposalFilledFormId, ProposalFilledFormUserType type)
        {
            return db.ProposalFilledFormUsers.Any(t => t.ProposalFilledFormId == proposalFilledFormId && t.Type == type);
        }

        public void Update(long? userId, ProposalFilledFormUserType type, long? fromUserId, long proposalFilledFormId)
        {
            if(userId.ToLongReturnZiro() > 0 && fromUserId.ToLongReturnZiro() > 0 && proposalFilledFormId > 0)
            {
                var foundItem = db.ProposalFilledFormUsers.Where(t => t.ProposalFilledFormId == proposalFilledFormId && t.Type == type).FirstOrDefault();
                if (foundItem != null)
                {
                    db.Entry(foundItem).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                    db.SaveChanges();
                    Create(userId, type, fromUserId, proposalFilledFormId);
                }
            }
        }
    }
}
