using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.Ticket.Models.View
{
    public class TicketUserCreateUpdateVM
    {
        public string title { get; set; }
        public int? subCId { get; set; }
        public string des { get; set; }
        public IFormFile mainFile { get; set; }
    }
}
