using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.Ticket.Models.View
{
    public class TicketUserAnswerCreateUpdateVM
    {
        public string answer { get; set; }
        public long? pKey { get; set; }
        public IFormFile mainFile { get; set; }
    }
}
