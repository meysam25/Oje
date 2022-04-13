using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.Ticket.Models.View
{
    public class TicketUserAdminMainGrid: GlobalGrid
    {
        public string title { get; set; }
        public bool? isAnswer { get; set; }
        public string createDate { get; set; }
        public string updateDate { get; set; }
        public string categoryTitle { get; set; }
        public string userfullname { get; set; }
        public string updateUserFullname { get; set; }
    }
}
