using Microsoft.EntityFrameworkCore;
using Oje.FireInsuranceService.Interfaces;
using Oje.FireInsuranceService.Models.DB;
using Oje.FireInsuranceService.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.FireInsuranceService.Services
{
    public class DutyService: IDutyService
    {
        readonly FireInsuranceServiceDBContext db = null;
        public DutyService(FireInsuranceServiceDBContext db)
        {
            this.db = db;
        }

        public Duty GetLastActiveItem()
        {
            return db.Duties.Where(t => t.IsActive == true).OrderByDescending(t => t.Year).AsNoTracking().FirstOrDefault();
        }
    }
}
