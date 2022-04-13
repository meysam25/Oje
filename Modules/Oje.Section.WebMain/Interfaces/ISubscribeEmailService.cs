using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Oje.Section.WebMain.Interfaces
{
    public interface ISubscribeEmailService
    {
        object Create(string email, IpSections clientIp, int? siteSettingId);
    }
}
