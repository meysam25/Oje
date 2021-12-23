using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.PaymentService.Models.View
{
    public class BankAccountMainGrid: GlobalGrid
    {
        public int? bankId { get; set; }
        public string title { get; set; }
        public string userfullanme { get; set; }
        public long? cardNo { get; set; }
        public long? hesabNo { get; set; }
        public bool? isActive { get; set; }
    }
}
