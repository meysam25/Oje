using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.WebMain.Interfaces
{
    public interface IAutoAnswerOnlineChatMessageLikeService
    {
        Task Create(int autoAnswerOnlineChatMessageId, bool isLike, int? siteSettingId, IpSections clientIp);
    }
}
