using Oje.PaymentService.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.PaymentService.Interfaces
{
    public interface IUserService
    {
        User GetBy(long? loginUserId, int? siteSettingId);
    }
}
