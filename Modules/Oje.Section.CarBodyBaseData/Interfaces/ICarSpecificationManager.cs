using Oje.Infrastructure.Models;

namespace Oje.Section.CarBodyBaseData.Interfaces
{
    public interface ICarSpecificationManager
    {
        object GetSelect2List(Select2SearchVM searchInput);
    }
}
