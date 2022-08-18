using Oje.Infrastructure.Models;
using Oje.Security.Models.View;

namespace Oje.Security.Interfaces
{
    public interface IInValidRangeIpService
    {
        bool Create(IpSections ip);
        Task ValidateIps();
        GridResultVM<InValidRangeIpMainGridResult> GetList(InValidRangeIpMainGrid searchInput);
    }
}
