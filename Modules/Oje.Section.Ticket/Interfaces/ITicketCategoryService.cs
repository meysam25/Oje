using Oje.Infrastructure.Models;
using Oje.Section.Ticket.Models.View;

namespace Oje.Section.Ticket.Interfaces
{
    public interface ITicketCategoryService
    {
        ApiResult Create(TicketCategoryCreateUpdateVM input, int? siteSettingId);
        ApiResult Delete(int? id, int? siteSettingId);
        object GetById(int? id, int? siteSettingId);
        ApiResult Update(TicketCategoryCreateUpdateVM input, int? siteSettingId);
        GridResultVM<TicketCategoryMainGridResultVM> GetList(TicketCategoryMainGrid searchInput, int? siteSettingId);
        object GetLightList(int? siteSettingId);
        object GetightListForSelect2(int ticketCategoryId, Select2SearchVM searchInput, int? siteSettingId);
        bool Exist(int? id, int? siteSettingId);
    }
}
