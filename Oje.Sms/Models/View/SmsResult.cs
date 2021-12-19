using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Sms.Models.View
{
    public class SmsResult
    {
        public bool isSuccess { get; set; }
        public string message { get; set; }
        public string traceCode { get; set; }
        public int? cId { get; set; }
    }
}
