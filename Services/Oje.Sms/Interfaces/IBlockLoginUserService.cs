namespace Oje.Sms.Interfaces
{
    public interface IBlockLoginUserService
    {
        bool IsValidDay(DateTime now, int? siteSettingId);
    }
}
