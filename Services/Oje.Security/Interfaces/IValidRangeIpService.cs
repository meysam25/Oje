using Oje.Infrastructure.Models;
using Oje.Security.Models.DB;
using Oje.Security.Models.View;

namespace Oje.Security.Interfaces
{
    public interface IValidRangeIpService
    {
        ApiResult Create(ValidRangeIpCreateUpdateVM input);
        ApiResult Delete(int? id);
        object GetById(int? id);
        ApiResult Update(ValidRangeIpCreateUpdateVM input);
        GridResultVM<ValidRangeIpMainGridResultVM> GetList(ValidRangeIpMainGrid searchInput);
        object CreateFromExcel(GlobalExcelFile input);
        List<ValidRangeIp> GetCacheIpRangeList();
    }
}
