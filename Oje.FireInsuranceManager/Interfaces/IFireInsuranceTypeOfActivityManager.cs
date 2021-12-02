using Oje.FireInsuranceManager.Models.DB;
using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.FireInsuranceManager.Interfaces
{
    public interface IFireInsuranceTypeOfActivityManager
    {
        object GetList(Select2SearchVM searchInput);
        List<FireInsuranceTypeOfActivity> GetBy(List<int> foundAllActivityIds);
    }
}
