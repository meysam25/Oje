using Oje.Infrastructure.Models;

namespace Oje.Security.Interfaces
{
    public interface IErrorFirewallManualAddService
    {
        ApiResult Block(string ip, int? siteSettingId);
    }
}
