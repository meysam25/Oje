using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.WebMain.Interfaces;
using Oje.Section.WebMain.Models.DB;
using Oje.Section.WebMain.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.WebMain.Services
{
    public class AutoAnswerOnlineChatMessageLikeService : IAutoAnswerOnlineChatMessageLikeService
    {
        readonly WebMainDBContext db = null;
        public AutoAnswerOnlineChatMessageLikeService
            (
                WebMainDBContext db
            )
        {
            this.db = db;
        }

        public async Task Create(int autoAnswerOnlineChatMessageId, bool isLike, int? siteSettingId, IpSections clientIp)
        {
            if (autoAnswerOnlineChatMessageId > 0 && siteSettingId.ToIntReturnZiro() > 0 && clientIp != null)
            {
                if (!db.AutoAnswerOnlineChatMessageLikes
                    .Any(t => t.Ip1 == clientIp.Ip1 && t.Ip2 == clientIp.Ip2 && t.Ip3 == clientIp.Ip3 && t.Ip4 == clientIp.Ip4 && t.AutoAnswerOnlineChatMessageId == autoAnswerOnlineChatMessageId && t.SiteSettingId == siteSettingId))
                {
                    db.Entry(new AutoAnswerOnlineChatMessageLike()
                    {
                        AutoAnswerOnlineChatMessageId = autoAnswerOnlineChatMessageId,
                        IsLike = isLike,
                        SiteSettingId = siteSettingId.Value,
                        Ip1 = clientIp.Ip1,
                        Ip2 = clientIp.Ip2,
                        Ip3 = clientIp.Ip3,
                        Ip4 = clientIp.Ip4
                    }).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                    await db.SaveChangesAsync();
                }
            }
        }
    }
}
