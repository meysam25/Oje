using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.Ticket.Models.View
{
    public class TicketUserItemListVM
    {
        public long id { get; set; }
        public string message { get; set; }
        public string createDate { get; set; }
        public bool isMyMessage { get; set; }
        public string createUsername { get; set; }
        public string fileUrl { get; set; }
    }
}
