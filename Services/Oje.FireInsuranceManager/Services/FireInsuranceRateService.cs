using Microsoft.EntityFrameworkCore;
using Oje.FireInsuranceService.Interfaces;
using Oje.FireInsuranceService.Models.DB;
using Oje.FireInsuranceService.Models.View;
using Oje.FireInsuranceService.Services.EContext;
using Oje.Infrastructure;
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
    public class FireInsuranceRateService : IFireInsuranceRateService
    {
        readonly FireInsuranceServiceDBContext db = null;
        readonly IProvinceService ProvinceService = null;
        readonly ICityService CityService = null;
        readonly IFireInsuranceBuildingUnitValueService FireInsuranceBuildingUnitValueService = null;
        readonly IFireInsuranceBuildingTypeService FireInsuranceBuildingTypeService = null;
        readonly IFireInsuranceBuildingBodyService FireInsuranceBuildingBodyService = null;
        readonly IFireInsuranceBuildingAgeService FireInsuranceBuildingAgeService = null;
        readonly ITaxService TaxService = null;
        readonly IDutyService DutyService = null;
        readonly IProposalFormService ProposalFormService = null;
        readonly IInquiryMaxDiscountService InquiryMaxDiscountService = null;
        readonly IInquiryCompanyLimitService InquiryCompanyLimitService = null;
        readonly IInquiryDurationService InquiryDurationService = null;
        readonly IInsuranceContractDiscountService InsuranceContractDiscountService = null;
        readonly IGlobalDiscountService GlobalDiscountService = null;
        readonly ICashPayDiscountService CashPayDiscountService = null;
        readonly IPaymentMethodService PaymentMethodService = null;
        readonly IGlobalInputInqueryService GlobalInputInqueryService = null;
        readonly IRoundInqueryService RoundInqueryService = null;
        readonly IGlobalInqueryService GlobalInqueryService = null;
        readonly IInqueryDescriptionService InqueryDescriptionService = null;
        readonly IFireInsuranceTypeOfActivityService FireInsuranceTypeOfActivityService = null;
        readonly IFireInsuranceCoverageTitleService FireInsuranceCoverageTitleService = null;
        readonly IFireInsuranceCoverageService FireInsuranceCoverageService = null;
        readonly IFireInsuranceCoverageCityDangerLevelService FireInsuranceCoverageCityDangerLevelService = null;
        public FireInsuranceRateService(
                FireInsuranceServiceDBContext db,
                IProvinceService ProvinceService,
                ICityService CityService,
                IFireInsuranceBuildingUnitValueService FireInsuranceBuildingUnitValueService,
                IFireInsuranceBuildingTypeService FireInsuranceBuildingTypeService,
                IFireInsuranceBuildingBodyService FireInsuranceBuildingBodyService,
                IFireInsuranceBuildingAgeService FireInsuranceBuildingAgeService,
                ITaxService TaxService,
                IDutyService DutyService,
                IProposalFormService ProposalFormService,
                IInquiryMaxDiscountService InquiryMaxDiscountService,
                IInquiryCompanyLimitService InquiryCompanyLimitService,
                IInquiryDurationService InquiryDurationService,
                IInsuranceContractDiscountService InsuranceContractDiscountService,
                IGlobalDiscountService GlobalDiscountService,
                ICashPayDiscountService CashPayDiscountService,
                IPaymentMethodService PaymentMethodService,
                IGlobalInputInqueryService GlobalInputInqueryService,
                IRoundInqueryService RoundInqueryService,
                IGlobalInqueryService GlobalInqueryService,
                IInqueryDescriptionService InqueryDescriptionService,
                IFireInsuranceTypeOfActivityService FireInsuranceTypeOfActivityService,
                IFireInsuranceCoverageTitleService FireInsuranceCoverageTitleService,
                IFireInsuranceCoverageService FireInsuranceCoverageService,
                IFireInsuranceCoverageCityDangerLevelService FireInsuranceCoverageCityDangerLevelService
            )
        {
            this.db = db;
            this.ProvinceService = ProvinceService;
            this.CityService = CityService;
            this.FireInsuranceBuildingUnitValueService = FireInsuranceBuildingUnitValueService;
            this.FireInsuranceBuildingTypeService = FireInsuranceBuildingTypeService;
            this.FireInsuranceBuildingBodyService = FireInsuranceBuildingBodyService;
            this.FireInsuranceBuildingAgeService = FireInsuranceBuildingAgeService;
            this.TaxService = TaxService;
            this.DutyService = DutyService;
            this.InquiryMaxDiscountService = InquiryMaxDiscountService;
            this.ProposalFormService = ProposalFormService;
            this.InquiryCompanyLimitService = InquiryCompanyLimitService;
            this.InquiryDurationService = InquiryDurationService;
            this.InsuranceContractDiscountService = InsuranceContractDiscountService;
            this.GlobalDiscountService = GlobalDiscountService;
            this.CashPayDiscountService = CashPayDiscountService;
            this.PaymentMethodService = PaymentMethodService;
            this.GlobalInputInqueryService = GlobalInputInqueryService;
            this.RoundInqueryService = RoundInqueryService;
            this.GlobalInqueryService = GlobalInqueryService;
            this.InqueryDescriptionService = InqueryDescriptionService;
            this.FireInsuranceTypeOfActivityService = FireInsuranceTypeOfActivityService;
            this.FireInsuranceCoverageTitleService = FireInsuranceCoverageTitleService;
            this.FireInsuranceCoverageService = FireInsuranceCoverageService;
            this.FireInsuranceCoverageCityDangerLevelService = FireInsuranceCoverageCityDangerLevelService;
        }

        public object Inquiry(int? siteSettingId, FireInsuranceInquiryVM input, string targetArea)
        {

            if (inquiryValidation(siteSettingId, input, null))
            {
                FireInsuranceInquiryFilledObj filledObj = inquiryFilledObjectAndValidation(siteSettingId, input);
                if (filledObj.Companies != null && filledObj.Companies.Count > 0)
                {
                    List<GlobalInquery> result = new List<GlobalInquery>();
                    long GlobalInputInqueryId = GlobalInputInqueryService.Create(input, siteSettingId);

                    addBasePrice(filledObj, result, GlobalInputInqueryId, siteSettingId);
                    addCacleForBuildingAge(filledObj, result);
                    addCalceForCoverages(filledObj, result, input);
                    addCacleDiscountContract(filledObj, result);
                    addCacleGlobalDiscountAuto(filledObj, result);
                    addCacleGlobalDiscount(filledObj, result, siteSettingId, input.discountCode);
                    addCacleDayLimitationDiscount(filledObj, result);
                    addCacleAddCashDiscount(result, filledObj);
                    InquiryCalceTaxAndDuty(result, filledObj);

                    GlobalInqueryService.Create(result);

                    return calceResponeForInquiry(result, filledObj, input, targetArea);
                }

            }

            return new { total = 0, data = new List<object>() };
        }

        private void addCalceForCoverages(FireInsuranceInquiryFilledObj filledObj, List<GlobalInquery> result, FireInsuranceInquiryVM input)
        {
            if (filledObj.FireInsuranceCoverageTitles != null && filledObj.FireInsuranceCoverageTitles.Count > 0 && input.exteraQuestions != null && input.exteraQuestions.Count > 0)
            {
                foreach (var r in result)
                {
                    foreach (var coverTitle in filledObj.FireInsuranceCoverageTitles)
                    {
                        if (coverTitle.FireInsuranceCoverageActivityDangerLevels != null && coverTitle.FireInsuranceCoverageActivityDangerLevels.Count > 0)
                        {
                            var currentUserInputValue = input.exteraQuestions.Where(t => t.id == coverTitle.Id).FirstOrDefault();

                            if (filledObj.FireInsuranceTypeOfActivities != null && currentUserInputValue != null && currentUserInputValue.value.ToIntReturnZiro() > 0)
                            {
                                var foundCurrActivity = filledObj.FireInsuranceTypeOfActivities.Where(t => t.IsActive == true && t.Id == currentUserInputValue.value.ToIntReturnZiro()).FirstOrDefault();
                                if (foundCurrActivity != null)
                                {
                                    var foundRateOfActivity = coverTitle.FireInsuranceCoverageActivityDangerLevels.Where(t => t.IsActive == true && t.DangerStep == foundCurrActivity.DangerStep).FirstOrDefault();
                                    if (foundRateOfActivity != null)
                                    {
                                        createNewItemForInquery(r, foundRateOfActivity.Rate, coverTitle.EffectOn, null, filledObj, input.assetValue, coverTitle.Title);
                                    }
                                }
                            }
                        }
                        else if (filledObj.FireInsuranceCoverageCityDangerLevels.Any(tt => tt.FireInsuranceCoverageTitleId == coverTitle.Id && tt.IsActive == true))
                        {
                            var currentUserInputValue = input.exteraQuestions.Where(t => t.id == coverTitle.Id).FirstOrDefault();
                            var foundCityRate = filledObj.FireInsuranceCoverageCityDangerLevels.Where(tt => tt.FireInsuranceCoverageTitleId == coverTitle.Id).FirstOrDefault();
                            if (currentUserInputValue != null && foundCityRate != null && !string.IsNullOrEmpty(currentUserInputValue.value) && currentUserInputValue.value.ToLower() == "true")
                            {
                                createNewItemForInquery(r, foundCityRate.Rate, coverTitle.EffectOn, null, filledObj, input.assetValue, coverTitle.Title);
                            }
                        }
                        else if (filledObj.FireInsuranceCoverages != null && filledObj.FireInsuranceCoverages.Any(t => t.FireInsuranceCoverageTitleId == coverTitle.Id && t.IsActive == true && t.FireInsuranceCoverageCompanies.Any(tt => tt.CompanyId == r.CompanyId)))
                        {
                            var currentUserInputValue = input.exteraQuestions.Where(t => t.id == coverTitle.Id).FirstOrDefault();
                            var foundCoverRate = filledObj.FireInsuranceCoverages.Where(t => t.FireInsuranceCoverageTitleId == coverTitle.Id && t.IsActive == true && t.FireInsuranceCoverageCompanies.Any(tt => tt.CompanyId == r.CompanyId)).FirstOrDefault();
                            if (foundCoverRate != null && !string.IsNullOrEmpty(currentUserInputValue.value) && currentUserInputValue.value.ToLower() != "false")
                            {
                                createNewItemForInquery(r, foundCoverRate.Rate, coverTitle.EffectOn, currentUserInputValue.value.ToLongReturnZiro(), filledObj, input.assetValue, coverTitle.Title);
                            }
                        }
                    }
                }

            }
        }

        private void createNewItemForInquery(GlobalInquery r, decimal? rate, FireInsuranceCoverageEffectOn effectOn, long? userInput, FireInsuranceInquiryFilledObj filledObj, long? assetValue, string coverTitle)
        {
            long targetPrice = 0;
            if (effectOn == FireInsuranceCoverageEffectOn.InputByUser)
                targetPrice = userInput.ToLongReturnZiro();
            else if (effectOn == FireInsuranceCoverageEffectOn.Investment)
                targetPrice = filledObj.sarmaye;
            else if (effectOn == FireInsuranceCoverageEffectOn.AssetsValue)
                targetPrice = assetValue.ToLongReturnZiro();
            else if (effectOn == FireInsuranceCoverageEffectOn.ValueOfBuilding)
                targetPrice = filledObj.arzeshSakhteman;

            if (targetPrice > 0)
            {
                if (rate != null)
                {
                    var newRow = new GlobalInquiryItem()
                    {
                        CalcKey = "cover",
                        GlobalInquiryId = r.Id,
                        Title = coverTitle
                    };
                    newRow.Price = RouteThisNumberIfConfigExist((Convert.ToDecimal(targetPrice * rate)).ToLongReturnZiro(), filledObj.RoundInquery);
                    r.GlobalInquiryItems.Add(newRow);
                    return;
                }
            }
            r.deleteMe = true;
        }

        private void addCacleForBuildingAge(FireInsuranceInquiryFilledObj filledObj, List<GlobalInquery> result)
        {

            if (filledObj.FireInsuranceBuildingAge != null && filledObj.FireInsuranceBuildingAge.Percent > 0)
            {
                foreach (var newItem in result)
                {
                    var s1Price = newItem.GlobalInquiryItems.Where(t => t.CalcKey == "s1").Select(t => t.Price).FirstOrDefault();
                    if (s1Price > 0)
                    {
                        var ageRow = new GlobalInquiryItem()
                        {
                            CalcKey = "age",
                            GlobalInquiryId = newItem.Id,
                            Price = RouteThisNumberIfConfigExist((Convert.ToDecimal(s1Price * filledObj.FireInsuranceBuildingAge.Percent) / filledObj.v100).ToLongReturnZiro(), filledObj.RoundInquery),
                            Title = String.Format(BMessages.Building_Age.GetEnumDisplayName(), filledObj.FireInsuranceBuildingAge.Title)
                        };
                        if (ageRow.Price > 0)
                            newItem.GlobalInquiryItems.Add(ageRow);
                    }

                }
            }
        }

        object calceResponeForInquiry(List<GlobalInquery> quiryObj, FireInsuranceInquiryFilledObj objPack, FireInsuranceInquiryVM input, string targetArea)
        {
            int row = 0;
            var result = quiryObj
                .Where(t => t.deleteMe != true)
                .Select(t => new
                {
                    id = t.Id,
                    idG = t.Id,
                    cid = t.CompanyId,
                    p = t.GlobalInquiryItems.Where(tt => tt.CalcKey == "s1").Select(tt => tt.Price).FirstOrDefault(),
                    cn = objPack.Companies.Where(x => x.Id == t.CompanyId).Select(x => x.Title).FirstOrDefault(),
                    cnPic = objPack.Companies.Where(x => x.Id == t.CompanyId).Select(x => x.Pic).FirstOrDefault(),
                    desc = objPack.InqueryDescriptions.OrderByDescending(tt => tt.SiteSettingId).Where(tt => tt.InqueryDescriptionCompanies.Any(ttt => ttt.CompanyId == t.CompanyId)).Select(tt => new
                    {
                        title = tt.Title,
                        desc = tt.Description
                    }).FirstOrDefault(),
                    hcd = t.GlobalInquiryItems.Any(tt => tt.CalcKey == "BCash"),
                    hdp = objPack.PaymentMethods.Any(tt => tt.Type == PaymentMethodType.Cash && tt.PaymentMethodCompanies.Any(ttt => ttt.CompanyId == t.CompanyId)),
                    hod = objPack.PaymentMethods.Any(tt => tt.Type == PaymentMethodType.Debit && tt.PaymentMethodCompanies.Any(ttt => ttt.CompanyId == t.CompanyId)),
                    dt = t.GlobalInquiryItems.OrderBy(tt => tt.Order).Select(tt => new
                    {
                        t = tt.Title,
                        p = tt.Price,
                        isE = tt.Expired,
                        c = tt.CalcKey,
                        bp = tt.basePriceEC
                    })
                }).ToList()
                .Select(t => new
                {
                    row = ++row,
                    i = t.idG,
                    t.hcd,
                    t.hdp,
                    t.hod,
                    t.id,
                    p = t.p.ToString("###,###") + " ریال",
                    sr = GlobalConfig.FileAccessHandlerUrl + t.cnPic,
                    t.cn,
                    sp = (t.dt.Where(tt => tt.isE != true).Count() > 0 ? t.dt.Where(tt => tt.isE != true).Sum(tt => tt.p).ToString("###,###") : "0") + " ریال",
                    dt = new
                    {
                        total = t.dt.Count(),
                        data = t.dt.Select(tt => new
                        {
                            t = tt.t,
                            p = (tt.p < 0 ? tt.p * -1 : tt.p).ToString("###,###") + (tt.p > 0 ? "+" : tt.p < 0 ? "-" : "") + " ریال",
                            tt.isE,
                            isET = tt.isE == true ? "عدم اعمال در محاصبات" : "اعمال",
                            tt.c,
                            tt.bp
                        }).ToList()
                    },
                    t.cid,
                    t.desc,
                    fid = objPack.ProposalForm?.Id,
                    targetArea = targetArea
                }).ToList();


            if (input.showStatus == 2)
                result = result.Where(t => t.hcd == true).ToList();
            else if (input.showStatus == 3)
                result = result.Where(t => t.hcd == true).ToList();
            return new { total = result.Count, data = result };
        }

        void InquiryCalceTaxAndDuty(List<GlobalInquery> result, FireInsuranceInquiryFilledObj objPack)
        {
            if (objPack.Tax != null && objPack.Duty != null)
                foreach (var newQueryItem in result)
                {
                    if (objPack.Tax != null && objPack.Duty != null && newQueryItem != null && newQueryItem.GlobalInquiryItems != null && newQueryItem.GlobalInquiryItems.Count > 0)
                    {
                        var sumPrice = newQueryItem.GlobalInquiryItems.Where(t => t.Expired != true).Sum(t => t.Price);
                        if (sumPrice > 0)
                        {
                            var tax = RouteThisNumberIfConfigExist(Math.Ceiling((Convert.ToDecimal(sumPrice) * objPack.Tax.Percent) / objPack.v100).ToLongReturnZiro(), objPack.RoundInquery);
                            var duty = RouteThisNumberIfConfigExist(Math.Ceiling((Convert.ToDecimal(sumPrice) * objPack.Duty.Percent) / objPack.v100).ToLongReturnZiro(), objPack.RoundInquery);

                            if (tax > 0)
                            {
                                GlobalInquiryItem newITem = new GlobalInquiryItem();
                                newITem.CalcKey = "tax";
                                newITem.Title = BMessages.Tax.GetEnumDisplayName();
                                newITem.GlobalInquiryId = newQueryItem.Id;
                                newITem.Price = tax;
                                if (newITem.Price > 0)
                                    newQueryItem.GlobalInquiryItems.Add(newITem);
                            }
                            if (duty > 0)
                            {
                                GlobalInquiryItem newITem = new GlobalInquiryItem();
                                newITem.CalcKey = "duty";
                                newITem.Title = BMessages.Duty.GetEnumDisplayName();
                                newITem.GlobalInquiryId = newQueryItem.Id;
                                newITem.Price = duty;
                                if (newITem.Price > 0)
                                    newQueryItem.GlobalInquiryItems.Add(newITem);
                            }
                        }
                    }
                }
        }

        void addCacleAddCashDiscount(List<GlobalInquery> result, FireInsuranceInquiryFilledObj objPack)
        {
            if (objPack.CashPayDiscounts != null && objPack.CashPayDiscounts.Count > 0)
            {
                List<GlobalInquery> cResultSaved = new List<GlobalInquery>();
                foreach (var cResult in result)
                {
                    if (cResult.deleteMe != true)
                    {
                        CashPayDiscount foundCurCashPayed = objPack.CashPayDiscounts.OrderByDescending(t => t.Id).Where(t => t.CashPayDiscountCompanies.Any(tt => tt.CompanyId == cResult.CompanyId)).FirstOrDefault();

                        if (foundCurCashPayed != null && foundCurCashPayed.Percent > 0)
                        {
                            GlobalInquery newItem = new GlobalInquery();
                            newItem.CompanyId = cResult.CompanyId;
                            if (cResult.GlobalInputInqueryId.ToLongReturnZiro() > 0)
                                newItem.GlobalInputInqueryId = cResult.GlobalInputInqueryId;
                            newItem.SiteSettingId = cResult.SiteSettingId;
                            newItem.ProposalFormId = cResult.ProposalFormId;

                            cResultSaved.Add(newItem);

                            foreach (var item in cResult.GlobalInquiryItems)
                            {
                                GlobalInquiryItem newItemItem = new GlobalInquiryItem();
                                newItemItem.GlobalInquiryId = newItem.Id;
                                newItemItem.CalcKey = item.CalcKey;
                                newItemItem.Title = item.Title;
                                newItemItem.Price = item.Price;
                                newItemItem.Expired = item.Expired;
                                newItemItem.basePriceEC = item.basePriceEC;
                                newItem.GlobalInquiryItems.Add(newItemItem);
                            }

                            long priceX = cResult.GlobalInquiryItems.Where(t => t.Expired != true).Sum(t => t.Price);

                            GlobalInquiryItem newItemItem2 = new GlobalInquiryItem();
                            newItemItem2.GlobalInquiryId = newItem.Id;
                            newItemItem2.CalcKey = "BCash";
                            newItemItem2.Title = foundCurCashPayed.Title;
                            newItemItem2.Price = RouteThisNumberIfConfigExist(Convert.ToInt64(Math.Ceiling((Convert.ToDecimal(priceX) * foundCurCashPayed.Percent) / objPack.v100)) * -1, objPack.RoundInquery);
                            newItem.GlobalInquiryItems.Add(newItemItem2);
                        }
                    }
                }
                result.AddRange(cResultSaved);
            }
        }

        void addCacleDiscountContract(FireInsuranceInquiryFilledObj objPack, List<GlobalInquery> result)
        {
            foreach (var newQueryItem in result)
            {
                if (objPack.InsuranceContractDiscount != null && objPack.InsuranceContractDiscount.Percent > 0)
                {
                    if (objPack.InsuranceContractDiscount.InsuranceContractDiscountCompanies.Any(tt => tt.CompanyId == newQueryItem.CompanyId))
                    {
                        GlobalInquiryItem newItem = new GlobalInquiryItem();
                        newItem.GlobalInquiryId = newQueryItem.Id;
                        newItem.Title = objPack.InsuranceContractDiscount.Title;
                        newItem.CalcKey = "CD";
                        var sumItemPrice = newQueryItem.GlobalInquiryItems.Where(t => t.Expired != true).Select(t => t.Price).Sum();
                        if (objPack.InsuranceContractDiscount.Percent > 0)
                        {
                            newItem.Price = RouteThisNumberIfConfigExist(Convert.ToInt64(Math.Ceiling((Convert.ToDecimal((sumItemPrice)) * objPack.InsuranceContractDiscount.Percent) / objPack.v100)) * -1, objPack.RoundInquery);
                            newQueryItem.maxDiscount += objPack.InsuranceContractDiscount.Percent;
                            if (objPack.InquiryMaxDiscounts.Any(t => t.Percent < newQueryItem.maxDiscount && t.InquiryMaxDiscountCompanies.Any(tt => tt.CompanyId == newQueryItem.CompanyId)))
                            {
                                newItem.Title = string.Format(BMessages.Expired_Discount.GetEnumDisplayName(), newItem.Title);
                                newItem.Expired = true;
                            }
                            newQueryItem.GlobalInquiryItems.Add(newItem);
                        }
                    }
                    else
                        newQueryItem.deleteMe = true;
                }
            }
        }

        void addCacleGlobalDiscount(FireInsuranceInquiryFilledObj objPack, List<GlobalInquery> result, int? siteSettingId, string discountCode)
        {
            if (!string.IsNullOrEmpty(discountCode))
            {
                GlobalDiscount foundDiscountCode = GlobalDiscountService.GetByCode(discountCode, siteSettingId, objPack.ProposalForm?.Id);
                if (foundDiscountCode != null)
                {
                    foreach (var newQueryItem in result)
                    {
                        if (foundDiscountCode.GlobalDiscountCompanies.Any(tt => tt.CompanyId == newQueryItem.CompanyId))
                        {
                            GlobalInquiryItem newItem = new GlobalInquiryItem();
                            newItem.GlobalInquiryId = newQueryItem.Id;
                            newItem.Title = foundDiscountCode.Title;
                            newItem.CalcKey = "ADI";
                            if (foundDiscountCode.Percent > 0)
                            {
                                long price = newQueryItem.GlobalInquiryItems.Where(t => t.Expired != true).Sum(t => t.Price);
                                if (!string.IsNullOrEmpty(foundDiscountCode.InqueryCode))
                                    price = newQueryItem.GlobalInquiryItems.Where(t => t.CalcKey == foundDiscountCode.InqueryCode).Select(t => t.Price).FirstOrDefault();
                                if (price > 0)
                                {
                                    newItem.Price = RouteThisNumberIfConfigExist(Convert.ToInt64(Math.Ceiling((Convert.ToDecimal((price)) * foundDiscountCode.Percent.Value) / objPack.v100)) * -1, objPack.RoundInquery);
                                    if (foundDiscountCode.MaxPrice > 0 && newItem.Price > foundDiscountCode.MaxPrice)
                                        newItem.Price = foundDiscountCode.MaxPrice;
                                    newQueryItem.maxDiscount += foundDiscountCode.Percent.Value;
                                    if (objPack.InquiryMaxDiscounts.Any(t => t.Percent < newQueryItem.maxDiscount && t.InquiryMaxDiscountCompanies.Any(tt => tt.CompanyId == newQueryItem.CompanyId)))
                                    {
                                        newItem.Title = string.Format(BMessages.Expired_Discount.GetEnumDisplayName(), newItem.Title);
                                        newItem.Expired = true;
                                    }
                                }
                            }
                            else if (foundDiscountCode.Price > 0)
                                newItem.Price = foundDiscountCode.Price.Value * -1;
                            if (newItem.Price < 0)
                            {
                                newItem.GlobalDiscountId = foundDiscountCode.Id;
                                newQueryItem.GlobalInquiryItems.Add(newItem);
                            }
                        }
                    }
                }
            }
        }

        private void addCacleGlobalDiscountAuto(FireInsuranceInquiryFilledObj objPack, List<GlobalInquery> result)
        {
            if (objPack.GlobalDiscounts == null)
                return;
            foreach (var newQueryItem in result)
            {
                var foundDiscountCode = objPack.GlobalDiscounts.Where(t => t.GlobalDiscountCompanies.Any(tt => tt.CompanyId == newQueryItem.CompanyId)).FirstOrDefault();
                if (foundDiscountCode != null)
                {
                    GlobalInquiryItem newItem = new GlobalInquiryItem();
                    newItem.GlobalInquiryId = newQueryItem.Id;
                    newItem.Title = foundDiscountCode.Title;
                    newItem.CalcKey = "ADI";
                    if (foundDiscountCode.Percent > 0)
                    {
                        long price = newQueryItem.GlobalInquiryItems.Where(t => t.Expired != true).Sum(t => t.Price);
                        if (!string.IsNullOrEmpty(foundDiscountCode.InqueryCode))
                            price = newQueryItem.GlobalInquiryItems.Where(t => t.CalcKey == foundDiscountCode.InqueryCode).Select(t => t.Price).FirstOrDefault();
                        if (price > 0)
                        {
                            newItem.Price = RouteThisNumberIfConfigExist(Convert.ToInt64(Math.Ceiling((Convert.ToDecimal((price)) * foundDiscountCode.Percent.Value) / objPack.v100)) * -1, objPack.RoundInquery);
                            if (foundDiscountCode.MaxPrice > 0 && newItem.Price > foundDiscountCode.MaxPrice)
                                newItem.Price = foundDiscountCode.MaxPrice;
                            newQueryItem.maxDiscount += foundDiscountCode.Percent.Value;
                            if (objPack.InquiryMaxDiscounts.Any(t => t.Percent < newQueryItem.maxDiscount && t.InquiryMaxDiscountCompanies.Any(tt => tt.CompanyId == newQueryItem.CompanyId)))
                            {
                                newItem.Title = string.Format(BMessages.Expired_Discount.GetEnumDisplayName(), newItem.Title);
                                newItem.Expired = true;
                            }
                        }
                    }
                    else if (foundDiscountCode.Price > 0)
                        newItem.Price = foundDiscountCode.Price.Value * -1;

                    if (newItem.Price < 0)
                    {
                        newItem.GlobalDiscountId = foundDiscountCode.Id;
                        newQueryItem.GlobalInquiryItems.Add(newItem);
                    }
                }
            }
        }

        private void addCacleDayLimitationDiscount(FireInsuranceInquiryFilledObj filledObj, List<GlobalInquery> result)
        {
            if (filledObj.InquiryDuration != null)
            {
                foreach (var r in result)
                {
                    if (filledObj.InquiryDuration.InquiryDurationCompanies.Any(t => t.CompanyId == r.CompanyId))
                    {
                        if (filledObj.InquiryDuration.Percent > 0 && filledObj.InquiryDuration.Percent < 100)
                        {
                            long price = r.GlobalInquiryItems.Where(t => t.Expired != true).Select(t => t.Price).Sum();
                            decimal rPercent = filledObj.v100 - filledObj.InquiryDuration.Percent;

                            GlobalInquiryItem newItem = new GlobalInquiryItem();
                            newItem.GlobalInquiryId = r.Id;
                            newItem.CalcKey = "dd";
                            newItem.Title = filledObj.InquiryDuration.Title;
                            newItem.Price = RouteThisNumberIfConfigExist(Convert.ToInt64(Math.Ceiling(((Convert.ToDecimal(price) * rPercent) / filledObj.v100) * -1)), filledObj.RoundInquery);

                            r.GlobalInquiryItems.Add(newItem);
                        }
                    }
                    else
                        r.deleteMe = true;
                }
            }
        }

        private void addBasePrice(FireInsuranceInquiryFilledObj filledObj, List<GlobalInquery> result, long globalInputInqueryId, int? siteSettingId)
        {
            if (filledObj.FireInsuranceRates != null && filledObj.FireInsuranceRates.Count > 0 && filledObj.ProposalForm != null && filledObj.sarmaye > 0)
            {
                foreach (var foundRate in filledObj.FireInsuranceRates)
                {
                    if (foundRate != null && foundRate.FireInsuranceRateCompanies != null && foundRate.FireInsuranceRateCompanies.Count > 0)
                    {
                        long basePrice = RouteThisNumberIfConfigExist(Math.Ceiling(Convert.ToDecimal(filledObj.sarmaye) * foundRate.Rate).ToLongReturnZiro(), filledObj.RoundInquery);
                        if (basePrice > 0)
                        {
                            foreach (var com in foundRate.FireInsuranceRateCompanies)
                            {
                                if (filledObj.Companies != null && filledObj.Companies.Count > 0)
                                    if (!filledObj.Companies.Any(tt => tt.Id == com.CompanyId))
                                        continue;

                                GlobalInquery newItem = new GlobalInquery();

                                newItem.GlobalInputInqueryId = globalInputInqueryId;
                                newItem.CompanyId = com.CompanyId;
                                newItem.SiteSettingId = siteSettingId.Value;
                                newItem.ProposalFormId = filledObj.ProposalForm.Id;
                                var basePriceItem = new GlobalInquiryItem()
                                {
                                    CalcKey = "s1",
                                    GlobalInquiryId = newItem.Id,
                                    Price = basePrice,
                                    Title = foundRate.Title
                                };
                                newItem.GlobalInquiryItems.Add(basePriceItem);

                                result.Add(newItem);
                            }
                        }
                    }
                }
            }
        }

        long RouteThisNumberIfConfigExist(long price, RoundInquery foundConfig)
        {
            long result = price;

            if (foundConfig != null && !string.IsNullOrEmpty(foundConfig.Format) && ("1" + foundConfig.Format).ToIntReturnZiro() > 0)
            {
                int mNumbrert = ("1" + foundConfig.Format).ToIntReturnZiro();
                if (foundConfig.Type == RoundInqueryType.Down)
                    result = (Math.Floor(Convert.ToDecimal(result) / Convert.ToDecimal(mNumbrert)) * Convert.ToDecimal(mNumbrert)).ToLongReturnZiro();
                else if (foundConfig.Type == RoundInqueryType.Up)
                    result = (Math.Ceiling(Convert.ToDecimal(result) / Convert.ToDecimal(mNumbrert)) * Convert.ToDecimal(mNumbrert)).ToLongReturnZiro();
                else if (foundConfig.Type == RoundInqueryType.DownUp)
                    result = (Math.Round(Convert.ToDecimal(result) / Convert.ToDecimal(mNumbrert)) * Convert.ToDecimal(mNumbrert)).ToLongReturnZiro();
            }

            return result;
        }

        private FireInsuranceInquiryFilledObj inquiryFilledObjectAndValidation(int? siteSettingId, FireInsuranceInquiryVM input)
        {
            FireInsuranceInquiryFilledObj result = new FireInsuranceInquiryFilledObj();

            inquiryValidation(siteSettingId, input, result);

            validateAndFillValidCompanies(result, siteSettingId, input);
            validateAndFillProposalForm(result, siteSettingId);
            validateAndFilleProvince(input, result);
            validateAndFilleCity(input, result);
            validateAndFIlleBuildingUnit(input, result);
            validateAndFillFireInsuranceBuildingType(input, result);
            validateAndFillFireInsuranceBuildingBody(input, result);
            validateAndFillFireInsuranceBuildingAge(input, result);
            validateAndFillShowStatus(input, result);
            fillRoundInquey(result, siteSettingId);
            validateAndFillTax(result);
            validateAndFillDuty(result);
            validateAndFillInquiryMaxDiscount(result, siteSettingId);
            validateAndFillInquiryDuration(result, siteSettingId, input);
            validateAndFillInsuranceContractDiscount(result, siteSettingId, input);
            validateAndFillGlobalDiscount(result, siteSettingId);
            validateAndFillCashPayDiscount(result, siteSettingId);
            validateAndFillPayemntMethod(result, siteSettingId);
            validateAndCaclulateSarmayeh(result, input);
            validateAndFillFireInsuranceRates(result, input);
            fillInqueryDescription(result, siteSettingId);
            validateAndFillFireInsuranceCoverageTitle(result, input);
            validateAndFillFireInsuranceTypeOfActivities(result, input);
            fillFireInsuranceCoverage(result);
            fillFireInsuranceCoverageCityDangerLevel(result);


            return result;
        }

        private void fillFireInsuranceCoverageCityDangerLevel(FireInsuranceInquiryFilledObj result)
        {
            result.FireInsuranceCoverageCityDangerLevels = FireInsuranceCoverageCityDangerLevelService.GetList(result.City?.FireDangerGroupLevel);
        }

        private void fillFireInsuranceCoverage(FireInsuranceInquiryFilledObj result)
        {
            result.FireInsuranceCoverages = FireInsuranceCoverageService.GetList(result.ProposalForm?.Id, result.Companies.Select(t => t.Id).ToList());
        }

        private void validateAndFillFireInsuranceTypeOfActivities(FireInsuranceInquiryFilledObj result, FireInsuranceInquiryVM input)
        {
            if (result.FireInsuranceCoverageTitles != null && result.FireInsuranceCoverageTitles.Count() > 0 && input.exteraQuestions != null)
            {
                List<int> allTitleIds = new List<int>();
                foreach (var title in result.FireInsuranceCoverageTitles)
                    if (title.FireInsuranceCoverageActivityDangerLevels != null && title.FireInsuranceCoverageActivityDangerLevels.Count > 0)
                    {
                        if (!allTitleIds.Contains(title.Id))
                            allTitleIds.Add(title.Id);
                    }
                var foundAllActivityIds = input.exteraQuestions.Where(t => allTitleIds.Contains(t.id)).Select(t => t.value.ToIntReturnZiro()).Where(t => t > 0).ToList();
                result.FireInsuranceTypeOfActivities = FireInsuranceTypeOfActivityService.GetBy(foundAllActivityIds);
                //if (foundAllActivityIds.Count != result.FireInsuranceTypeOfActivities.Count)
                //    throw BException.GenerateNewException(BMessages.Validation_Error);

            }
        }

        private void validateAndFillFireInsuranceCoverageTitle(FireInsuranceInquiryFilledObj result, FireInsuranceInquiryVM input)
        {
            if (input.exteraQuestions != null && input.exteraQuestions.Count > 0)
            {
                var allIds = input.exteraQuestions.Where(t => t.id > 0).Select(t => t.id).ToList();
                if (allIds.Count != input.exteraQuestions.Count)
                    throw BException.GenerateNewException(BMessages.Validation_Error);
                result.FireInsuranceCoverageTitles = FireInsuranceCoverageTitleService.GetBy(allIds);
                if (result.FireInsuranceCoverageTitles.Count != allIds.Count)
                    throw BException.GenerateNewException(BMessages.Validation_Error);
            }
        }

        private void fillInqueryDescription(FireInsuranceInquiryFilledObj result, int? siteSettingId)
        {
            result.InqueryDescriptions = InqueryDescriptionService.GetBy(result.ProposalForm?.Id, siteSettingId);
        }

        private void fillRoundInquey(FireInsuranceInquiryFilledObj result, int? siteSettingId)
        {
            result.RoundInquery = RoundInqueryService.GetBy(result.ProposalForm?.Id, siteSettingId);
        }

        private void validateAndCaclulateSarmayeh(FireInsuranceInquiryFilledObj result, FireInsuranceInquiryVM input)
        {
            result.arzeshSakhteman = (input.metrazh.ToIntReturnZiro() * input.value.ToLongReturnZiro());
            result.sarmaye = result.arzeshSakhteman + input.assetValue.ToLongReturnZiro();
            if (result.sarmaye <= 0)
                throw BException.GenerateNewException(BMessages.Validation_Error);
        }

        private void validateAndFillFireInsuranceRates(FireInsuranceInquiryFilledObj result, FireInsuranceInquiryVM input)
        {
            result.FireInsuranceRates = GetListBy(result.FireInsuranceBuildingBody?.Id, result.FireInsuranceBuildingType?.Id, result.sarmaye);
        }

        private List<FireInsuranceRate> GetListBy(int? FireInsuranceBuildingBodyId, int? FireInsuranceBuildingTypeId, long sarmaye)
        {
            if (FireInsuranceBuildingBodyId.ToIntReturnZiro() > 0 && FireInsuranceBuildingBodyId.ToIntReturnZiro() > 0 && sarmaye > 0)
                return db.FireInsuranceRates
                    .Include(t => t.FireInsuranceRateCompanies)
                    .Where(t =>
                        t.IsActive == true && t.FireInsuranceBuildingBodyId == FireInsuranceBuildingBodyId &&
                        t.FireInsuranceBuildingTypeId == FireInsuranceBuildingTypeId && t.FromPrice <= sarmaye && t.ToPrice > sarmaye
                        )
                    .AsNoTracking()
                    .ToList();

            return new List<FireInsuranceRate>();
        }

        private void validateAndFillPayemntMethod(FireInsuranceInquiryFilledObj result, int? siteSettingId)
        {
            result.PaymentMethods = PaymentMethodService.GetBy(result.ProposalForm?.Id, siteSettingId);
        }

        private void validateAndFillCashPayDiscount(FireInsuranceInquiryFilledObj result, int? siteSettingId)
        {
            result.CashPayDiscounts = CashPayDiscountService.GetBy(result.ProposalForm?.Id, siteSettingId);
        }

        private void validateAndFillGlobalDiscount(FireInsuranceInquiryFilledObj result, int? siteSettingId)
        {
            result.GlobalDiscounts = GlobalDiscountService.GetAutoDiscountList(result.ProposalForm?.Id, siteSettingId);
        }

        private void validateAndFillInsuranceContractDiscount(FireInsuranceInquiryFilledObj result, int? siteSettingId, FireInsuranceInquiryVM input)
        {
            if (input.discountContractId.ToIntReturnZiro() > 0)
            {
                result.InsuranceContractDiscount = InsuranceContractDiscountService.GetBy(siteSettingId, input.discountContractId, ProposalFormType.FireInsurance);
                if (result.InsuranceContractDiscount == null)
                    throw BException.GenerateNewException(BMessages.Validation_Error);
                input.discountContractId_Title = result.InsuranceContractDiscount.Title;
            }
            else
            {
                input.discountContractId = null;
                input.discountContractId_Title = null;
            }
        }

        private void validateAndFillInquiryDuration(FireInsuranceInquiryFilledObj result, int? siteSettingId, FireInsuranceInquiryVM input)
        {
            if (input.dayLimitation.ToIntReturnZiro() > 0)
            {
                result.InquiryDuration = InquiryDurationService.GetBy(siteSettingId, result.ProposalForm?.Id, input.dayLimitation);
                if (result.InquiryDuration == null)
                    throw BException.GenerateNewException(BMessages.Validation_Error);
                input.dayLimitation_Title = result.InquiryDuration.Title;
            }
            else
            {
                input.dayLimitation = null;
                input.dayLimitation_Title = null;
            }
        }

        private void validateAndFillValidCompanies(FireInsuranceInquiryFilledObj result, int? siteSettingId, FireInsuranceInquiryVM input)
        {
            result.Companies = InquiryCompanyLimitService.GetCompanies(siteSettingId, InquiryCompanyLimitType.Fire);
            if (result.Companies != null && input.comIds != null && input.comIds.Count > 0)
                result.Companies = result.Companies.Where(t => input.comIds.Contains(t.Id)).ToList();
            if (result.Companies.Count == 0)
                throw BException.GenerateNewException(BMessages.Validation_Error);
        }

        private void validateAndFillInquiryMaxDiscount(FireInsuranceInquiryFilledObj result, int? siteSettingId)
        {
            result.InquiryMaxDiscounts = InquiryMaxDiscountService.GetByFormId(result?.ProposalForm?.Id, siteSettingId);
        }

        private void validateAndFillProposalForm(FireInsuranceInquiryFilledObj result, int? siteSettingId)
        {
            result.ProposalForm = ProposalFormService.GetByType(ProposalFormType.FireInsurance, siteSettingId);
            if (result.ProposalForm == null)
                throw BException.GenerateNewException(BMessages.Validation_Error);
        }

        private void validateAndFillDuty(FireInsuranceInquiryFilledObj result)
        {
            result.Duty = DutyService.GetLastActiveItem();
        }

        private void validateAndFillTax(FireInsuranceInquiryFilledObj result)
        {
            result.Tax = TaxService.GetLastActiveItem();
        }

        private void validateAndFillShowStatus(FireInsuranceInquiryVM input, FireInsuranceInquiryFilledObj result)
        {
            if (input.showStatus.ToIntReturnZiro() > 0)
            {
                var allValidStatus = EnumService.GetEnum("PriceDiscountStatus");
                if (!allValidStatus.Any(t => t.id == input.showStatus + ""))
                    throw BException.GenerateNewException(BMessages.Payment_Method_Is_Not_Valid);
                input.showStatus_Title = allValidStatus.Where(t => t.id == input.showStatus + "").Select(t => t.title).FirstOrDefault();
            }
            else
                throw BException.GenerateNewException(BMessages.Validation_Error);
        }

        private void validateAndFillFireInsuranceBuildingAge(FireInsuranceInquiryVM input, FireInsuranceInquiryFilledObj result)
        {
            if (input.buildingAge.ToIntReturnZiro() > 0)
            {
                result.FireInsuranceBuildingAge = FireInsuranceBuildingAgeService.GetById(input.buildingAge);
                if (result.FireInsuranceBuildingAge == null)
                    throw BException.GenerateNewException(BMessages.Validation_Error);
                input.buildingAge_Title = result.FireInsuranceBuildingAge.Title;
            }
            else
                throw BException.GenerateNewException(BMessages.Validation_Error);
        }

        private void validateAndFillFireInsuranceBuildingBody(FireInsuranceInquiryVM input, FireInsuranceInquiryFilledObj result)
        {
            if (input.bodyId.ToIntReturnZiro() > 0)
            {
                result.FireInsuranceBuildingBody = FireInsuranceBuildingBodyService.GetById(input.bodyId);
                if (result.FireInsuranceBuildingBody == null)
                    throw BException.GenerateNewException(BMessages.Validation_Error);
                input.bodyId_Title = result.FireInsuranceBuildingBody.Title;
            }
            else
                throw BException.GenerateNewException(BMessages.Validation_Error);
        }

        private void validateAndFillFireInsuranceBuildingType(FireInsuranceInquiryVM input, FireInsuranceInquiryFilledObj result)
        {
            if (input.typeId.ToIntReturnZiro() > 0)
            {
                result.FireInsuranceBuildingType = FireInsuranceBuildingTypeService.GetById(input.typeId);
                if (result.FireInsuranceBuildingType == null)
                    throw BException.GenerateNewException(BMessages.Validation_Error);
                input.typeId_Title = result.FireInsuranceBuildingType.Title;
            }
            else
                throw BException.GenerateNewException(BMessages.Validation_Error);
        }

        private void validateAndFIlleBuildingUnit(FireInsuranceInquiryVM input, FireInsuranceInquiryFilledObj result)
        {
            if (input.value.ToIntReturnZiro() > 0)
            {
                result.FireInsuranceBuildingUnitValue = FireInsuranceBuildingUnitValueService.GetById(input.value);
                if (result.FireInsuranceBuildingUnitValue == null)
                    throw BException.GenerateNewException(BMessages.Validation_Error);
                input.value_Title = result.FireInsuranceBuildingUnitValue.Title;
            }
            else
                throw BException.GenerateNewException(BMessages.Validation_Error);
        }

        private void validateAndFilleCity(FireInsuranceInquiryVM input, FireInsuranceInquiryFilledObj result)
        {
            if (input.cityId.ToIntReturnZiro() > 0)
            {
                result.City = CityService.GetById(input.cityId);
                if (result.City == null)
                    throw BException.GenerateNewException(BMessages.Please_Select_City);
                input.cityId_Title = result.City.Title;
            }
            else
                throw BException.GenerateNewException(BMessages.Validation_Error);
        }

        private void validateAndFilleProvince(FireInsuranceInquiryVM input, FireInsuranceInquiryFilledObj result)
        {
            if (input.provinceId.ToIntReturnZiro() > 0)
            {
                result.Province = ProvinceService.GetProvinceById(input.provinceId);
                if (result.Province == null)
                    throw BException.GenerateNewException(BMessages.Please_Select_Province);
                input.provinceId_Title = result.Province.Title;
            }
            else
                throw BException.GenerateNewException(BMessages.Validation_Error);
        }

        private bool inquiryValidation(int? siteSettingId, FireInsuranceInquiryVM input, FireInsuranceInquiryFilledObj filledObj)
        {
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (input.provinceId.ToIntReturnZiro() <= 0)
                return false;
            if (input.cityId.ToIntReturnZiro() <= 0)
                return false;
            if (input.value.ToIntReturnZiro() <= 0)
                return false;
            if (input.metrazh.ToIntReturnZiro() <= 0)
                return false;
            if (input.typeId.ToIntReturnZiro() <= 0)
                return false;
            if (input.bodyId.ToIntReturnZiro() <= 0)
                return false;
            if (input.buildingAge.ToIntReturnZiro() <= 0)
                return false;
            //if (filledObj.Province != null && filledObj.City != null && filledObj.Province.Id != filledObj.City.ProvinceId)
            // return false;
            if (input.assetValue != null && input.assetValue < 0)
                return false;

            return true;
        }
    }
}
