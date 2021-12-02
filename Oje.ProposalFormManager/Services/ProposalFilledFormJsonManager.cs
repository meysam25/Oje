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
    public class ProposalFilledFormJsonManager: IProposalFilledFormJsonManager
    {
        readonly ProposalFormDBContext db = null;
        public ProposalFilledFormJsonManager(
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
    }
}
