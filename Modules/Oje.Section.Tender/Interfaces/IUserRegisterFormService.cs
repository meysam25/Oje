namespace Oje.Section.Tender.Interfaces
{
    public interface IUserRegisterFormService
    {
        bool Exist(int id, int? siteSettingId);
        object GetLightList(int? siteSettingId);
    }
}
