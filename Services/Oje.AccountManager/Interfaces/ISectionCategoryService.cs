using Oje.AccountService.Models.View;
using Oje.Infrastructure.Models;

namespace Oje.AccountService.Interfaces
{
    public interface ISectionCategoryService
    {
        ApiResult Create(SectionCategoryCreateUpdateVM input);
        ApiResult Delete(int? id);
        object GetById(int? id);
        ApiResult Update(SectionCategoryCreateUpdateVM input);
        GridResultVM<SectionCategoryMainGridResultVM> GetList(SectionCategoryMainGrid searchInput);
    }
}
