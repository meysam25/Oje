using Oje.Infrastructure.Models;
using Oje.Section.Ticket.Models.View;
using System.Collections.Generic;

namespace Oje.Section.Ticket.Interfaces
{
    public interface ITicketUserService
    {
        ApiResult Create(TicketUserCreateUpdateVM input, int? siteSettingId, long? loginUserId);
        object GetList(TicketUserMainGrid searchInput, int? siteSettingId, long? loginUserId);
        bool Exist(long? id, int? siteSettingId, long? loginUserId);
        string UpdateUserIdAndUdateDate(long? id, long? loginUserId);
        List<TicketUserItemListVM> GetAsListBy(long? id, int? siteSettingId, long? loginUserId);
        GridResultVM<TicketUserAdminMainGridResultVM> GetListForAdmin(TicketUserAdminMainGrid searchInput, int? siteSettingId);
        bool Exist(long? id, int? siteSettingId);
        List<TicketUserItemListVM> GetAsListBy(long? id, int? siteSettingId);
    }
}
