using Microsoft.EntityFrameworkCore;
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
    public class ProposalFilledFormJsonService: IProposalFilledFormJsonService
    {
        readonly ProposalFormDBContext db = null;
        public ProposalFilledFormJsonService(
                ProposalFormDBContext db
            )
        {
            this.db = db;
        }

        public void Create(long proposalFilledFormId, string jsonConfig)
        {
            db.Entry(new ProposalFilledFormJson()
            {
                ProposalFilledFormId = proposalFilledFormId,
                JsonConfig = jsonConfig
            }).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            db.SaveChanges();
        }

        public ProposalFilledFormJson GetBy(long proposalFilledFormId)
        {
            return db.ProposalFilledFormJsons.Where(t => t.ProposalFilledFormId == proposalFilledFormId).AsNoTracking().FirstOrDefault();
        }
    }
}
