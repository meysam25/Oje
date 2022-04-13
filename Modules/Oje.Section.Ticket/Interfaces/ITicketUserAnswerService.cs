using Oje.Infrastructure.Models;
using Oje.Section.Ticket.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.Ticket.Interfaces
{
    public interface ITicketUserAnswerService
    {
        object GetList(TicketUserAnswerMainGrid searchInput, int? siteSettingId, long? loginUserId);
        ApiResult Create(TicketUserAnswerCreateUpdateVM input, int? siteSettingId, long? loginUserId);
        ApiResult CreateForAdmin(TicketUserAnswerCreateUpdateVM input, int? siteSettingId, long? loginUserId);
        object GetListForAdmin(TicketUserAnswerMainGrid searchInput, int? siteSettingId);
    }
}
