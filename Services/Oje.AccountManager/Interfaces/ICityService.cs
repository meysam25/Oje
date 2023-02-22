using Oje.Infrastructure.Models;

namespace Oje.AccountService.Interfaces
{
    public interface ICityService
    {
        object GetLightList(int? provinceId);
        int? GetIdBy(string title);
        int? Create(int? provinceId, string title, bool isActive);
        object GetSelect2List(int? provinceId, Select2SearchVM searchInput);
        bool Exist(int provinceId, int id);
    }
}
