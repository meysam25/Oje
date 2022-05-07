using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.PaymentService.Models.View
{
    public class BankMainGrid: GlobalGrid
    {
        public string title { get; set; }
        public int? code { get; set; }
        public bool? isActive { get; set; }
    }
}
