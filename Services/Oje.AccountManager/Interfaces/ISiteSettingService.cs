using Oje.AccountService.Models.DB;

namespace Oje.AccountService.Interfaces
{
    public interface ISiteSettingService
    {
        object GetightList();
        SiteSetting GetSiteSetting();
        void UpdateSiteSettings();
        object GetManifest();
        string GetMainService();
        string GetRegisterServices();
        object GetightList(bool? canSeeAllWeb, Infrastructure.Models.Select2SearchVM searchInput);
    }
}
