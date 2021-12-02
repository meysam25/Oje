using Microsoft.EntityFrameworkCore;
using Oje.FireInsuranceManager.Interfaces;
using Oje.FireInsuranceManager.Models.DB;
using Oje.FireInsuranceManager.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.FireInsuranceManager.Services
{
    public class DutyManager: IDutyManager
    {
        readonly FireInsuranceManagerDBContext db = null;
        public DutyManager(FireInsuranceManagerDBContext db)
        {
            this.db = db;
        }

        public Duty GetLastActiveItem()
        {
            return db.Duties.Where(t => t.IsActive == true).OrderByDescending(t => t.Year).AsNoTracking().FirstOrDefault();
        }
    }
}
