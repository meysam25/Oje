using Microsoft.EntityFrameworkCore;
using Oje.FireInsuranceService.Interfaces;
using Oje.FireInsuranceService.Models.DB;
using Oje.FireInsuranceService.Services.EContext;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.FireInsuranceService.Services
{
    public class InsuranceContractDiscountService: IInsuranceContractDiscountService
    {
        readonly FireInsuranceServiceDBContext db = null;
        public InsuranceContractDiscountService(FireInsuranceServiceDBContext db)
        {
            this.db = db;
        }

        public InsuranceContractDiscount GetBy(int? siteSettingId, int? id, ProposalFormType type)
        {
            return db.InsuranceContractDiscounts
                .Where(t => t.SiteSettingId == siteSettingId && t.IsActive == true && t.InsuranceContract.ProposalForm.Type == type && t.Id == id)
                .Include(t => t.InsuranceContractDiscountCompanies)
                .AsNoTracking()
                .FirstOrDefault();
        }

        public object GetLightList(int? siteSettingId, ProposalFormType type)
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetEnumDisplayName() } };

            result.AddRange(db.InsuranceContractDiscounts
                .Where(t => t.IsActive == true && t.SiteSettingId == siteSettingId && t.IsActive == true && t.InsuranceContract.ProposalForm.Type == type)
                .Select(t => new { id = t.Id, title = t.Title })
                .ToList());

            return result;
        }
    }
}
