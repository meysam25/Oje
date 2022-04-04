using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.InsuranceContractBaseData.Interfaces;
using Oje.Section.InsuranceContractBaseData.Models.DB;
using Oje.Section.InsuranceContractBaseData.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InsuranceContractBaseData.Services
{
    public class InsuranceContractProposalFilledFormUserService : IInsuranceContractProposalFilledFormUserService
    {
        readonly InsuranceContractBaseDataDBContext db = null;
        public InsuranceContractProposalFilledFormUserService(InsuranceContractBaseDataDBContext db)
        {
            this.db = db;
        }

        public void Create(List<IdTitle> familyRelations, List<IdTitle> familyCTypes, long insuranceContractProposalFilledFormId)
        {
            for (
                var i = 0;
                familyRelations != null && i < familyRelations.Count && familyCTypes != null && i < familyCTypes.Count;
                i++
                )
            {
                var newItem = new InsuranceContractProposalFilledFormUser()
                {
                    InsuranceContractProposalFilledFormId = insuranceContractProposalFilledFormId,
                    InsuranceContractTypeId = familyCTypes[i].id.ToIntReturnZiro(),
                    InsuranceContractUserId = familyRelations[i].id.ToLongReturnZiro()
                };
                if (db.InsuranceContractProposalFilledFormUsers.Any(t => t.InsuranceContractTypeId == newItem.InsuranceContractTypeId && t.InsuranceContractUserId == newItem.InsuranceContractUserId && t.InsuranceContractProposalFilledFormId == newItem.InsuranceContractProposalFilledFormId))
                    throw BException.GenerateNewException(BMessages.Dublicate_Item);
                db.Entry(newItem).State = EntityState.Added;
            }
            db.SaveChanges();
        }
    }
}
