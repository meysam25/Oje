using Oje.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Infrastructure.Models
{
    public class PaymentFactorVM
    {
        public BankAccountFactorType type { get; set; }
        public long objectId { get; set; }
        public long price { get; set; }
        public string returnUrl { get; set; }
        public long? userId { get; set; }
    }
}
