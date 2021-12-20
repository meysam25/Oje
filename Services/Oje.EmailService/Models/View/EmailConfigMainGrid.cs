using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.EmailService.Models.View
{
    public class EmailConfigMainGrid: GlobalGrid
    {
        public string title { get; set; }
        public string eUsername { get; set; }
        public int? smtpPort { get; set; }
        public string smtpHost { get; set; }
        public bool? enableSsl { get; set; }
        public int timeout { get; set; }
        public bool? isActive { get; set; }
    }
}
