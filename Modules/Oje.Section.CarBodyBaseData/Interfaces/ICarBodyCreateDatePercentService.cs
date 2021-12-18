using Oje.Infrastructure.Models;
using Oje.Section.CarBodyBaseData.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarBodyBaseData.Interfaces
{
    public interface ICarBodyCreateDatePercentService
    {
        ApiResult Create(CreateUpdateCarBodyCreateDatePercentVM input);
        ApiResult Delete(int? id);
        CreateUpdateCarBodyCreateDatePercentVM GetById(int? id);
        ApiResult Update(CreateUpdateCarBodyCreateDatePercentVM input);
        GridResultVM<CarBodyCreateDatePercentMainGridResultVM> GetList(CarBodyCreateDatePercentMainGrid searchInput);
    }
}
