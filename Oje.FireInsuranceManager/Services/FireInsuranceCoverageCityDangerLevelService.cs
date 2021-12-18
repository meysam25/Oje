using Microsoft.EntityFrameworkCore;
using Oje.FireInsuranceService.Interfaces;
using Oje.FireInsuranceService.Models.DB;
using Oje.FireInsuranceService.Services.EContext;
using Oje.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.FireInsuranceService.Services
{
    public class FireInsuranceCoverageCityDangerLevelService: IFireInsuranceCoverageCityDangerLevelService
    {
        readonly FireInsuranceServiceDBContext db = null;
        public FireInsuranceCoverageCityDangerLevelService(FireInsuranceServiceDBContext db)
        {
            this.db = db;
        }

        public List<FireInsuranceCoverageCityDangerLevel> GetList(FireDangerGroupLevelType? fireDangerGroupLevel)
        {
            int dangerLevel = (int)fireDangerGroupLevel;
            return db.FireInsuranceCoverageCityDangerLevels
                .Where(t => t.IsActive == true && t.DangerStep == dangerLevel)
                .AsNoTracking()
                .ToList();
        }
    }
}
