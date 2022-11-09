using Oje.Infrastructure.Models;
using Oje.Section.GlobalForms.Models.View;
using System.Collections.Generic;

namespace Oje.Section.GlobalForms.Interfaces
{
    public interface IGeneralFormStatusService
    {
        ApiResult Create(GeneralFormStatusCreateUpdateVM input);
        ApiResult Delete(long? id);
        object GetById(long? id);
        GridResultVM<GeneralFormStatusMainGridResultVM> GetList(GeneralFormStatusMainGrid searchInput);
        ApiResult Update(GeneralFormStatusCreateUpdateVM input);
        object GetSelect2List(Select2SearchVM searchInput, long? generalFormId);
        List<IdTitle> GetLightList(List<string> roleNames);

        long GetFirstId(long id);
        List<IdTitle> GetNextStatuses(long id);
    }
}
