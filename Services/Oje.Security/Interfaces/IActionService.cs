using Oje.Infrastructure.Models;

namespace Oje.Security.Interfaces
{
    public interface IActionService
    {
        object GetSelect2List(Select2SearchVM searchInput);
    }
}
