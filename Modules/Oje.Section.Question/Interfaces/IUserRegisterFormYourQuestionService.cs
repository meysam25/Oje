using Oje.Infrastructure.Models;
using Oje.Section.Question.Models.View;

namespace Oje.Section.Question.Interfaces
{
    public interface IUserRegisterFormYourQuestionService
    {
        ApiResult Create(UserRegisterFormYourQuestionCreateUpdateVM input, int? siteSettingId);
        ApiResult Delete(int? id, int? siteSettingId);
        object GetById(int? id, int? siteSettingId);
        ApiResult Update(UserRegisterFormYourQuestionCreateUpdateVM input, int? siteSettingId);
        GridResultVM<UserRegisterFormYourQuestionMainGridResultVM> GetList(UserRegisterFormYourQuestionMainGrid searchInput, int? siteSettingId);
        object GetListForWeb(int? siteSettingId, int? formid);
    }
}
