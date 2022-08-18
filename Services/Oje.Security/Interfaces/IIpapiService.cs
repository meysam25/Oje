using Oje.Security.Models.View;

namespace Oje.Security.Interfaces
{
    public interface IIpapiService
    {
        Task<IpDetectionServiceVM> Validate(string ip);
    }
}
