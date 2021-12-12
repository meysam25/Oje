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
    public class ProposalFilledFormUseManager: IProposalFilledFormUseManager
    {
        readonly ProposalFormDBContext db = null;
        public ProposalFilledFormUseManager(ProposalFormDBContext db)
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

        public void Update(long? userId, ProposalFilledFormUserType type, long? fromUserId, long proposalFilledFormId)
        {
            var foundItem = db.ProposalFilledFormUsers.Where(t => t.ProposalFilledFormId == proposalFilledFormId && t.Type == type).FirstOrDefault();
            if(foundItem != null)
            {
                foundItem.FromUserId = fromUserId;
                foundItem.UserId = userId.ToLongReturnZiro();
                db.SaveChanges();
            }
        }
    }
}
