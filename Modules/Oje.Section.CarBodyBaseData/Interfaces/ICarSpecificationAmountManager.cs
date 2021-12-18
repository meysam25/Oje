using Oje.Infrastructure.Models;
using Oje.Section.CarBodyBaseData.Models.View;

namespace Oje.Section.CarBodyBaseData.Interfaces
{
    public interface ICarSpecificationAmountService
    {
        ApiResult Create(CreateUpdateCarSpecificationAmountVM input);
        ApiResult Delete(int? id);
        CreateUpdateCarSpecificationAmountVM GetById(int? id);
        ApiResult Update(CreateUpdateCarSpecificationAmountVM input);
        GridResultVM<CarSpecificationAmountMainGridResultVM> GetList(CarSpecificationAmountMainGrid searchInput);
    }
}
