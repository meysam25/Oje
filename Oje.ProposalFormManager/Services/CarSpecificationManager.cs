using Microsoft.EntityFrameworkCore;
using Oje.ProposalFormManager.Interfaces;
using Oje.ProposalFormManager.Models.DB;
using Oje.ProposalFormManager.Services.EContext;
using System.Linq;

namespace Oje.ProposalFormManager.Services
{
    public class CarSpecificationManager: ICarSpecificationManager
    {
        readonly ProposalFormDBContext db = null;
        public CarSpecificationManager(ProposalFormDBContext db)
        {
            this.db = db;
        }

        public CarSpecification GetById(int? id)
        {
            return db.CarSpecifications.Where(t => t.Id == id).AsNoTracking().FirstOrDefault();
        }
    }
}
