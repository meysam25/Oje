using Oje.Infrastructure.Models;
using Oje.Section.ProposalFormBaseData.Models;
using Oje.Section.ProposalFormBaseData.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.ProposalFormBaseData.Interfaces
{
    public interface IProposalFormRequiredDocumentTypeService
    {
        ApiResult Create(CreateUpdateProposalFormRequiredDocumentTypeVM input);
        ApiResult Delete(int? id);
        CreateUpdateProposalFormRequiredDocumentTypeVM GetById(int? id);
        ApiResult Update(CreateUpdateProposalFormRequiredDocumentTypeVM input);
        GridResultVM<ProposalFormRequiredDocumentTypeMainGridResultVM> GetList(ProposalFormRequiredDocumentTypeMainGrid searchInput);
        object GetSellect2List(Select2SearchVM searchInput);
    }
}
