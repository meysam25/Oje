namespace Oje.Section.Question.Interfaces
{
    public interface IUserRegisterFormService
    {
        bool Exist(int? siteSettingId, int? id);
        object GetLightList(int? siteSettingId);
    }
}
