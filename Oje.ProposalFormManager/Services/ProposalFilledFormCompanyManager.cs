using Oje.Infrastructure.Exceptions;
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
    public class ProposalFilledFormCompanyManager: IProposalFilledFormCompanyManager
    {
        readonly ProposalFormDBContext db = null;
        public ProposalFilledFormCompanyManager(ProposalFormDBContext db)
        {
            this.db = db;
        }

        public void Create(long inquiryId, int? siteSettingId, long proposalFilledFormId, long proposalFilledFormPrice, int companyId)
        {
            if (inquiryId > 0)
            {
                db.Entry(new ProposalFilledFormCompany()
                {
                    CompanyId = companyId,
                    ProposalFilledFormId = proposalFilledFormId,
                    Price = proposalFilledFormPrice
                }).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                db.SaveChanges();

            }
            else
                throw BException.GenerateNewException(BMessages.No_Imprement_Yet);
        }
    }
}
