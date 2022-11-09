using Oje.Infrastructure.Models;
using Oje.Section.GlobalForms.Models.View;

namespace Oje.Section.GlobalForms.Interfaces
{
    public interface IGeneralFilledFormStatusService
    {
        void Create(long filledFormId, long statusId, string desc, long loginUserId);
        object GetList(GeneralFilledFormStatusMainGrid searchInput, int? siteSettingId, LoginUserVM loginUserVM);
    }
}
