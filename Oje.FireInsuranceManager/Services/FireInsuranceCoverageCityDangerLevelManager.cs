using Microsoft.EntityFrameworkCore;
using Oje.FireInsuranceManager.Interfaces;
using Oje.FireInsuranceManager.Models.DB;
using Oje.FireInsuranceManager.Services.EContext;
using Oje.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.FireInsuranceManager.Services
{
    public class FireInsuranceCoverageCityDangerLevelManager: IFireInsuranceCoverageCityDangerLevelManager
    {
        readonly FireInsuranceManagerDBContext db = null;
        public FireInsuranceCoverageCityDangerLevelManager(FireInsuranceManagerDBContext db)
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
