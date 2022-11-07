using Oje.Infrastructure.Models;
using Oje.Section.GlobalForms.Models.View;

namespace Oje.Section.GlobalForms.Interfaces
{
    public interface IGeneralFormStatusService
    {
        ApiResult Create(GeneralFormStatusCreateUpdateVM input);
        ApiResult Delete(long? id);
        object GetById(long? id);
        GridResultVM<GeneralFormStatusMainGridResultVM> GetList(GeneralFormStatusMainGrid searchInput);
        ApiResult Update(GeneralFormStatusCreateUpdateVM input);

        long GetFirstId(long id);
    }
}
