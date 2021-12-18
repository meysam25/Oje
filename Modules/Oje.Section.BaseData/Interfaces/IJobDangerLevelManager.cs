using Oje.Infrastructure.Models;
using Oje.Section.BaseData.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.BaseData.Interfaces
{
    public interface IJobDangerLevelService
    {
        ApiResult Create(CreateUpdateJobDangerLevelVM input);
        ApiResult Delete(int? id);
        CreateUpdateJobDangerLevelVM GetById(int? id);
        ApiResult Update(CreateUpdateJobDangerLevelVM input);
        GridResultVM<JobDangerLevelMainGridResultVM> GetList(JobDangerLevelMainGrid searchInput);
        object GetLightList();
    }
}
