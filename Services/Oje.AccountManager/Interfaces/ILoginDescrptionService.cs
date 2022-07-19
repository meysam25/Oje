namespace Oje.AccountService.Interfaces
{
    public interface ILoginDescrptionService
    {
        object GetBy(int? siteSettingId, string returnUrl);
    }
}
