using Oje.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.PaymentService.Interfaces
{
    public interface IBankAccountDetectorService
    {
        BankAccountType GetByType(int bankAccountId, int? siteSettingId);
    }
}
