
namespace Oje.Sanab.Interfaces
{
    public interface ISanabLoginService
    {
        Task<string> LoginAsync(int? siteSettingId);
        Task<string> GenerateAccessToken(int? siteSettingId);
    }
}
