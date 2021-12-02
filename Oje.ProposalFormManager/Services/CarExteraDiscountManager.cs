using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using Oje.ProposalFormManager.Interfaces;
using Oje.ProposalFormManager.Models.DB;
using Oje.ProposalFormManager.Models.View;
using Oje.ProposalFormManager.Services.EContext;
using System.Collections.Generic;
using System.Linq;

namespace Oje.ProposalFormManager.Services
{
    public class CarExteraDiscountManager : ICarExteraDiscountManager
    {
        readonly ProposalFormDBContext db = null;
        public CarExteraDiscountManager(ProposalFormDBContext db)
        {
            this.db = db;
        }

        public List<CarExteraDiscount> GetOptionSelectedCtrls(List<int> Ids, int? proposalFormId)
        {
            return db.CarExteraDiscounts
                    .Include(t => t.CarExteraDiscountCategory)
                    .Include(t => t.CarExteraDiscountValues).ThenInclude(t => t.CarExteraDiscountRangeAmounts).ThenInclude(t => t.CarExteraDiscountRangeAmountCompanies)
                    .Include(t => t.CarExteraDiscountRangeAmounts).ThenInclude(t => t.CarExteraDiscountRangeAmountCompanies)
                    .Include(t => t.CarType.VehicleSystems)
                    .Where(t => t.IsActive == true && Ids.Contains(t.Id) && t.IsOption == true && t.ProposalFormId == proposalFormId)
                    .AsNoTracking()
                    .ToList();
        }

        public object GetRequiredQuestions(RequiredQuestionVM input, int? proposalFormId)
        {
            List<object> result = new List<object>();
            bool hasPrevInsurance = input.havePrevInsurance.ToIntReturnZiro() > 0;
            var tempResult = db.CarExteraDiscounts
                .OrderBy(t => t.Order)
                .Where(t => t.IsActive == true && t.ProposalFormId == proposalFormId && t.IsOption == false &&
                            (t.CarTypeId == null || t.CarType.VehicleSystems.Any(tt => tt.Id == input.brandId)) &&
                            (t.HasPrevInsurance == null || t.HasPrevInsurance == hasPrevInsurance)
                       )
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title,
                    values = t.CarExteraDiscountValues.Where(tt => tt.IsActive == true).Select(tt => new
                    {
                        id = tt.Id,
                        title = tt.Title
                    }).ToList()
                }).ToList();

            foreach (var item in tempResult)
            {
                List<object> valueTempObj = new List<object>();
                for (var i = 0; i < item.values.Count(); i++)
                {
                    if (i == 0)
                        valueTempObj.Add(new { id = "", title = BMessages.Please_Select_One_Item.GetEnumDisplayName() });
                    valueTempObj.Add(new { id = item.values[i].id, title = item.values[i].title });
                }

                result.Add(new { id = item.id, title = item.title, values = valueTempObj });
            }

            return result;
        }

        public List<CarExteraDiscount> GetRequredValidCTRLs(int? proposalFormId, int vehicleSystemId, List<int> validCompanyIds)
        {
            if (validCompanyIds == null)
                validCompanyIds = new List<int>();

            return db.CarExteraDiscounts
                    .Include(t => t.CarExteraDiscountCategory)
                    .Include(t => t.CarExteraDiscountValues).ThenInclude(t => t.CarExteraDiscountRangeAmounts).ThenInclude(t => t.CarExteraDiscountRangeAmountCompanies)
                    .Include(t => t.CarExteraDiscountRangeAmounts).ThenInclude(t => t.CarExteraDiscountRangeAmountCompanies)
                    .Include(t => t.CarType.VehicleSystems)
                    .Where(t => t.IsActive == true)
                    .AsNoTracking()
                    .Where(t => t.ProposalFormId == proposalFormId && t.IsOption == false &&
                            (validCompanyIds.Count == 0 || t.CarExteraDiscountRangeAmounts.Any(tt => tt.CarExteraDiscountRangeAmountCompanies.Any(ttt => validCompanyIds.Contains(ttt.CompanyId)))) &&
                            (t.CarTypeId == null || t.CarType.VehicleSystems.Any(tt => tt.Id == vehicleSystemId))
                           )
                     .ToList()
                    ;
        }
    }
}
