using Microsoft.EntityFrameworkCore;
using Oje.PaymentService.Interfaces;
using Oje.PaymentService.Models.DB;
using Oje.PaymentService.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.PaymentService.Services
{
    public class UserService : IUserService
    {
        readonly PaymentDBContext db = null;
        public UserService(PaymentDBContext db)
        {
            this.db = db;
        }

        public User GetBy(long? loginUserId, int? siteSettingId)
        {
            return db.Users.Where(t => t.Id == loginUserId && t.SiteSettingId == siteSettingId).AsNoTracking().FirstOrDefault();
        }
    }
}
