using Oje.Infrastructure.Models;

namespace Oje.Section.Ticket.Models.View
{
    public class TicketCategoryCreateUpdateVM: GlobalSiteSetting
    {
        public long? id { get; set; }
        public int? pKey { get; set; }
        public string title { get; set; }
        public int? order { get; set; }
        public bool? isActive { get; set; }
    }
}
