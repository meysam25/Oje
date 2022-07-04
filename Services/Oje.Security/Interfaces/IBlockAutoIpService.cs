using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;
using Oje.Security.Models.View;

namespace Oje.Security.Interfaces
{
    public interface IBlockAutoIpService
    {
        void CheckIfRequestIsValid(BlockClientConfigType type, BlockAutoIpAction exeType, IpSections ipSections, int? siteSettingId);
        GridResultVM<BlockAutoIpMainGridResultVM> GetList(BlockAutoIpMainGrid searchInput, int? siteSettingId);
    }
}
