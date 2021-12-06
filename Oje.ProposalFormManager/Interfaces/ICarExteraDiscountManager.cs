using Oje.ProposalFormManager.Models;
using Oje.ProposalFormManager.Models.DB;
using Oje.ProposalFormManager.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormManager.Interfaces
{
    public interface ICarExteraDiscountManager
    {
        List<CarExteraDiscount> GetRequredValidCTRLs(int? proposalFormId, int vehicleSystemId, bool hasPrevInsurance);
        List<CarExteraDiscount> GetOptionSelectedCtrls(List<int> Ids , int? proposalFormId);
        object GetRequiredQuestions(RequiredQuestionVM input, int? proposalFormId);
        object GetRequiredQuestionsJsonCtrls(RequiredQuestionVM input, int? proposalFormId);
        object GetValuesForDD(int id);
    }
}
