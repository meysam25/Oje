using Microsoft.EntityFrameworkCore;
using Oje.Section.InsuranceContractBaseData.Interfaces;
using Oje.Section.InsuranceContractBaseData.Models.DB;
using Oje.Section.InsuranceContractBaseData.Services.EContext;
using System.Linq;

namespace Oje.Section.InsuranceContractBaseData.Services
{
    public class InsuranceContractProposalFilledFormJsonService: IInsuranceContractProposalFilledFormJsonService
    {
        readonly InsuranceContractBaseDataDBContext db = null;
        public InsuranceContractProposalFilledFormJsonService(InsuranceContractBaseDataDBContext db)
        {
            this.db = db;
        }

        public void Create(long insuranceContractProposalFilledFormId, string jsonConfigStr)
        {
            db.Entry(new InsuranceContractProposalFilledFormJson() 
            {
                InsuranceContractProposalFilledFormId = insuranceContractProposalFilledFormId,
                JsonConfig = jsonConfigStr
            }).State = EntityState.Added;
            db.SaveChanges();
        }

        public string GetBy(long insuranceContractProposalFilledFormId)
        {
            return db.InsuranceContractProposalFilledFormJsons
                .Where(t => t.InsuranceContractProposalFilledFormId == insuranceContractProposalFilledFormId)
                .Select(t => t.JsonConfig)
                .FirstOrDefault();
        }
    }
}
