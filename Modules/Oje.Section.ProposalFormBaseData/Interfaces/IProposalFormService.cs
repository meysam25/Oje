using Oje.Infrastructure.Models;
using Oje.Section.ProposalFormBaseData.Models.View;

namespace Oje.Section.ProposalFormBaseData.Interfaces
{
    public interface IProposalFormService
    {
        ApiResult Create(CreateUpdateProposalFormVM input, long? userId);
        ApiResult Delete(int? id);
        GetByIdProposalFormVM GetById(int? id);
        ApiResult Update(CreateUpdateProposalFormVM input, long? userId);
        GridResultVM<ProposalFormMainGridResultVM> GetList(ProposalFormMainGrid searchInput);
        object GetSelect2List(Select2SearchVM searchInput, int? siteSettingId);
        bool Exist(int id, int? siteSettingId);
        string GetJSonConfigFile(int id, int? siteSettingId);
    }
}
