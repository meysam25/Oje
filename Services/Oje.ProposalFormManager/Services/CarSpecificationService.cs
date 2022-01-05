using Microsoft.EntityFrameworkCore;
using Oje.ProposalFormService.Interfaces;
using Oje.ProposalFormService.Models.DB;
using Oje.ProposalFormService.Services.EContext;
using System.Linq;

namespace Oje.ProposalFormService.Services
{
    public class CarSpecificationService : ICarSpecificationService
    {
        readonly ProposalFormDBContext db = null;
        public CarSpecificationService(ProposalFormDBContext db)
        {
            this.db = db;
        }

        public CarSpecification GetById(int? id)
        {
            return db.CarSpecifications.Where(t => t.CarSpecificationVehicleSpecs.Any(tt => tt.VehicleSpecId == id)).AsNoTracking().FirstOrDefault();
        }
    }
}
