using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.EmailService.Models.View
{
    public class EmailResult
    {
        public bool isSuccess { get; set; }
        public string message { get; set; }
        public int? cId { get; set; }
    }
}
