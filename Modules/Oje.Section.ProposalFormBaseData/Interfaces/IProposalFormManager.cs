using Oje.Infrastructure.Models;
using Oje.Section.ProposalFormBaseData.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.ProposalFormBaseData.Interfaces
{
    public interface IProposalFormManager
    {
        ApiResult Create(CreateUpdateProposalFormVM input, long? userId);
        ApiResult Delete(int? id);
        GetByIdProposalFormVM GetById(int? id);
        ApiResult Update(CreateUpdateProposalFormVM input, long? userId);
        GridResultVM<ProposalFormMainGridResultVM> GetList(ProposalFormMainGrid searchInput);
        object GetSelect2List(Select2SearchVM searchInput);
        bool Exist(int id, int? siteSettingId);
    }
}
