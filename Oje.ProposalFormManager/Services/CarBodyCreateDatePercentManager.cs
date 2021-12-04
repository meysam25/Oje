using Microsoft.EntityFrameworkCore;
using Oje.ProposalFormManager.Interfaces;
using Oje.ProposalFormManager.Models.DB;
using Oje.ProposalFormManager.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormManager.Services
{
    public class CarBodyCreateDatePercentManager: ICarBodyCreateDatePercentManager
    {
        readonly ProposalFormDBContext db = null;
        public CarBodyCreateDatePercentManager(ProposalFormDBContext db)
        {
            this.db = db;
        }

        public List<CarBodyCreateDatePercent> GetByList()
        {
            return db.CarBodyCreateDatePercents
                .Where(t => t.IsActive == true)
                .AsNoTracking()
                .ToList();
        }
    }
}
