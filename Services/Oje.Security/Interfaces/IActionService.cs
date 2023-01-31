using Oje.Infrastructure.Models;

namespace Oje.Security.Interfaces
{
    public interface IActionService
    {
        List<long> GetList();
        object GetSelect2List(Select2SearchVM searchInput);
    }
}
