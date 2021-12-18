using Oje.Infrastructure.Models;
using Oje.Section.CarBaseData.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarBaseData.Interfaces
{
    public interface ICarSpecificationService
    {
        ApiResult Create(CreateUpdateCarSpecificationVM input);
        ApiResult Delete(int? id);
        CreateUpdateCarSpecificationVM GetById(int? id);
        ApiResult Update(CreateUpdateCarSpecificationVM input);
        GridResultVM<CarSpecificationMainGridResultVM> GetList(CarSpecificationMainGrid searchInput);
        object GetSelect2List(Select2SearchVM searchInput);
    }
}
