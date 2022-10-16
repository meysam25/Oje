using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using Oje.ProposalFormService.Interfaces;
using Oje.ProposalFormService.Models.DB;
using Oje.ProposalFormService.Models.View;
using Oje.ProposalFormService.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Oje.ProposalFormService.Services
{
    public class CarSpecificationAmountService : ICarSpecificationAmountService
    {
        readonly ProposalFormDBContext db = null;
        readonly IGlobalInqueryService GlobalInqueryService = null;
        readonly IGlobalInputInqueryService GlobalInputInqueryService = null;
        readonly IGlobalDiscountService GlobalDiscountService = null;
        readonly IProposalFormService ProposalFormService = null;
        readonly IInquiryCompanyLimitService InquiryCompanyLimitService = null;
        readonly ICompanyService CompanyService = null;
        readonly IVehicleTypeService VehicleTypeService = null;
        readonly IVehicleSystemService VehicleSystemService = null;
        readonly IVehicleUsageService VehicleUsageService = null;
        readonly ICarExteraDiscountService CarExteraDiscountService = null;
        readonly INoDamageDiscountService NoDamageDiscountService = null;
        readonly ICashPayDiscountService CashPayDiscountService = null;
        readonly IInsuranceContractDiscountService InsuranceContractDiscountService = null;
        readonly IInquiryDurationService InquiryDurationService = null;
        readonly IRoundInqueryService RoundInqueryService = null;
        readonly ICarSpecificationService CarSpecificationService = null;
        readonly IInqueryDescriptionService InqueryDescriptionService = null;
        readonly ITaxService TaxService = null;
        readonly IDutyService DutyService = null;
        readonly IInquiryMaxDiscountService InquiryMaxDiscountService = null;
        readonly IPaymentMethodService PaymentMethodService = null;
        readonly ICarBodyCreateDatePercentService CarBodyCreateDatePercentService = null;
        readonly ICarTypeService CarTypeService = null;
        readonly IVehicleSpecsService VehicleSpecsService = null;

        public CarSpecificationAmountService(
            ProposalFormDBContext db,
            IGlobalInqueryService GlobalInqueryService,
            IGlobalInputInqueryService GlobalInputInqueryService,
            IGlobalDiscountService GlobalDiscountService,
            IProposalFormService ProposalFormService,
            IInquiryCompanyLimitService InquiryCompanyLimitService,
            ICompanyService CompanyService,
            IVehicleTypeService VehicleTypeService,
            IVehicleSystemService VehicleSystemService,
            IVehicleUsageService VehicleUsageService,
            ICarExteraDiscountService CarExteraDiscountService,
            INoDamageDiscountService NoDamageDiscountService,
            ICashPayDiscountService CashPayDiscountService,
            IInsuranceContractDiscountService InsuranceContractDiscountService,
            IInquiryDurationService InquiryDurationService,
            IRoundInqueryService RoundInqueryService,
            ICarSpecificationService CarSpecificationService,
            IInqueryDescriptionService InqueryDescriptionService,
            ITaxService TaxService,
            IDutyService DutyService,
            IInquiryMaxDiscountService InquiryMaxDiscountService,
            IPaymentMethodService PaymentMethodService,
            ICarBodyCreateDatePercentService CarBodyCreateDatePercentService,
            ICarTypeService CarTypeService,
            IVehicleSpecsService VehicleSpecsService
            )
        {
            this.db = db;
            this.GlobalInqueryService = GlobalInqueryService;
            this.GlobalInputInqueryService = GlobalInputInqueryService;
            this.GlobalDiscountService = GlobalDiscountService;
            this.ProposalFormService = ProposalFormService;
            this.InquiryCompanyLimitService = InquiryCompanyLimitService;
            this.CompanyService = CompanyService;
            this.VehicleTypeService = VehicleTypeService;
            this.VehicleSystemService = VehicleSystemService;
            this.VehicleUsageService = VehicleUsageService;
            this.CarExteraDiscountService = CarExteraDiscountService;
            this.NoDamageDiscountService = NoDamageDiscountService;
            this.CashPayDiscountService = CashPayDiscountService;
            this.InsuranceContractDiscountService = InsuranceContractDiscountService;
            this.InquiryDurationService = InquiryDurationService;
            this.RoundInqueryService = RoundInqueryService;
            this.CarSpecificationService = CarSpecificationService;
            this.InqueryDescriptionService = InqueryDescriptionService;
            this.TaxService = TaxService;
            this.DutyService = DutyService;
            this.InquiryMaxDiscountService = InquiryMaxDiscountService;
            this.PaymentMethodService = PaymentMethodService;
            this.CarBodyCreateDatePercentService = CarBodyCreateDatePercentService;
            this.CarTypeService = CarTypeService;
            this.VehicleSpecsService = VehicleSpecsService;
        }

        public object Inquiry(int? siteSettingId, CarBodyInquiryVM input, string targetArea)
        {
            List<GlobalInquery> result = new List<GlobalInquery>();

            if (IsValidInquiry(input) == true && siteSettingId.ToIntReturnZiro() > 0)
            {
                CarBodyInquiryObjects objPack = fillRequiredObjectsAndValidate(input, siteSettingId);

                long GlobalInputInqueryId = GlobalInputInqueryService.Create(input, null, siteSettingId);
                if (GlobalInputInqueryId > 0 && objPack.CarSpecificationAmounts != null && objPack.CarSpecificationAmounts.Count > 0)
                {
                    CreateGlobalInqueryObjectForEachCompanyS1(objPack, siteSettingId, GlobalInputInqueryId, result, input);

                    foreach (var newQueryItem in result)
                    {
                        InquiryCalceCreateDatePercen(objPack, newQueryItem, input);
                        InquiryCalceVehicleUsage(objPack, newQueryItem, input);
                        addDynamicControlCalce(objPack, newQueryItem, input);
                        addCacleForNoDamageDiscount(objPack, newQueryItem);
                        addCacleRightOptionFilter(objPack, newQueryItem, input);
                        addCacleForNoDamageDiscountExtera(objPack, newQueryItem);
                        addCacleDiscountContract(objPack, newQueryItem);
                        addCacleGlobalDiscountAuto(objPack, newQueryItem);
                        addCacleGlobalDiscount(objPack, newQueryItem, siteSettingId, input.discountCode);
                        addCacleDayLimitationDiscount(objPack, newQueryItem);
                    }

                    addCacleAddCashDiscount(result, objPack);
                    InquiryCalceTaxAndDuty(result, objPack);
                }
                GlobalInqueryService.Create(result);

                return calceResponeForInquiry(result, objPack, input, targetArea);
            }

            return new { total = 0, data = new List<object>() };
        }

        void addCacleForNoDamageDiscount(CarBodyInquiryObjects objPack, GlobalInquery newQueryItem)
        {
            if (objPack.NoDamageDiscount != null && objPack.NoDamageDiscount.Percent > 0)
            {
                if (objPack.NoDamageDiscount.NoDamageDiscountCompanies.Any(tt => tt.CompanyId == newQueryItem.CompanyId))
                {
                    GlobalInquiryItem newItem = new GlobalInquiryItem();
                    newItem.GlobalInquiryId = newQueryItem.Id;
                    newItem.Title = objPack.NoDamageDiscount.Title;
                    newItem.CalcKey = "nodD";
                    var sumItemPrice = newQueryItem.GlobalInquiryItems.Where(t => t.Expired != true && (t.CalcKey == "s1" || t.CalcKey == "crdOl" || t.CalcKey == "vUsag")).Select(t => t.Price).Sum();
                    if (objPack.NoDamageDiscount.Percent > 0)
                    {
                        newItem.Price = RouteThisNumberIfConfigExist(Convert.ToInt64(Math.Ceiling((Convert.ToDecimal((sumItemPrice)) * objPack.NoDamageDiscount.Percent) / objPack.v100)) * -1, objPack.RoundInquery);
                        newQueryItem.maxDiscount += objPack.NoDamageDiscount.Percent;
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

        void addCacleForNoDamageDiscountExtera(CarBodyInquiryObjects objPack, GlobalInquery newQueryItem)
        {
            if (objPack.NoDamageDiscountExtera != null && objPack.NoDamageDiscountExtera.Percent > 0)
            {
                if (objPack.NoDamageDiscountExtera.NoDamageDiscountCompanies.Any(tt => tt.CompanyId == newQueryItem.CompanyId))
                {
                    GlobalInquiryItem newItem = new GlobalInquiryItem();
                    newItem.GlobalInquiryId = newQueryItem.Id;
                    newItem.Title = objPack.NoDamageDiscountExtera.Title + "(عدم خسارت اضافی)";
                    newItem.CalcKey = "nodDE";
                    var sumItemPrice = newQueryItem.GlobalInquiryItems.Where(t => t.Expired != true && t.CalcKey == "DRO" && t.Price > 0 && t.curCalculateType != CarExteraDiscountCalculateType.OnBaseAndCreateYearAndNoDamageDiscount).Select(t => t.Price).Sum();
                    if (objPack.NoDamageDiscountExtera.Percent > 0)
                    {
                        newItem.Price = RouteThisNumberIfConfigExist(Convert.ToInt64(Math.Ceiling((Convert.ToDecimal((sumItemPrice)) * objPack.NoDamageDiscountExtera.Percent) / objPack.v100)) * -1, objPack.RoundInquery);
                        newQueryItem.maxDiscount += objPack.NoDamageDiscountExtera.Percent;
                        if (objPack.InquiryMaxDiscounts.Any(t => t.Percent < newQueryItem.maxDiscount && t.InquiryMaxDiscountCompanies.Any(tt => tt.CompanyId == newQueryItem.CompanyId)))
                        {
                            newItem.Title = string.Format(BMessages.Expired_Discount.GetEnumDisplayName(), newItem.Title);
                            newItem.Expired = true;
                        }
                        if (newItem.Price < 0)
                            newQueryItem.GlobalInquiryItems.Add(newItem);
                    }
                }
                else
                    newQueryItem.deleteMe = true;
            }
        }

        CarBodyInquiryObjects fillRequiredObjectsAndValidate(CarBodyInquiryVM input, int? siteSettingId)
        {
            CarBodyInquiryObjects result = new CarBodyInquiryObjects();

            if (input.exteraQuestions != null)
                input.exteraQuestions = input.exteraQuestions.Where(t => t.id.ToIntReturnZiro() > 0 && !string.IsNullOrEmpty(t.value)).ToList();

            fillAndValidateProposalForm(result, siteSettingId);
            fillAndValidateValidCompany(result, input, siteSettingId);
            fillAndValidatePrevInsuranceAndIsNewCar(result, input);
            fillAndValidateVehicleType(result, input);
            fillAndValidateVehicleSystem(input);
            fillAndValidateVehicleUsage(result, input);
            fillAndValidateCarExteraDiscountRequired(result, input);
            fillAndValidateNoDamageDiscount(result, input);
            fillAndValidatePriceDiscountStatus(result, input, siteSettingId);
            fillAndValidateExteraDiscountRightFilterObject(result, input);
            fillAndValidateInsuranceContractDiscount(result, input, siteSettingId);
            fillAndValidateInquiryDuration(result, input, siteSettingId);
            fillRoundInquery(result, siteSettingId);
            fillCarSpecification(input, result);
            fillInqueryDescriptions(result, siteSettingId);
            fillTaxAndDuty(result);
            fillInquiryMaxDiscounts(result, siteSettingId);
            fillGlobalDiscounts(result, siteSettingId);
            fillCashPayDiscounts(result, siteSettingId);
            filPaymentMethods(result, siteSettingId);
            fillCarSpecificationAmount(result, input);
            fillAndValidateCarBodyCreateDatePercent(result, input);

            return result;
        }

        private void fillCarSpecificationAmount(CarBodyInquiryObjects result, CarBodyInquiryVM input)
        {
            if (input.specId.ToIntReturnZiro() > 0)
            {
                result.VehicleSpec = VehicleSpecsService.GetBy(input.specId, input.brandId, input.vehicleTypeId);
                if (result.VehicleSpec == null)
                    throw BException.GenerateNewException(BMessages.Validation_Error);
                input.specId_Title = result.VehicleSpec.Title;
                result.CarSpecificationAmounts = GetBy(input.specId);
            }
        }

        private List<CarSpecificationAmount> GetBy(int? specId)
        {
            return db.CarSpecificationAmounts
                .Where(t => t.IsActive == true && t.CarSpecification.CarSpecificationVehicleSpecs.Any(tt => tt.VehicleSpecId == specId))
                .Include(t => t.CarSpecificationAmountCompanies)
                .AsNoTracking()
                .ToList();
        }

        void InquiryCalceCreateDatePercen(CarBodyInquiryObjects objPack, GlobalInquery newQueryItem, CarBodyInquiryVM input)
        {
            var foundS2 = newQueryItem.GlobalInquiryItems.Where(t => t.CalcKey == "s1").FirstOrDefault();
            if (foundS2 != null && objPack.CarBodyCreateDatePercents != null && objPack.CarBodyCreateDatePercents.Count > 0)
            {
                var defYear = DateTime.Now.Year - input.createYear.ToIntReturnZiro();
                if (defYear > 0)
                {
                    var foundTargetRate = objPack.CarBodyCreateDatePercents.Where(t => t.FromYear <= defYear && t.ToYear > defYear).FirstOrDefault();
                    if (foundTargetRate != null)
                    {
                        GlobalInquiryItem newITem = new GlobalInquiryItem()
                        {
                            CalcKey = "crdOl",
                            GlobalInquiryId = newQueryItem.Id,
                            Title = BMessages.CreateDate_Price.GetEnumDisplayName(),
                            Price = RouteThisNumberIfConfigExist(Math.Ceiling((foundTargetRate.Percent * Convert.ToDecimal(foundS2.Price)) / objPack.v100).ToLongReturnZiro(), objPack.RoundInquery)
                        };
                        if (newITem.Price > 0)
                            newQueryItem.GlobalInquiryItems.Add(newITem);
                    }
                }
            }
        }

        void InquiryCalceVehicleUsage(CarBodyInquiryObjects objPack, GlobalInquery newQueryItem, CarBodyInquiryVM input)
        {
            var foundS2 = newQueryItem.GlobalInquiryItems.Where(t => t.CalcKey == "s1").FirstOrDefault();
            var foundCreateDatePrice = newQueryItem.GlobalInquiryItems.Where(t => t.CalcKey == "crdOl").FirstOrDefault();
            if (foundCreateDatePrice == null)
                foundCreateDatePrice = new();
            if (foundS2 != null && objPack.VehicleUsage != null && objPack.VehicleUsage.BodyPercent != null && objPack.VehicleUsage.BodyPercent > 0)
            {
                GlobalInquiryItem newITem = new GlobalInquiryItem()
                {
                    CalcKey = "vUsag",
                    GlobalInquiryId = newQueryItem.Id,
                    Title = BMessages.CarUsage_BasePrice.GetEnumDisplayName(),
                    Price = RouteThisNumberIfConfigExist(Math.Ceiling((objPack.VehicleUsage.BodyPercent.Value * Convert.ToDecimal(foundS2.Price + foundCreateDatePrice.Price)) / objPack.v100).ToLongReturnZiro(), objPack.RoundInquery)
                };
                if (newITem.Price > 0)
                    newQueryItem.GlobalInquiryItems.Add(newITem);
            }
        }

        private void fillAndValidateCarBodyCreateDatePercent(CarBodyInquiryObjects result, CarBodyInquiryVM input)
        {
            if (input.vehicleTypeId.ToLongReturnZiro() > 0)
                result.CarBodyCreateDatePercents = CarBodyCreateDatePercentService.GetByList(input.vehicleTypeId);
        }

        private void filPaymentMethods(CarBodyInquiryObjects result, int? siteSettingId)
        {
            result.PaymentMethods = PaymentMethodService.GetList(siteSettingId, result.currentProposalForm?.Id);
        }

        private void fillCashPayDiscounts(CarBodyInquiryObjects result, int? siteSettingId)
        {
            result.CashPayDiscounts = CashPayDiscountService.GetBySiteSettingAndProposalFormId(siteSettingId, result.currentProposalForm?.Id);
        }

        private void fillGlobalDiscounts(CarBodyInquiryObjects result, int? siteSettingId)
        {
            result.GlobalDiscounts = GlobalDiscountService.GetAutoDiscounts(result.currentProposalForm?.Id, siteSettingId);
        }

        private void fillInquiryMaxDiscounts(CarBodyInquiryObjects result, int? siteSettingId)
        {
            result.InquiryMaxDiscounts = InquiryMaxDiscountService.GetByFormAndSiteSettingId(result.currentProposalForm?.Id, siteSettingId);
        }

        private void fillTaxAndDuty(CarBodyInquiryObjects result)
        {
            result.Tax = TaxService.GetLastItem();
            result.Duty = DutyService.GetLastItem();
        }

        private void fillInqueryDescriptions(CarBodyInquiryObjects result, int? siteSettingId)
        {
            result.InqueryDescriptions = InqueryDescriptionService.GetByProposalFormId(result.currentProposalForm?.Id, siteSettingId);
        }

        private void fillCarSpecification(CarBodyInquiryVM input, CarBodyInquiryObjects result)
        {
            result.CarSpecification = CarSpecificationService.GetById(input.specId);
        }

        private void fillRoundInquery(CarBodyInquiryObjects result, int? siteSettingId)
        {
            result.RoundInquery = RoundInqueryService.GetBySiteSettingAndProposalForm(siteSettingId, result.currentProposalForm?.Id);
        }

        private void fillAndValidateInquiryDuration(CarBodyInquiryObjects result, CarBodyInquiryVM input, int? siteSettingId)
        {
            if (input.dayLimitation.ToIntReturnZiro() > 0)
            {
                result.InquiryDuration = InquiryDurationService.GetById(siteSettingId, result?.currentProposalForm?.Id, input.dayLimitation);

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

        private void fillAndValidateInsuranceContractDiscount(CarBodyInquiryObjects result, CarBodyInquiryVM input, int? siteSettingId)
        {
            if (input.discountContractId.ToIntReturnZiro() > 0)
            {
                result.InsuranceContractDiscount = InsuranceContractDiscountService.GetById(siteSettingId, result.currentProposalForm?.Id, input.discountContractId);
                if (result.InsuranceContractDiscount == null)
                    throw BException.GenerateNewException(BMessages.Insurance_Conteract_Not_Found);
                input.discountContractId_Title = result.InsuranceContractDiscount.Title;
            }
            else
            {
                input.discountContractId = null;
                input.discountContractId_Title = null;
            }
        }

        void fillAndValidateExteraDiscountRightFilterObject(CarBodyInquiryObjects result, CarBodyInquiryVM input)
        {
            if (input.exteraQuestions != null && input.exteraQuestions.Count > 0)
            {
                var allExtraDiscountCoverIDs = input.exteraQuestions.Select(t => t.id).ToList();
                result.rightOptionDynamicFilterCTRLs = CarExteraDiscountService.GetOptionSelectedCtrls(allExtraDiscountCoverIDs, result.currentProposalForm?.Id);

                if (result.rightOptionDynamicFilterCTRLs == null || result.rightOptionDynamicFilterCTRLs.Count == 0)
                    throw BException.GenerateNewException(BMessages.Validation_Error);
                if (result.rightOptionDynamicFilterCTRLs.Count != input.exteraQuestions.Count)
                    throw BException.GenerateNewException(BMessages.Validation_Error);
            }
            else
                input.exteraQuestions = null;
        }

        private void fillAndValidatePriceDiscountStatus(CarBodyInquiryObjects result, CarBodyInquiryVM input, int? siteSettingId)
        {
            if (input.showStatus.ToIntReturnZiro() > 0)
            {
                var allValidStatus = EnumService.GetEnum("PriceDiscountStatus");
                if (!allValidStatus.Any(t => t.id == input.showStatus + ""))
                    throw BException.GenerateNewException(BMessages.Payment_Method_Is_Not_Valid);
                input.showStatus_Title = allValidStatus.Where(t => t.id == input.showStatus + "").Select(t => t.title).FirstOrDefault();
            }
            else
            {
                input.showStatus_Title = null;
                input.showStatus = null;
                if (CashPayDiscountService.HasAnyDebitPayment(siteSettingId, result.currentProposalForm?.Id))
                    throw BException.GenerateNewException(BMessages.Please_Select_Payment_Method);
            }
        }

        private void fillAndValidateNoDamageDiscount(CarBodyInquiryObjects result, CarBodyInquiryVM input)
        {
            if (input.noDamageDiscountBody.ToIntReturnZiro() > 0)
            {
                result.NoDamageDiscount = NoDamageDiscountService.GetByFormId(result.currentProposalForm?.Id, input.noDamageDiscountBody);
                if (result.NoDamageDiscount == null)
                    throw BException.GenerateNewException(BMessages.Please_Enter_NoDamage_Discount_Percent);
                input.noDamageDiscountBody_Title = result.NoDamageDiscount.Title;
            }
            else
            {
                input.noDamageDiscountBody_Title = null;
                input.noDamageDiscountBody = null;
            }

            if (input.noDamageDiscountBodyExtera.ToIntReturnZiro() > 0)
            {
                result.NoDamageDiscountExtera = NoDamageDiscountService.GetByFormId(result.currentProposalForm?.Id, input.noDamageDiscountBodyExtera);
                if (result.NoDamageDiscountExtera == null)
                    throw BException.GenerateNewException(BMessages.Please_Enter_NoDamage_Discount_Percent);
                input.noDamageDiscountBodyExtera_Title = result.NoDamageDiscount.Title;
            }
            else
            {
                input.noDamageDiscountBodyExtera_Title = null;
                input.noDamageDiscountBodyExtera = null;
            }
        }

        private void fillAndValidateCarExteraDiscountRequired(CarBodyInquiryObjects result, CarBodyInquiryVM input)
        {
            result.requiredValidDynamicCTRLs = CarExteraDiscountService.GetRequredValidCTRLs(result?.currentProposalForm?.Id, input.brandId.ToIntReturnZiro(), input.havePrevInsurance > 0 ? true : false);
            if (input.dynamicCTRLs != null && input.dynamicCTRLs.Count > 0)
            {
                result.requiredSelectedDynamicCTRLs = result.requiredValidDynamicCTRLs.Where(t => t.CarExteraDiscountValues.Any(tt => input.dynamicCTRLs.Contains(tt.Id))).ToList();
                if (result.requiredSelectedDynamicCTRLs == null || result.requiredSelectedDynamicCTRLs.Count == 0)
                    throw BException.GenerateNewException(BMessages.Validation_Error);
                if (result.requiredSelectedDynamicCTRLs.SelectMany(t => t.CarExteraDiscountValues).Where(tt => input.dynamicCTRLs.Contains(tt.Id)).Count() != input.dynamicCTRLs.Count)
                    throw BException.GenerateNewException(BMessages.Validation_Error);
                input.dynamicCTRLs_Title = new List<string>();
                foreach (var dc in result.requiredSelectedDynamicCTRLs.SelectMany(t => t.CarExteraDiscountValues).Where(tt => input.dynamicCTRLs.Contains(tt.Id)).ToList())
                    input.dynamicCTRLs_Title.Add(dc.Title);
            }
            else
            {
                if (result.requiredValidDynamicCTRLs.Count > 0)
                    throw BException.GenerateNewException(BMessages.Validation_Error);
                input.dynamicCTRLs = null;
                input.dynamicCTRLs_Title = null;
            }
        }

        private void fillAndValidateVehicleUsage(CarBodyInquiryObjects result, CarBodyInquiryVM input)
        {
            result.CarType = CarTypeService.GetById(input.carTypeId, input.vehicleTypeId);
            if (result.CarType == null)
                throw BException.GenerateNewException(BMessages.Please_Select_CarType);
            input.carTypeId_Title = result.CarType.Title;

            result.VehicleUsage = VehicleUsageService.GetById(input.carTypeId);
            //if (result.VehicleUsage == null)
            //    throw BException.GenerateNewException(BMessages.Invalid_VehicleUsage);
        }

        void fillAndValidateVehicleSystem(CarBodyInquiryVM input)
        {
            string brandTitle = VehicleSystemService.GetTitleById(input.brandId);
            if (string.IsNullOrEmpty(brandTitle))
                throw BException.GenerateNewException(BMessages.Please_Select_VehicleBrand);
            input.brandId_Title = brandTitle;
        }

        void fillAndValidateVehicleType(CarBodyInquiryObjects result, CarBodyInquiryVM input)
        {
            result.VehicleType = VehicleTypeService.GetById(input.vehicleTypeId);
            if (result.VehicleType == null)
                throw BException.GenerateNewException(BMessages.Validation_Error);
            input.vehicleTypeId_Title = result.VehicleType.Title;
        }

        void fillAndValidatePrevInsuranceAndIsNewCar(CarBodyInquiryObjects result, CarBodyInquiryVM input)
        {
            if (input.havePrevInsurance.ToIntReturnZiro() < 0)
                throw BException.GenerateNewException(BMessages.Selected_Company_Is_Not_Valid);
            else if (input.havePrevInsurance.ToIntReturnZiro() > 0)
                initIfHavePrevInsurance(result, input);
            else
                cleanInputRelatedToDontHavePrevInsurance(input);

            if (input.isNewCar == 0 && input.createYear.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Enter_Car_CreateDate);

            if (input.isNewCar == 0)
                input.isNewCar_Title = "خیر";
            else
                input.isNewCar_Title = "بله";
        }

        void cleanInputRelatedToDontHavePrevInsurance(CarBodyInquiryVM input)
        {
            input.noDamageDiscountBody = null;
            input.noDamageDiscountBody_Title = null;
            input.havePrevInsurance_Title = "ندارم";
            input.noDamageDiscountBodyExtera = null;
            input.noDamageDiscountBodyExtera_Title = null;
        }

        void initIfHavePrevInsurance(CarBodyInquiryObjects result, CarBodyInquiryVM input)
        {
            result.prevInsuranceCompany = CompanyService.GetById(input.havePrevInsurance);
            if (result.prevInsuranceCompany == null)
                throw BException.GenerateNewException(BMessages.Selected_Company_Is_Not_Valid);
            input.havePrevInsurance_Title = result.prevInsuranceCompany.Title;
            input.isNewCar = 0;

            if (input.noDamageDiscountBody.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Enter_NoDamage_Discount_Percent);
            if (input.noDamageDiscountBodyExtera.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Enter_NoDamage_Discount_Percent);
        }

        void fillAndValidateValidCompany(CarBodyInquiryObjects result, CarBodyInquiryVM input, int? siteSettingId)
        {
            result.validCompanies = InquiryCompanyLimitService.GetCompanies(siteSettingId, InquiryCompanyLimitType.CarBody);
            if (result.validCompanies != null && result.validCompanies.Count > 0 && input.comIds != null && input.comIds.Count > 0)
                result.validCompanies = result.validCompanies.Where(t => input.comIds.Contains(t.Id)).ToList();
            if (result.validCompanies.Count == 0)
                throw BException.GenerateNewException(BMessages.Validation_Error);
        }

        void fillAndValidateProposalForm(CarBodyInquiryObjects result, int? siteSettingId)
        {
            result.currentProposalForm = ProposalFormService.GetByType(ProposalFormType.CarBody, siteSettingId);
            if (result.currentProposalForm == null)
                throw BException.GenerateNewException(BMessages.ProposalForm_Not_Founded);
        }

        void addCacleRightOptionFilter(CarBodyInquiryObjects objPack, GlobalInquery newQueryItem, CarBodyInquiryVM input)
        {
            if (input.exteraQuestions != null && input.exteraQuestions.Count > 0)
            {
                foreach (var dItemValue in objPack.rightOptionDynamicFilterCTRLs)
                {
                    var selectValueId = input.exteraQuestions.Where(tt => tt.id == dItemValue.Id).Select(tt => tt.value).FirstOrDefault();
                    List<CarExteraDiscountRangeAmount> CarExteraDiscountRangeAmounts =
                         selectValueId.ToIntReturnZiro() > 0 ?
                            dItemValue.CarExteraDiscountValues.Where(tt => tt.IsActive == true && tt.Id == selectValueId.ToIntReturnZiro()).SelectMany(tt => tt.CarExteraDiscountRangeAmounts).ToList() :
                            (!string.IsNullOrEmpty(selectValueId) && selectValueId.ToLower() == "true") ? dItemValue.CarExteraDiscountRangeAmounts.ToList() : new List<CarExteraDiscountRangeAmount>();

                    if (dItemValue.IsActive == true &&
                        CarExteraDiscountRangeAmounts.Any(t => t.IsActive == true && t.CarExteraDiscountRangeAmountCompanies.Any(tt => tt.CompanyId == newQueryItem.CompanyId)))
                    {
                        var allRangeAmount = CarExteraDiscountRangeAmounts.Where(t => t.IsActive == true && t.CarExteraDiscountRangeAmountCompanies.Any(tt => tt.CompanyId == newQueryItem.CompanyId)).ToList();
                        if (allRangeAmount.Count > 0)
                        {
                            GlobalInquiryItem newItem = new GlobalInquiryItem();
                            newItem.GlobalInquiryId = newQueryItem.Id;
                            newItem.Title = dItemValue.Title + (selectValueId.ToIntReturnZiro() > 0 ? dItemValue.CarExteraDiscountValues.Where(tt => tt.IsActive == true && tt.Id == selectValueId.ToIntReturnZiro()).Select(tt => "/" + tt.Title).FirstOrDefault() : "");
                            newItem.CalcKey = "DRO";

                            var s1Price = newQueryItem.GlobalInquiryItems.Where(t => t.CalcKey == "s1").Select(t => t.Price).FirstOrDefault();
                            var crdOlPrice = newQueryItem.GlobalInquiryItems.Where(t => t.CalcKey == "crdOl").Select(t => t.Price).FirstOrDefault();
                            var noDDPrice = newQueryItem.GlobalInquiryItems.Where(t => t.CalcKey == "nodD").Select(t => t.Price).FirstOrDefault();
                            var vUsagPrice = newQueryItem.GlobalInquiryItems.Where(t => t.CalcKey == "vUsag").Select(t => t.Price).FirstOrDefault();
                            var sumItemPrice = newQueryItem.GlobalInquiryItems.Where(t => t.Expired != true).Select(t => t.Price).Sum();

                            newItem.curCalculateType = dItemValue.CalculateType;
                            newItem.Price = RouteThisNumberIfConfigExist(
                                cacleInsuranceDynamicCTRL
                                (
                                    s1Price, sumItemPrice, dItemValue.Type, input.createYear, dItemValue.CalculateType, allRangeAmount,
                                    crdOlPrice, input.carValue.ToLongReturnZiro() + input.exteraValue.ToLongReturnZiro(), input.exteraValue.ToLongReturnZiro(),
                                    noDDPrice,
                                    (objPack.CarBodyCreateDatePercents != null ? objPack.CarBodyCreateDatePercents.OrderBy(t => t.FromYear).Select(t => t.FromYear).FirstOrDefault() : 0),
                                    vUsagPrice, objPack.VehicleUsage?.BodyPercent
                                ), objPack.RoundInquery);
                            if (newItem.Price != 0)
                                newQueryItem.GlobalInquiryItems.Add(newItem);
                        }
                    }
                    else
                    {
                        if (dItemValue?.DontRemoveInSearch != true)
                            newQueryItem.deleteMe = true;

                    }
                }
            }
        }

        void addDynamicControlCalce(CarBodyInquiryObjects objPack, GlobalInquery newQueryItem, CarBodyInquiryVM input)
        {
            if (input.dynamicCTRLs != null && input.dynamicCTRLs.Count > 0)
            {
                var allDynamicCTRlSelectedValues = objPack.requiredSelectedDynamicCTRLs.SelectMany(t => t.CarExteraDiscountValues).ToList().Where(t => input.dynamicCTRLs.Contains(t.Id)).ToList();
                if (allDynamicCTRlSelectedValues.Count > 0)
                {
                    foreach (var dItemValue in allDynamicCTRlSelectedValues)
                    {
                        var currCarExteraDiscount = objPack.requiredSelectedDynamicCTRLs.Where(t => t.Id == dItemValue.CarExteraDiscountId).FirstOrDefault();
                        if (currCarExteraDiscount.CarExteraDiscountRangeAmounts.Any(t => t.IsActive == true && t.CarExteraDiscountRangeAmountCompanies.Any(tt => tt.CompanyId == newQueryItem.CompanyId)))
                        {
                            dItemValue.CarExteraDiscountRangeAmounts =
                                dItemValue.CarExteraDiscountRangeAmounts.Where(t => t.IsActive == true && t.CarExteraDiscountRangeAmountCompanies.Any(tt => tt.CompanyId == newQueryItem.CompanyId)).ToList();
                            if (dItemValue.CarExteraDiscountRangeAmounts.Count > 0)
                            {
                                GlobalInquiryItem newItem = new GlobalInquiryItem();
                                newItem.GlobalInquiryId = newQueryItem.Id;
                                newItem.Title = currCarExteraDiscount.Title + "/" + dItemValue.Title;
                                newItem.CalcKey = "DR";

                                var s1Price = newQueryItem.GlobalInquiryItems.Where(t => t.CalcKey == "s1").Select(t => t.Price).FirstOrDefault();
                                var crdOlPrice = newQueryItem.GlobalInquiryItems.Where(t => t.CalcKey == "crdOl").Select(t => t.Price).FirstOrDefault();
                                var noDDPrice = newQueryItem.GlobalInquiryItems.Where(t => t.CalcKey == "nodD").Select(t => t.Price).FirstOrDefault();
                                var sumItemPrice = newQueryItem.GlobalInquiryItems.Where(t => t.Expired != true).Select(t => t.Price).Sum();
                                var vUsagPrice = newQueryItem.GlobalInquiryItems.Where(t => t.CalcKey == "vUsag").Select(t => t.Price).FirstOrDefault();

                                newItem.Price = RouteThisNumberIfConfigExist(
                                    cacleInsuranceDynamicCTRL
                                    (
                                        s1Price, sumItemPrice, currCarExteraDiscount.Type, input.createYear, currCarExteraDiscount.CalculateType,
                                        dItemValue.CarExteraDiscountRangeAmounts, crdOlPrice, input.carValue.ToLongReturnZiro() + input.exteraValue.ToLongReturnZiro(),
                                        input.exteraValue.ToLongReturnZiro(), noDDPrice,
                                        (objPack.CarBodyCreateDatePercents != null ? objPack.CarBodyCreateDatePercents.OrderBy(t => t.FromYear).Select(t => t.FromYear).FirstOrDefault() : 0),
                                        vUsagPrice, objPack.VehicleUsage?.BodyPercent
                                    )
                                    , objPack.RoundInquery);
                                if (newItem.Price != 0)
                                    newQueryItem.GlobalInquiryItems.Add(newItem);
                            }
                        }
                        else
                            if (currCarExteraDiscount?.DontRemoveInSearch != true)
                            newQueryItem.deleteMe = true;
                    }
                }
            }
        }

        long cacleInsuranceDynamicCTRL(
            long s1Price, long sumItemPrice, CarExteraDiscountType type, int? createYear, CarExteraDiscountCalculateType calculateType,
            List<CarExteraDiscountRangeAmount> carExteraDiscountRangeAmounts, long crdOlPrice, long investment, long userInutAsscessory,
            long noDDPrice, int createDateIgnore, long vUsagPrice, decimal? bodyPercent)
        {
            long result = 0;
            decimal v100 = 100;

            carExteraDiscountRangeAmounts = carExteraDiscountRangeAmounts.OrderBy(t => t.MinValue).ToList();

            foreach (var rv in carExteraDiscountRangeAmounts)
            {
                if (type == CarExteraDiscountType.Price)
                {
                    if (calculateType == CarExteraDiscountCalculateType.OnUserInputPrice)
                    {
                        if (rv.Amount != null && rv.Amount != 0)
                        {
                            if (investment > rv.MaxValue)
                                result += rv.Amount.Value;
                            else if (investment >= rv.MinValue && investment <= rv.MaxValue)
                                result += rv.Amount.Value;
                        }
                        else if (rv.Percent != 0 && rv.Percent != null)
                        {
                            if (investment > rv.MaxValue)
                            {
                                var def = Convert.ToDecimal(rv.MaxValue - rv.MinValue) * rv.Percent.Value;
                                result += Convert.ToInt32(Math.Ceiling(def / v100));
                            }
                            else if (investment >= rv.MinValue && investment <= rv.MaxValue)
                            {
                                var def = Convert.ToDecimal(investment - rv.MinValue) * rv.Percent.Value;
                                result += Convert.ToInt32(Math.Ceiling(def / v100));
                            }
                        }
                    }
                    else if (calculateType == CarExteraDiscountCalculateType.OnFirstResult)
                    {
                        if (rv.Amount != 0 && rv.Amount != null)
                        {
                            if (sumItemPrice > rv.MaxValue)
                                result += rv.Amount.Value;
                            else if (sumItemPrice >= rv.MinValue && sumItemPrice <= rv.MaxValue)
                                result += rv.Amount.Value;
                        }
                        else if (rv.Percent != 0 && rv.Percent != null)
                        {
                            if (sumItemPrice > rv.MaxValue)
                            {
                                var def = Convert.ToDecimal(rv.MaxValue - rv.MinValue) * rv.Percent.Value;
                                result += Convert.ToInt32(Math.Ceiling(def / v100));
                            }
                            else if (sumItemPrice >= rv.MinValue && sumItemPrice <= rv.MaxValue)
                            {
                                var def = Convert.ToDecimal(sumItemPrice - rv.MinValue) * rv.Percent.Value;
                                result += Convert.ToInt32(Math.Ceiling(def / v100));
                            }
                        }
                    }
                    else if (calculateType == CarExteraDiscountCalculateType.OnBaseAndCreateYear)
                    {
                        var calcPrice = crdOlPrice + s1Price + vUsagPrice;
                        if (rv.Amount != 0 && rv.Amount != null)
                        {
                            if (s1Price > rv.MaxValue)
                                result += rv.Amount.Value;
                            else if (s1Price >= rv.MinValue && s1Price <= rv.MaxValue)
                                result += rv.Amount.Value;
                        }
                        else if (rv.Percent != 0 && rv.Percent != null)
                        {
                            if (s1Price > rv.MaxValue)
                            {
                                var def = Convert.ToDecimal(rv.MaxValue - rv.MinValue) * rv.Percent.Value;
                                result += Convert.ToInt32(Math.Ceiling(def / v100));
                            }
                            else if (s1Price >= rv.MinValue && s1Price <= rv.MaxValue)
                            {
                                var def = Convert.ToDecimal(calcPrice - rv.MinValue) * rv.Percent.Value;
                                result += Convert.ToInt32(Math.Ceiling(def / v100));
                            }
                        }
                    }
                    else if (calculateType == CarExteraDiscountCalculateType.OnUserInputAssessory)
                    {
                        if (rv.Amount != null && rv.Amount != 0)
                        {
                            if (userInutAsscessory > rv.MaxValue)
                                result += rv.Amount.Value;
                            else if (userInutAsscessory >= rv.MinValue && userInutAsscessory <= rv.MaxValue)
                                result += rv.Amount.Value;
                        }
                        else if (rv.Percent != 0 && rv.Percent != null)
                        {
                            if (userInutAsscessory > rv.MaxValue)
                            {
                                var def = Convert.ToDecimal(rv.MaxValue - rv.MinValue) * rv.Percent.Value;
                                result += Convert.ToInt32(Math.Ceiling(def / v100));
                            }
                            else if (userInutAsscessory >= rv.MinValue && userInutAsscessory <= rv.MaxValue)
                            {
                                var def = Convert.ToDecimal(userInutAsscessory - rv.MinValue) * rv.Percent.Value;
                                result += Convert.ToInt32(Math.Ceiling(def / v100));
                            }
                        }
                    }
                    else if (calculateType == CarExteraDiscountCalculateType.OnBaseAndCreateYearAndNoDamageDiscount)
                    {
                        var bcanPrice = noDDPrice + s1Price + crdOlPrice + vUsagPrice;
                        if (rv.Amount != 0 && rv.Amount != null)
                        {
                            if (bcanPrice > rv.MaxValue)
                                result += rv.Amount.Value;
                            else if (bcanPrice >= rv.MinValue && bcanPrice <= rv.MaxValue)
                                result += rv.Amount.Value;
                        }
                        else if (rv.Percent != 0 && rv.Percent != null)
                        {
                            if (bcanPrice > rv.MaxValue)
                            {
                                var def = Convert.ToDecimal(rv.MaxValue - rv.MinValue) * rv.Percent.Value;
                                result += Convert.ToInt32(Math.Ceiling(def / v100));
                            }
                            else if (bcanPrice >= rv.MinValue && bcanPrice <= rv.MaxValue)
                            {
                                var def = Convert.ToDecimal(bcanPrice - rv.MinValue) * rv.Percent.Value;
                                result += Convert.ToInt32(Math.Ceiling(def / v100));
                            }
                        }
                    }
                }
                else if (type == CarExteraDiscountType.Date && createYear > 0)
                {
                    int defYear = DateTime.Now.Year - createYear.Value;
                    if (defYear >= 0)
                    {
                        if (rv.Amount != 0 && rv.Amount != null)
                        {
                            if (defYear > rv.MaxValue)
                                result += rv.Amount.Value;
                            else if (defYear >= rv.MinValue && defYear <= rv.MaxValue)
                                result += rv.Amount.Value;
                        }
                        else if (rv.Percent != 0 && rv.Percent != null)
                        {
                            if (calculateType == CarExteraDiscountCalculateType.OnUserInputPrice)
                            {
                                if (defYear > rv.MaxValue || (defYear >= rv.MinValue && defYear <= rv.MaxValue))
                                {
                                    var def = Convert.ToDecimal(s1Price) * rv.Percent.Value;
                                    result += Convert.ToInt32(Math.Ceiling(def / v100));
                                }
                            }
                            else if (calculateType == CarExteraDiscountCalculateType.OnFirstResult)
                            {
                                if (defYear > rv.MaxValue || (defYear >= rv.MinValue && defYear <= rv.MaxValue))
                                {
                                    var def = Convert.ToDecimal(sumItemPrice) * rv.Percent.Value;
                                    result += Convert.ToInt32(Math.Ceiling(def / v100));
                                }
                            }
                        }
                    }
                }

                if (rv.CreateDateSelfPercent != null && result > 0 && rv.CreateDateSelfPercent != 0 )
                {
                    if(createDateIgnore > 0)
                    {
                        createDateIgnore = createDateIgnore - 1;
                        var defYear = DateTime.Now.Year - createYear.ToIntReturnZiro();
                        defYear = defYear - createDateIgnore;
                        if (defYear > 0)
                            result = result + ((Math.Round(Convert.ToDecimal(result) * rv.CreateDateSelfPercent.Value / v100).ToLongReturnZiro()) * defYear);
                    }
                    if (bodyPercent != null && bodyPercent != 0)
                        result = result + ((Math.Round(Convert.ToDecimal(result) * bodyPercent.Value / v100).ToLongReturnZiro()));
                }
            }

            return result;
        }

        void addCacleDiscountContract(CarBodyInquiryObjects objPack, GlobalInquery newQueryItem)
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

        void addCacleGlobalDiscountAuto(CarBodyInquiryObjects objPack, GlobalInquery newQueryItem)
        {
            if (objPack.GlobalDiscounts == null)
                return;
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

        void addCacleGlobalDiscount(CarBodyInquiryObjects objPack, GlobalInquery newQueryItem, int? siteSettingId, string discountCode)
        {
            if (!string.IsNullOrEmpty(discountCode))
            {
                GlobalDiscount foundDiscountCode = GlobalDiscountService.GetByCode(discountCode, siteSettingId, objPack.currentProposalForm?.Id);
                if (foundDiscountCode != null)
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

        void addCacleDayLimitationDiscount(CarBodyInquiryObjects objPack, GlobalInquery newQueryItem)
        {
            if (objPack.InquiryDuration != null)
            {
                if (objPack.InquiryDuration.InquiryDurationCompanies.Any(t => t.CompanyId == newQueryItem.CompanyId))
                {
                    if (objPack.InquiryDuration.Percent > 0 && objPack.InquiryDuration.Percent < 100)
                    {
                        long price = newQueryItem.GlobalInquiryItems.Where(t => t.Expired != true).Sum(t => t.Price);
                        decimal rPercent = objPack.v100 - objPack.InquiryDuration.Percent;
                        GlobalInquiryItem newItem = new GlobalInquiryItem();
                        newItem.GlobalInquiryId = newQueryItem.Id;
                        newItem.CalcKey = "DS";
                        newItem.Title = objPack.InquiryDuration.Title;
                        newItem.Price = RouteThisNumberIfConfigExist(Convert.ToInt64(Math.Ceiling(((Convert.ToDecimal(price) * rPercent) / objPack.v100) * -1)), objPack.RoundInquery);

                        newQueryItem.GlobalInquiryItems.Add(newItem);
                    }
                }
                else
                    newQueryItem.deleteMe = true;
            }
        }

        void CreateGlobalInqueryObjectForEachCompanyS1(CarBodyInquiryObjects CarBodyInquiryObjects, int? siteSettingId, long globalInputInqueryId, List<GlobalInquery> result, CarBodyInquiryVM input)
        {
            if (CarBodyInquiryObjects.validCompanies != null && CarBodyInquiryObjects.validCompanies.Count > 0 &&
                siteSettingId.ToIntReturnZiro() > 0 && CarBodyInquiryObjects.currentProposalForm != null)
            {
                foreach (var company in CarBodyInquiryObjects.validCompanies)
                {
                    GlobalInquery newQueryItem = new GlobalInquery()
                    {
                        CompanyId = company.Id,
                        CreateDate = DateTime.Now,
                        GlobalInputInqueryId = globalInputInqueryId,
                        SiteSettingId = siteSettingId.Value,
                        ProposalFormId = CarBodyInquiryObjects.currentProposalForm.Id
                    };

                    long basePriceS1 = InquiryCalceS1(company.Id, CarBodyInquiryObjects, newQueryItem, input);

                    if (basePriceS1 > 0)
                        result.Add(newQueryItem);
                }
            }
        }

        long InquiryCalceS1(int companyId, CarBodyInquiryObjects CarBodyInquiryObjects, GlobalInquery newQueryItem, CarBodyInquiryVM input)
        {
            var foundRates = CarBodyInquiryObjects.CarSpecificationAmounts.OrderByDescending(t => t.Id).Where(t => t.CarSpecificationAmountCompanies != null && t.CarSpecificationAmountCompanies.Any(tt => tt.CompanyId == companyId)).ToList();
            if (foundRates != null && foundRates.Count > 0)
            {
                GlobalInquiryItem newITem = new GlobalInquiryItem();
                newITem.CalcKey = "s1";
                newITem.GlobalInquiryId = newQueryItem.Id;
                newITem.Title = BMessages.BasePrice.GetEnumDisplayName();
                newITem.Price = RouteThisNumberIfConfigExist
                    (
                        cacleBodyInsurance(foundRates, input.carValue),
                        CarBodyInquiryObjects.RoundInquery
                    );
                if (newITem.Price > 0)
                    newQueryItem.GlobalInquiryItems.Add(newITem);
                return newITem.Price;
            }
            return 0;
        }

        private long cacleBodyInsurance(IEnumerable<CarSpecificationAmount> specValues, long? carValue)
        {
            long result = 0;

            decimal v100 = 100;

            if (carValue > 0)
            {
                specValues = specValues.OrderBy(t => t.MinAmount).ToList();
                foreach (var sValue in specValues)
                {
                    if (carValue > sValue.MaxAmount)
                    {
                        if (sValue.Amount > 0)
                            result += sValue.Amount.Value;
                        else if (sValue.Rate > 0)
                        {
                            var def = Convert.ToDecimal(sValue.MaxAmount - sValue.MinAmount) * sValue.Rate.Value;
                            result = result + Convert.ToInt64(Math.Ceiling(def / v100));
                        }
                    }
                    else if (carValue <= sValue.MaxAmount && carValue >= sValue.MinAmount)
                    {
                        if (sValue.Amount > 0)
                            result += sValue.Amount.Value;
                        else if (sValue.Rate > 0)
                        {
                            var def = Convert.ToDecimal(carValue.Value - sValue.MinAmount) * sValue.Rate.Value;
                            result = result + Convert.ToInt64(Math.Ceiling(def / v100));
                        }
                    }
                }
            }

            return result;
        }

        object calceResponeForInquiry(List<GlobalInquery> quiryObj, CarBodyInquiryObjects objPack, CarBodyInquiryVM input, string targetArea)
        {
            int row = 0;
            var priceUnit = "ریال";
            var result = quiryObj
                .Where(t => t.deleteMe != true)
                .Select(t => new
                {
                    id = t.Id,
                    idG = t.Id,
                    cid = t.CompanyId,
                    p = t.GlobalInquiryItems.Where(tt => tt.CalcKey == "s1").Select(tt => tt.Price).FirstOrDefault(),
                    cn = objPack.validCompanies.Where(x => x.Id == t.CompanyId).Select(x => x.Title).FirstOrDefault(),
                    cnPic = objPack.validCompanies.Where(x => x.Id == t.CompanyId).Select(x => x.Pic64).FirstOrDefault(),
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
                    p = t.p.ToString("###,###") + priceUnit,
                    sr = GlobalConfig.FileAccessHandlerUrl + t.cnPic,
                    t.cn,
                    sp = (t.dt.Where(tt => tt.isE != true).Count() > 0 ? t.dt.Where(tt => tt.isE != true).Sum(tt => tt.p).ToString("###,###") : "0") + priceUnit,
                    dt = new
                    {
                        total = t.dt.Count(),
                        data = t.dt.Select(tt => new
                        {
                            t = tt.t,
                            p = (tt.p < 0 ? tt.p * -1 : tt.p).ToString("###,###") + (tt.p > 0 ? "+" : tt.p < 0 ? "-" : "") + priceUnit,
                            tt.isE,
                            isET = tt.isE == true ? "عدم اعمال در محاصبات" : "اعمال",
                            tt.c,
                            tt.bp
                        }).ToList()
                    },
                    t.cid,
                    t.desc,
                    fid = objPack.currentProposalForm?.Id,
                    targetArea = targetArea
                }).ToList();


            if (input.showStatus == 2)
                result = result.Where(t => t.hcd == true).ToList();
            else if (input.showStatus == 3)
                result = result.Where(t => t.hcd == true).ToList();
            return new { total = result.Count, data = result };
        }

        void InquiryCalceTaxAndDuty(List<GlobalInquery> result, CarBodyInquiryObjects objPack)
        {
            if (objPack.Tax != null && objPack.Duty != null)
                foreach (var newQueryItem in result)
                {
                    if (objPack.Tax != null && objPack.Duty != null && newQueryItem != null && newQueryItem.GlobalInquiryItems != null && newQueryItem.GlobalInquiryItems.Count > 0)
                    {
                        var sumPrice = newQueryItem.GlobalInquiryItems.Where(t => t.Expired != true && t.CalcKey != "s9").Count() > 0 ?
                            newQueryItem.GlobalInquiryItems.Where(t => t.Expired != true && t.CalcKey != "s9").Sum(t => t.Price) : 0;
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

        void addCacleAddCashDiscount(List<GlobalInquery> result, CarBodyInquiryObjects objPack)
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

        bool IsValidInquiry(CarBodyInquiryVM input)
        {
            return input != null && input.brandId.ToIntReturnZiro() > 0 && input.vehicleTypeId.ToIntReturnZiro() > 0 && input.carTypeId.ToIntReturnZiro() > 0 && input.specId.ToIntReturnZiro() > 0;
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
    }
}
