using Oje.AccountService.Models.View;
using Oje.Infrastructure.Models;

namespace Oje.AccountService.Interfaces
{
    public interface IControllerCategoryService
    {
        ApiResult Create(ControllerCategoryCreateUpdateVM input);
        ApiResult Delete(int? id);
        object GetById(int? id);
        ApiResult Update(ControllerCategoryCreateUpdateVM input);
        GridResultVM<ControllerCategoryMainGridResultVM> GetList(ControllerCategoryMainGrid searchInput);
    }
}
