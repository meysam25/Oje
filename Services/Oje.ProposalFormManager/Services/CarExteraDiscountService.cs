using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using Oje.ProposalFormService.Interfaces;
using Oje.ProposalFormService.Models.DB;
using Oje.ProposalFormService.Models.View;
using Oje.ProposalFormService.Services.EContext;
using System.Collections.Generic;
using System.Linq;

namespace Oje.ProposalFormService.Services
{
    public class CarExteraDiscountService : ICarExteraDiscountService
    {
        readonly ProposalFormDBContext db = null;
        public CarExteraDiscountService(ProposalFormDBContext db)
        {
            this.db = db;
        }

        public List<CarExteraDiscount> GetOptionSelectedCtrls(List<int> Ids, int? proposalFormId)
        {
            return db.CarExteraDiscounts
                    .Include(t => t.CarExteraDiscountCategory)
                    .Include(t => t.CarExteraDiscountValues).ThenInclude(t => t.CarExteraDiscountRangeAmounts).ThenInclude(t => t.CarExteraDiscountRangeAmountCompanies)
                    .Include(t => t.CarExteraDiscountRangeAmounts).ThenInclude(t => t.CarExteraDiscountRangeAmountCompanies)
                    //.Include(t => t.CarType.VehicleSystems)
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
                            (t.VehicleTypeId == null || t.VehicleTypeId == input.vehicleTypeId) &&
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

        public object GetRequiredQuestionsJsonCtrls(RequiredQuestionVM input, int? proposalFormId)
        {
            var allPanels = new List<object>();
            bool hasPrevInsurance = input.havePrevInsurance.ToIntReturnZiro() > 0;
            var allExteraDiscounts = db.CarExteraDiscounts
                .Include(t => t.CarExteraDiscountValues)
                .Include(t => t.CarExteraDiscountCategory)
                .OrderBy(t => t.Order)
                .Where(t =>
                            t.IsActive == true && t.IsOption == true && t.ProposalFormId == proposalFormId &&
                             (t.VehicleTypeId == null || t.VehicleTypeId == input.vehicleTypeId) &&
                             (t.HasPrevInsurance == null || t.HasPrevInsurance == hasPrevInsurance)
                        )
                .AsNoTracking()
                .ToList();


            var curCategoryCtrl = allExteraDiscounts;
            var allCtrls = new List<object>();
            for (var i = 0; i < curCategoryCtrl.Count; i++)
            {
                if (curCategoryCtrl[i].CarExteraDiscountValues.Count > 0)
                {
                    allCtrls.Add(new
                    {
                        parentCL = "col-xl-3 col-lg-4 col-md-6 col-sm-6 col-xs-12",
                        name = "exteraQuestions[" + i + "].value",
                        type = "dropDown",
                        textfield = "title",
                        valuefield = "id",
                        dataurl = "/ProposalFormInquiries/CarBodyInquiry/GetExteraFilterValuess?id=" + curCategoryCtrl[i].Id,
                        label = curCategoryCtrl[i].Title,
                        onChange = "refreshInquiryGrid('{{currentIdHolder}}');"
                    });
                    allCtrls.Add(new
                    {
                        name = "exteraQuestions[" + i + "].id",
                        type = "hidden",
                        dfaultValue = curCategoryCtrl[i].Id
                    });
                }
                else
                {
                    allCtrls.Add(new
                    {
                        parentCL = "col-xl-3 col-lg-4 col-md-6 col-sm-6 col-xs-12",
                        name = "exteraQuestions[" + i + "].value",
                        type = "dropDown",
                        textfield = "title",
                        valuefield = "id",
                        dataurl = "/Core/BaseData/Get/IsActive",
                        label = curCategoryCtrl[i].Title,
                        onChange = "refreshInquiryGrid('{{currentIdHolder}}');"
                    });
                    allCtrls.Add(new
                    {
                        name = "exteraQuestions[" + i + "].id",
                        type = "hidden",
                        dfaultValue = curCategoryCtrl[i].Id
                    });
                }
            }
            if (allCtrls.Count > 0)
                allPanels.Add(new
                {
                    id = "exteraCoverPanel_11",
                    title =  "پوشش اضافی" ,
                    @class = "col-xl-12 col-lg-12 col-md-12 col-sm-12 col-xs-12",
                    ctrls = allCtrls
                });



            return new
            {
                panels = allPanels
            };
        }

        public List<CarExteraDiscount> GetRequredValidCTRLs(int? proposalFormId, int vehicleSystemId, bool hasPrevInsurance)
        {
            return db.CarExteraDiscounts
                    .Include(t => t.CarExteraDiscountCategory)
                    .Include(t => t.CarExteraDiscountValues).ThenInclude(t => t.CarExteraDiscountRangeAmounts).ThenInclude(t => t.CarExteraDiscountRangeAmountCompanies)
                    .Include(t => t.CarExteraDiscountRangeAmounts).ThenInclude(t => t.CarExteraDiscountRangeAmountCompanies)
                    .Where(t => t.IsActive == true)
                    .AsNoTracking()
                    .Where(t => t.ProposalFormId == proposalFormId && t.IsOption == false && t.IsActive == true &&
                            (t.VehicleTypeId == null || t.VehicleType.VehicleSystemVehicleTypes.Any(tt => tt.VehicleSystemId == vehicleSystemId)) &&
                            (t.HasPrevInsurance == null || t.HasPrevInsurance == hasPrevInsurance)
                           )
                     .ToList()
                    ;
        }

        public object GetValuesForDD(int id)
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetEnumDisplayName() } };
            result.AddRange(db.CarExteraDiscounts.Where(t => t.Id == id).SelectMany(t => t.CarExteraDiscountValues).Select(t => new { id = t.Id, title = t.Title }).ToList());
            return result;
        }
    }
}
