using Microsoft.EntityFrameworkCore;
using Oje.Section.InsuranceContractBaseData.Interfaces;
using Oje.Section.InsuranceContractBaseData.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InsuranceContractBaseData.Services
{
    public class InsuranceContractProposalFilledFormKeyService : IInsuranceContractProposalFilledFormKeyService
    {
        readonly InsuranceContractBaseDataDBContext db = null;
        public InsuranceContractProposalFilledFormKeyService(InsuranceContractBaseDataDBContext db)
        {
            this.db = db;
        }

        public int CreateIfNeeded(string name)
        {
            var foundItem = db.InsuranceContractProposalFilledFormKeys.Where(t => t.Key == name).FirstOrDefault();
            if (foundItem != null)
                return foundItem.Id;

            foundItem = new Models.DB.InsuranceContractProposalFilledFormKey() { Key = name };
            db.Entry(foundItem).State = EntityState.Added;
            db.SaveChanges();

            return foundItem.Id;
        }
    }
}
