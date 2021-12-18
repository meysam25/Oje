using Oje.Infrastructure.Models;

namespace Oje.Section.CarBodyBaseData.Interfaces
{
    public interface ICarSpecificationService
    {
        object GetSelect2List(Select2SearchVM searchInput);
    }
}
