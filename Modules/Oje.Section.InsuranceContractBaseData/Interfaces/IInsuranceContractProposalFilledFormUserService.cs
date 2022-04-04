using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InsuranceContractBaseData.Interfaces
{
    public interface IInsuranceContractProposalFilledFormUserService
    {
        void Create(List<IdTitle> familyRelations, List<IdTitle> familyCTypes, long insuranceContractProposalFilledFormId);
    }
}
