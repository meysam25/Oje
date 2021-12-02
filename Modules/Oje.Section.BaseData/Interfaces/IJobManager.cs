using Oje.Infrastructure.Models;
using Oje.Section.BaseData.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.BaseData.Interfaces
{
    public interface IJobManager
    {
        ApiResult Create(CreateUpdateJobVM input);
        ApiResult Delete(int? id);
        CreateUpdateJobVM GetById(int? id);
        ApiResult Update(CreateUpdateJobVM input);
        GridResultVM<JobMainGridResultVM> GetList(JobMainGrid searchInput);
    }
}
