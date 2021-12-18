using Oje.Infrastructure.Models;
using Oje.Section.CarThirdBaseData.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarThirdBaseData.Interfaces
{
    public interface IThirdPartyLifeCommitmentService
    {
        ApiResult Create(CreateUpdateThirdPartyLifeCommitmentVM input);
        ApiResult Delete(int? id);
        CreateUpdateThirdPartyLifeCommitmentVM GetById(int? id);
        ApiResult Update(CreateUpdateThirdPartyLifeCommitmentVM input);
        GridResultVM<ThirdPartyLifeCommitmentMainGridResultVM> GetList(ThirdPartyLifeCommitmentMainGrid searchInput);
    }
}
