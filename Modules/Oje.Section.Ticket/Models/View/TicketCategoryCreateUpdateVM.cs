using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.Ticket.Models.View
{
    public class TicketCategoryCreateUpdateVM
    {
        public long? id { get; set; }
        public int? pKey { get; set; }
        public string title { get; set; }
        public int? order { get; set; }
        public bool? isActive { get; set; }
    }
}
