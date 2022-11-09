using Oje.Infrastructure.Models;
using Oje.Section.GlobalForms.Models.DB;
using Oje.Section.GlobalForms.Models.View;
using System.Collections.Generic;

namespace Oje.Section.GlobalForms.Interfaces
{
    public interface IGeneralFormStatusGridColumnService
    {
        ApiResult Create(GeneralFormStatusGridColumnCreateUpdateVM input);
        ApiResult Delete(int? id);
        object GetById(int? id);
        GridResultVM<GeneralFormStatusGridColumnMainGridResult> GetList(GeneralFormStatusGridColumnMainGrid searchInput);
        ApiResult Update(GeneralFormStatusGridColumnCreateUpdateVM input);
        List<GeneralFormStatusGridColumn> GetListBy(long formId, long statusId);

    }
}
