using Oje.Infrastructure.Models;
using Oje.Section.Question.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.Question.Interfaces
{
    public interface IYourQuestionService
    {
        ApiResult Create(YourQuestionCreateUpdateVM input, int? siteSettingId);
        ApiResult Delete(int? id, int? siteSettingId);
        object GetById(int? id, int? siteSettingId);
        ApiResult Update(YourQuestionCreateUpdateVM input, int? siteSettingId);
        GridResultVM<YourQuestionMainGridResultVM> GetList(YourQuestionMainGrid searchInput, int? siteSettingId);
        object GetListForWeb(int? siteSettingId);
    }
}
