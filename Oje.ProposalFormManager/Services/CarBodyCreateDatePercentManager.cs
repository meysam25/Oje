using Microsoft.EntityFrameworkCore;
using Oje.ProposalFormService.Interfaces;
using Oje.ProposalFormService.Models.DB;
using Oje.ProposalFormService.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormService.Services
{
    public class CarBodyCreateDatePercentService: ICarBodyCreateDatePercentService
    {
        readonly ProposalFormDBContext db = null;
        public CarBodyCreateDatePercentService(ProposalFormDBContext db)
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
