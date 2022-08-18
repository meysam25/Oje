using Oje.Infrastructure.Models;
using Oje.Sanab.Models.View;

namespace Oje.Sanab.Interfaces
{
    public interface ISanabCompanyService
    {
        ApiResult Create(SanabCompanyCreateUpdateVM input);
        ApiResult Delete(int? id);
        object GetById(int? id);
        ApiResult Update(SanabCompanyCreateUpdateVM input);
        GridResultVM<SanabCompanyMainGridResultVM> GetList(SanabCompanyMainGrid searchInput);
        int? GetCompanyId(int? code);
    }
}
