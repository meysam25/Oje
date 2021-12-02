using Oje.AccountManager.Models.DB;

namespace Oje.AccountManager.Interfaces
{
    public interface ISiteSettingManager
    {
        object GetightList();
        SiteSetting GetSiteSetting();
        void UpdateSiteSettings();
    }
}
