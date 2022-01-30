using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Security.Interfaces
{
    public interface IBlockAutoIpService
    {
        void CheckIfRequestIsValid(BlockClientConfigType type, BlockAutoIpAction exeType, IpSections ipSections, int? siteSettingId);
    }
}
