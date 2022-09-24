using Microsoft.EntityFrameworkCore;
using Oje.ProposalFormService.Interfaces;
using Oje.ProposalFormService.Models.DB;
using Oje.ProposalFormService.Services.EContext;
using System.Collections.Generic;
using System.Linq;

namespace Oje.ProposalFormService.Services
{
    public class ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountService: IThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountService
    {
        readonly ProposalFormDBContext db = null;
        public ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountService
            (
                ProposalFormDBContext db
            )
        {
            this.db = db;
        }

        public List<ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscount> GetListBy(int? vehicleTypeId, int? siteSettingId)
        {
            return db.ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscounts
                .Where(t => t.VehicleTypeId == vehicleTypeId && t.SiteSettingId == siteSettingId)
                .Include(t => t.ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountCompanies)
                .AsNoTracking()
                .ToList();
        }
    }
}
