using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.ProposalFormManager.Interfaces;
using Oje.ProposalFormManager.Models.DB;
using Oje.ProposalFormManager.Models.View;
using Oje.ProposalFormManager.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Oje.ProposalFormManager.Services
{
    public class ThirdPartyRateManager : IThirdPartyRateManager
    {

        readonly ProposalFormDBContext db = null;
        readonly IGlobalInputInqueryManager GlobalInputInqueryManager = null;
        readonly IGlobalInqueryManager GlobalInqueryManager = null;
        readonly ICarExteraDiscountManager CarExteraDiscountManager = null;
        readonly IInquiryCompanyLimitManager InquiryCompanyLimitManager = null;
        readonly ICompanyManager CompanyManager = null;
        readonly IProposalFormManager ProposalFormManager = null;
        readonly IThirdPartyDriverNoDamageDiscountHistoryManager ThirdPartyDriverNoDamageDiscountHistoryManager = null;
        readonly IVehicleTypeManager VehicleTypeManager = null;
        readonly IVehicleSystemManager VehicleSystemManager = null;
        readonly IVehicleUsageManager VehicleUsageManager = null;
        readonly IThirdPartyFinancialAndBodyHistoryDamagePenaltyManager ThirdPartyFinancialAndBodyHistoryDamagePenaltyManager = null;
        readonly IThirdPartyDriverHistoryDamagePenaltyManager ThirdPartyDriverHistoryDamagePenaltyManager = null;
        readonly ICashPayDiscountManager CashPayDiscountManager = null;
        readonly IThirdPartyFinancialCommitmentManager ThirdPartyFinancialCommitmentManager = null;
        readonly IThirdPartyLifeCommitmentManager ThirdPartyLifeCommitmentManager = null;
        readonly IThirdPartyDriverFinancialCommitmentManager ThirdPartyDriverFinancialCommitmentManager = null;
        readonly IThirdPartyPassengerRateManager ThirdPartyPassengerRateManager = null;
        readonly IRoundInqueryManager RoundInqueryManager = null;
        readonly ICarSpecificationManager CarSpecificationManager = null;
        readonly ITaxManager TaxManager = null;
        readonly IDutyManager DutyManager = null;
        readonly IThirdPartyCarCreateDatePercentManager ThirdPartyCarCreateDatePercentManager = null;
        readonly IInquiryMaxDiscountManager InquiryMaxDiscountManager = null;
        readonly IGlobalDiscountManager GlobalDiscountManager = null;
        readonly IThirdPartyRequiredFinancialCommitmentManager ThirdPartyRequiredFinancialCommitmentManager = null;
        readonly IInsuranceContractDiscountManager InsuranceContractDiscountManager = null;
        readonly IInquiryDurationManager InquiryDurationManager = null;
        readonly IInqueryDescriptionManager InqueryDescriptionManager = null;
        readonly IPaymentMethodManager PaymentMethodManager = null;
        readonly IThirdPartyBodyNoDamageDiscountHistoryManager ThirdPartyBodyNoDamageDiscountHistoryManager = null;

        public ThirdPartyRateManager(
            ProposalFormDBContext db,
            IGlobalInputInqueryManager GlobalInputInqueryManager,
            IGlobalInqueryManager GlobalInqueryManager,
            ICarExteraDiscountManager CarExteraDiscountManager,
            IInquiryCompanyLimitManager InquiryCompanyLimitManager,
            ICompanyManager CompanyManager,
            IProposalFormManager ProposalFormManager,
            IThirdPartyDriverNoDamageDiscountHistoryManager ThirdPartyDriverNoDamageDiscountHistoryManager,
            IVehicleTypeManager VehicleTypeManager,
            IVehicleSystemManager VehicleSystemManager,
            IVehicleUsageManager VehicleUsageManager,
            IThirdPartyFinancialAndBodyHistoryDamagePenaltyManager ThirdPartyFinancialAndBodyHistoryDamagePenaltyManager,
            IThirdPartyDriverHistoryDamagePenaltyManager ThirdPartyDriverHistoryDamagePenaltyManager,
            ICashPayDiscountManager CashPayDiscountManager,
            IThirdPartyFinancialCommitmentManager ThirdPartyFinancialCommitmentManager,
            IThirdPartyLifeCommitmentManager ThirdPartyLifeCommitmentManager,
            IThirdPartyDriverFinancialCommitmentManager ThirdPartyDriverFinancialCommitmentManager,
            IThirdPartyPassengerRateManager ThirdPartyPassengerRateManager,
            IRoundInqueryManager RoundInqueryManager,
            ICarSpecificationManager CarSpecificationManager,
            ITaxManager TaxManager,
            IDutyManager DutyManager,
            IThirdPartyCarCreateDatePercentManager ThirdPartyCarCreateDatePercentManager,
            IInquiryMaxDiscountManager InquiryMaxDiscountManager,
            IGlobalDiscountManager GlobalDiscountManager,
            IThirdPartyRequiredFinancialCommitmentManager ThirdPartyRequiredFinancialCommitmentManager,
            IInsuranceContractDiscountManager InsuranceContractDiscountManager,
            IInquiryDurationManager InquiryDurationManager,
            IInqueryDescriptionManager InqueryDescriptionManager,
            IPaymentMethodManager PaymentMethodManager,
            IThirdPartyBodyNoDamageDiscountHistoryManager ThirdPartyBodyNoDamageDiscountHistoryManager
            )
        {
            this.db = db;
            this.GlobalInputInqueryManager = GlobalInputInqueryManager;
            this.GlobalInqueryManager = GlobalInqueryManager;
            this.CarExteraDiscountManager = CarExteraDiscountManager;
            this.InquiryCompanyLimitManager = InquiryCompanyLimitManager;
            this.CompanyManager = CompanyManager;
            this.ProposalFormManager = ProposalFormManager;
            this.ThirdPartyDriverNoDamageDiscountHistoryManager = ThirdPartyDriverNoDamageDiscountHistoryManager;
            this.VehicleTypeManager = VehicleTypeManager;
            this.VehicleSystemManager = VehicleSystemManager;
            this.VehicleUsageManager = VehicleUsageManager;
            this.ThirdPartyFinancialAndBodyHistoryDamagePenaltyManager = ThirdPartyFinancialAndBodyHistoryDamagePenaltyManager;
            this.ThirdPartyDriverHistoryDamagePenaltyManager = ThirdPartyDriverHistoryDamagePenaltyManager;
            this.CashPayDiscountManager = CashPayDiscountManager;
            this.ThirdPartyFinancialCommitmentManager = ThirdPartyFinancialCommitmentManager;
            this.ThirdPartyLifeCommitmentManager = ThirdPartyLifeCommitmentManager;
            this.ThirdPartyDriverFinancialCommitmentManager = ThirdPartyDriverFinancialCommitmentManager;
            this.ThirdPartyPassengerRateManager = ThirdPartyPassengerRateManager;
            this.RoundInqueryManager = RoundInqueryManager;
            this.CarSpecificationManager = CarSpecificationManager;
            this.TaxManager = TaxManager;
            this.DutyManager = DutyManager;
            this.ThirdPartyCarCreateDatePercentManager = ThirdPartyCarCreateDatePercentManager;
            this.InquiryMaxDiscountManager = InquiryMaxDiscountManager;
            this.GlobalDiscountManager = GlobalDiscountManager;
            this.ThirdPartyRequiredFinancialCommitmentManager = ThirdPartyRequiredFinancialCommitmentManager;
            this.InsuranceContractDiscountManager = InsuranceContractDiscountManager;
            this.InquiryDurationManager = InquiryDurationManager;
            this.InqueryDescriptionManager = InqueryDescriptionManager;
            this.PaymentMethodManager = PaymentMethodManager;
            this.ThirdPartyBodyNoDamageDiscountHistoryManager = ThirdPartyBodyNoDamageDiscountHistoryManager;
        }

        public object Inquiry(int? siteSettingId, CarThirdPartyInquiryVM input)
        {
            List<GlobalInquery> result = new List<GlobalInquery>();

            if (IsValidInquiry(input) == true && siteSettingId.ToIntReturnZiro() > 0)
            {
                CarThirdPartyInquiryObjects objPack = fillRequiredObjectsAndValidate(input, siteSettingId);
                List<InqeryExteraParameter> InqeryExteraParameters = fillExteraParamters(objPack);

                long GlobalInputInqueryId = GlobalInputInqueryManager.Create(input, InqeryExteraParameters, siteSettingId);
                if (GlobalInputInqueryId > 0 && objPack.ThirdPartyDriverFinancialCommitment != null && objPack.ThirdPartyRates != null &&
                    objPack.ThirdPartyRates.Count > 0 && objPack.ThirdPartyLifeCommitment != null && objPack.VehicleUsage != null)
                {
                    CreateGlobalInqueryObjectForEachCompanyS1(objPack, siteSettingId, GlobalInputInqueryId, result);
                    foreach (var newQueryItem in result)
                    {
                        InquiryCalceS3(objPack, newQueryItem);
                        InquiryCalceS4(objPack, newQueryItem, input.createYear);
                        InquiryCalceS4_2(objPack, newQueryItem, input.createYear);
                        InquiryCalceS6(objPack, newQueryItem);
                        InquiryCalceS8(objPack, newQueryItem);
                        InquiryCalceS10(objPack, newQueryItem, input.hasRoom);
                        InquiryCalceS13(objPack, newQueryItem);
                        InquiryCalceS15(objPack, newQueryItem);
                        InquiryCalceS9(objPack, newQueryItem, input);
                        InquiryCalceS9_2(objPack, newQueryItem, input);
                    }

                    DublicateInqueryForExteraCommitment(result, objPack, input);

                    foreach (var newQueryItem in result)
                    {
                        InquiryCalceS5(objPack, newQueryItem, input);
                        InquiryCalceS22(objPack, newQueryItem);
                        InquiryCalceS14(objPack, newQueryItem);
                        addDynamicControlCalce(objPack, newQueryItem, input);
                        addCacleRightOptionFilter(objPack, newQueryItem, input);
                        addCacleDiscountContract(objPack, newQueryItem);
                        addCacleGlobalDiscountAuto(objPack, newQueryItem);
                        addCacleGlobalDiscount(objPack, newQueryItem, siteSettingId, input.discountCode);
                        addCacleDayLimitationDiscount(objPack, newQueryItem);
                    }

                    addCacleAddCashDiscount(result, objPack);
                    InquiryCalceTaxAndDuty(result, objPack);
                }
                GlobalInqueryManager.Create(result);

                return calceResponeForInquiry(result, objPack, input);
            }

            return new { total = 0, data = new List<object>() };
        }

        object calceResponeForInquiry(List<GlobalInquery> quiryObj, CarThirdPartyInquiryObjects objPack, CarThirdPartyInquiryVM input)
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
                    cn = objPack.validCompanies.Where(x => x.Id == t.CompanyId).Select(x => x.Title).FirstOrDefault(),
                    cnPic = objPack.validCompanies.Where(x => x.Id == t.CompanyId).Select(x => x.Pic).FirstOrDefault(),
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
                    tmsj = ((t.dt.Any(tt => tt.c == "s2" && tt.bp.ToLongReturnZiro() > 0) ? t.dt.Where(tt => tt.c == "s2").Select(tt => tt.bp).FirstOrDefault() : objPack.ThirdPartyFinancialCommitmentLong).Value.ToString("###,###")),
                    tjsj = objPack.ThirdPartyLifeCommitmentLong.ToString("###,###"),
                    trsj = objPack.ThirdPartyDriverFinancialCommitmentLong.ToString("###,###"),
                    t.cid,
                    t.desc,
                    fid = objPack.currentProposalForm?.Id
                }).ToList();


            if (input.showStatus == 2)
                result = result.Where(t => t.hcd == true).ToList();
            else if (input.showStatus == 3)
                result = result.Where(t => t.hcd == true).ToList();
            return new { total = result.Count, data = result };
        }

        void InquiryCalceTaxAndDuty(List<GlobalInquery> result, CarThirdPartyInquiryObjects objPack)
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

        void addCacleAddCashDiscount(List<GlobalInquery> result, CarThirdPartyInquiryObjects objPack)
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

        void addCacleDayLimitationDiscount(CarThirdPartyInquiryObjects objPack, GlobalInquery newQueryItem)
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

        void addCacleGlobalDiscount(CarThirdPartyInquiryObjects objPack, GlobalInquery newQueryItem, int? siteSettingId, string discountCode)
        {
            if (!string.IsNullOrEmpty(discountCode))
            {
                GlobalDiscount foundDiscountCode = GlobalDiscountManager.GetByCode(discountCode, siteSettingId, objPack.currentProposalForm?.Id);
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

        void addCacleGlobalDiscountAuto(CarThirdPartyInquiryObjects objPack, GlobalInquery newQueryItem)
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

        void addCacleDiscountContract(CarThirdPartyInquiryObjects objPack, GlobalInquery newQueryItem)
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

        void addCacleRightOptionFilter(CarThirdPartyInquiryObjects objPack, GlobalInquery newQueryItem, CarThirdPartyInquiryVM input)
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
                            var sumItemPrice = newQueryItem.GlobalInquiryItems.Where(t => t.Expired != true).Select(t => t.Price).Sum();

                            newItem.Price = RouteThisNumberIfConfigExist(cacleInsuranceDynamicCTRL(
                                s1Price, sumItemPrice, dItemValue.Type,
                                input.createYear, dItemValue.CalculateType, allRangeAmount), objPack.RoundInquery);
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

        void addDynamicControlCalce(CarThirdPartyInquiryObjects objPack, GlobalInquery newQueryItem, CarThirdPartyInquiryVM input)
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
                                var sumItemPrice = newQueryItem.GlobalInquiryItems.Where(t => t.Expired != true).Select(t => t.Price).Sum();

                                newItem.Price = RouteThisNumberIfConfigExist(cacleInsuranceDynamicCTRL(
                                    s1Price, sumItemPrice, currCarExteraDiscount.Type,
                                    input.createYear, currCarExteraDiscount.CalculateType, dItemValue.CarExteraDiscountRangeAmounts), objPack.RoundInquery);
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
            long userInputPrice, long basePrice, CarExteraDiscountType type, int? createYear, CarExteraDiscountCalculateType calculateType,
            List<CarExteraDiscountRangeAmount> carExteraDiscountRangeAmounts
            )
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
                            if (userInputPrice > rv.MaxValue)
                                result += rv.Amount.Value;
                            else if (userInputPrice >= rv.MinValue && userInputPrice <= rv.MaxValue)
                                result += rv.Amount.Value;
                        }
                        else if (rv.Percent != 0 && rv.Percent != null)
                        {
                            if (userInputPrice > rv.MaxValue)
                            {
                                var def = Convert.ToDecimal(rv.MaxValue - rv.MinValue) * rv.Percent.Value;
                                result += Convert.ToInt32(Math.Ceiling(def / v100));
                            }
                            else if (userInputPrice >= rv.MinValue && userInputPrice <= rv.MaxValue)
                            {
                                var def = Convert.ToDecimal(userInputPrice - rv.MinValue) * rv.Percent.Value;
                                result += Convert.ToInt32(Math.Ceiling(def / v100));
                            }
                        }
                    }
                    else if (calculateType == CarExteraDiscountCalculateType.OnFirstResult)
                    {
                        if (rv.Amount != 0 && rv.Amount != null)
                        {
                            if (basePrice > rv.MaxValue)
                                result += rv.Amount.Value;
                            else if (basePrice >= rv.MinValue && basePrice <= rv.MaxValue)
                                result += rv.Amount.Value;
                        }
                        else if (rv.Percent != 0 && rv.Percent != null)
                        {
                            if (basePrice > rv.MaxValue)
                            {
                                var def = Convert.ToDecimal(rv.MaxValue - rv.MinValue) * rv.Percent.Value;
                                result += Convert.ToInt32(Math.Ceiling(def / v100));
                            }
                            else if (basePrice >= rv.MinValue && basePrice <= rv.MaxValue)
                            {
                                var def = Convert.ToDecimal(basePrice - rv.MinValue) * rv.Percent.Value;
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
                                    var def = Convert.ToDecimal(userInputPrice) * rv.Percent.Value;
                                    result += Convert.ToInt32(Math.Ceiling(def / v100));
                                }
                            }
                            else if (calculateType == CarExteraDiscountCalculateType.OnFirstResult)
                            {
                                if (defYear > rv.MaxValue || (defYear >= rv.MinValue && defYear <= rv.MaxValue))
                                {
                                    var def = Convert.ToDecimal(basePrice) * rv.Percent.Value;
                                    result += Convert.ToInt32(Math.Ceiling(def / v100));
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }

        void InquiryCalceS14(CarThirdPartyInquiryObjects objPack, GlobalInquery newQueryItem)
        {
            if (
                    (objPack.bodyNoDamagePercent != null && objPack.bodyNoDamagePercent.Percent >= 0) ||
                    (objPack.ThirdPartyFinancialAndBodyHistoryDamagePenaltyNotFinancial != null && objPack.ThirdPartyFinancialAndBodyHistoryDamagePenaltyNotFinancial.Percent >= 0) ||
                    (objPack.ThirdPartyFinancialAndBodyHistoryDamagePenaltyFinancial != null && objPack.ThirdPartyFinancialAndBodyHistoryDamagePenaltyFinancial.Percent >= 0)
                )
            {
                var foundS2 = newQueryItem.GlobalInquiryItems.Where(t => t.CalcKey == "s2").FirstOrDefault();
                var foundS7 = newQueryItem.GlobalInquiryItems.Where(t => t.CalcKey == "s7").FirstOrDefault();
                var foundS22 = newQueryItem.GlobalInquiryItems.Where(t => t.CalcKey == "s22").FirstOrDefault();
                var foundS5 = newQueryItem.GlobalInquiryItems.Where(t => t.CalcKey == "s5").FirstOrDefault();
                if (foundS7 == null)
                    foundS7 = new GlobalInquiryItem() { Price = 0 };
                if (foundS5 == null)
                    foundS5 = new GlobalInquiryItem() { Price = 0 };
                if (foundS22 == null)
                    foundS22 = new GlobalInquiryItem() { Price = 0 };

                if (foundS2 != null && foundS7 != null && foundS5 != null)
                {
                    decimal targetPercent = 0;
                    if (objPack.bodyNoDamagePercent != null)
                        targetPercent = objPack.bodyNoDamagePercent.Percent;
                    if (
                            objPack.bodyNoDamagePercent != null && objPack.bodyNoDamagePercent.Percent >= 0 &&
                            (objPack.ThirdPartyFinancialAndBodyHistoryDamagePenaltyNotFinancial == null || objPack.ThirdPartyFinancialAndBodyHistoryDamagePenaltyNotFinancial.Percent == 0) &&
                            (objPack.ThirdPartyFinancialAndBodyHistoryDamagePenaltyFinancial == null || objPack.ThirdPartyFinancialAndBodyHistoryDamagePenaltyFinancial.Percent == 0)
                        )
                    {
                        if (objPack.usMinus5ForNoDamageDiscount == false)
                            targetPercent += 5;
                        if (targetPercent > 70)
                            targetPercent = 70;
                    }
                    targetPercent = targetPercent -
                        (objPack.ThirdPartyFinancialAndBodyHistoryDamagePenaltyNotFinancial == null ? 0 : objPack.ThirdPartyFinancialAndBodyHistoryDamagePenaltyNotFinancial.Percent) -
                         (objPack.ThirdPartyFinancialAndBodyHistoryDamagePenaltyFinancial == null ? 0 : objPack.ThirdPartyFinancialAndBodyHistoryDamagePenaltyFinancial.Percent)
                        ;
                    if (targetPercent > 0)
                    {
                        GlobalInquiryItem newITem = new GlobalInquiryItem();
                        newITem.CalcKey = "s14";
                        newITem.GlobalInquiryId = newQueryItem.Id;
                        newITem.Title = BMessages.NoDamage_Discount_Extera.GetEnumDisplayName();
                        newITem.Price = RouteThisNumberIfConfigExist(
                            Math.Ceiling((targetPercent / objPack.v100) * Convert.ToDecimal(foundS2.Price + foundS5.Price + foundS7.Price + foundS22.Price)).ToLongReturnZiro() * -1, objPack.RoundInquery);
                        if (newITem.Price < 0)
                            newQueryItem.GlobalInquiryItems.Add(newITem);
                    }
                    else if (targetPercent < 0)
                    {
                        GlobalInquiryItem newITem = new GlobalInquiryItem();
                        newITem.CalcKey = "s20";
                        newITem.Title = BMessages.DamagePenalty_Body_Financial_Extera.GetEnumDisplayName();
                        newITem.GlobalInquiryId = newQueryItem.Id;
                        newITem.Price = RouteThisNumberIfConfigExist(Math.Ceiling((targetPercent / objPack.v100) * Convert.ToDecimal(foundS2.Price + foundS5.Price + foundS22.Price)).ToLongReturnZiro() * -1, objPack.RoundInquery);
                        if (newITem.Price > 0)
                            newQueryItem.GlobalInquiryItems.Add(newITem);
                    }
                }
            }
        }

        void InquiryCalceS22(CarThirdPartyInquiryObjects objPack, GlobalInquery newQueryItem)
        {
            var foundS2 = newQueryItem.GlobalInquiryItems.Where(t => t.CalcKey == "s2").FirstOrDefault();

            if (objPack.VehicleUsage != null && objPack.VehicleUsage.ThirdPartyPercent > 0 && foundS2 != null)
            {
                GlobalInquiryItem newITem = new GlobalInquiryItem();
                newITem.CalcKey = "s22";
                newITem.GlobalInquiryId = newQueryItem.Id;
                newITem.Title = BMessages.Extera_CarUsage_Price.GetEnumDisplayName();
                newITem.Price = RouteThisNumberIfConfigExist(Math.Ceiling((objPack.VehicleUsage.ThirdPartyPercent.Value * Convert.ToDecimal(foundS2.Price)) / objPack.v100).ToLongReturnZiro(), objPack.RoundInquery);
                if (newITem.Price > 0)
                    newQueryItem.GlobalInquiryItems.Add(newITem);
            }
        }

        void InquiryCalceS5(CarThirdPartyInquiryObjects objPack, GlobalInquery newQueryItem, CarThirdPartyInquiryVM input)
        {
            var foundS2 = newQueryItem.GlobalInquiryItems.Where(t => t.CalcKey == "s2").FirstOrDefault();
            if (foundS2 != null && objPack.ThirdPartyCarCreateDatePercents != null && objPack.ThirdPartyCarCreateDatePercents.Count > 0)
            {
                var defYear = DateTime.Now.Year - input.createYear.ToIntReturnZiro();
                if (defYear > 0)
                {
                    var foundTargetRate = objPack.ThirdPartyCarCreateDatePercents.Where(t => t.FromYear <= defYear && t.ToYear > defYear).FirstOrDefault();
                    if (foundTargetRate != null)
                    {
                        GlobalInquiryItem newITem = new GlobalInquiryItem()
                        {
                            CalcKey = "s5",
                            GlobalInquiryId = newQueryItem.Id,
                            Title = BMessages.Extera_CreateDate_Price.GetEnumDisplayName(),
                            Price = RouteThisNumberIfConfigExist(Math.Ceiling((foundTargetRate.Percent * Convert.ToDecimal(foundS2.Price)) / objPack.v100).ToLongReturnZiro(), objPack.RoundInquery)
                        };
                        if (newITem.Price > 0)
                            newQueryItem.GlobalInquiryItems.Add(newITem);
                    }
                }
            }
        }

        void DublicateInqueryForExteraCommitment(List<GlobalInquery> inputResult, CarThirdPartyInquiryObjects objPack, CarThirdPartyInquiryVM input)
        {
            if (input.coverIds == null || input.coverIds.Count == 0 || objPack.ThirdPartyRequiredFinancialCommitments == null || objPack.ThirdPartyRequiredFinancialCommitments.Count == 0)
                return;

            List<GlobalInquery> result = new List<GlobalInquery>();

            foreach (var inquery in inputResult)
            {
                foreach (var ex in objPack.ThirdPartyRequiredFinancialCommitments)
                {
                    if (ex.ThirdPartyRequiredFinancialCommitmentCompanies != null && ex.ThirdPartyRequiredFinancialCommitmentCompanies.Any(t => t.CompanyId == inquery.CompanyId))
                    {
                        var listRate = ex.ThirdPartyExteraFinancialCommitments
                            .Where(t => t.IsActive == true && t.ThirdPartyExteraFinancialCommitmentComs.Any(tt => tt.CompanyId == inquery.CompanyId)).ToList();
                        if (listRate.Count > 0)
                        {
                            var foundWithType = listRate.OrderByDescending(t => t.Year).Where(t => t.CarSpecificationId == objPack.VehicleType?.CarSpecificationId).FirstOrDefault();
                            if (foundWithType == null)
                                foundWithType = listRate.OrderByDescending(t => t.Year).FirstOrDefault();
                            if (foundWithType != null)
                            {
                                GlobalInquery newITem = new GlobalInquery();
                                if (inquery.GlobalInputInqueryId.ToIntReturnZiro() > 0)
                                    newITem.GlobalInputInqueryId = inquery.GlobalInputInqueryId;
                                newITem.CompanyId = inquery.CompanyId;
                                newITem.Company = inquery.Company;
                                newITem.SiteSettingId = inquery.SiteSettingId;
                                newITem.ProposalFormId = inquery.ProposalFormId;
                                foreach (var qItem in inquery.GlobalInquiryItems)
                                {
                                    newITem.GlobalInquiryItems.Add(new GlobalInquiryItem()
                                    {
                                        CalcKey = qItem.CalcKey,
                                        Expired = qItem.Expired,
                                        GlobalInquiryId = newITem.Id,
                                        GlobalDiscountId = qItem.GlobalDiscountId,
                                        Price = qItem.Price,
                                        Title = qItem.Title
                                    });
                                }
                                if (objPack.ThirdPartyFinancialCommitment != null)
                                {
                                    newITem.GlobalInquiryItems.Add(new GlobalInquiryItem()
                                    {
                                        CalcKey = "s2",
                                        Price = Math.Ceiling(Convert.ToDecimal(ex.Price - objPack.ThirdPartyFinancialCommitment.Price) * foundWithType.Rate).ToLongReturnZiro(),
                                        Title = ex.Title,
                                        GlobalInquiryId = newITem.Id,
                                        basePriceEC = foundWithType?.ThirdPartyRequiredFinancialCommitment?.Price
                                    });
                                    result.Add(newITem);
                                }
                            }
                        }
                    }
                }
            }
            if (input.coverIds != null && input.coverIds.Count > 0)
                inputResult.Clear();

            inputResult.AddRange(result);
        }

        void InquiryCalceS9_2(CarThirdPartyInquiryObjects objPack, GlobalInquery newQueryItem, CarThirdPartyInquiryVM input)
        {
            if (input.havePrevInsurance != null && input.havePrevInsurance == 0 && input.isNewCar != null && input.isNewCar == 0)
            {
                var foundS1 = newQueryItem.GlobalInquiryItems.Where(t => t.CalcKey == "s1").FirstOrDefault();
                var foundS4 = newQueryItem.GlobalInquiryItems.Where(t => t.CalcKey == "s4").FirstOrDefault();
                var foundS6 = newQueryItem.GlobalInquiryItems.Where(t => t.CalcKey == "s6").FirstOrDefault();
                if (foundS4 == null)
                    foundS4 = new GlobalInquiryItem() { Price = 0 };
                if (foundS6 == null)
                    foundS6 = new GlobalInquiryItem() { Price = 0 };

                if (foundS1 != null && foundS4 != null && foundS6 != null)
                {
                    GlobalInquiryItem newITem = new GlobalInquiryItem();
                    newITem.CalcKey = "s9_2";
                    newITem.GlobalInquiryId = newQueryItem.Id;
                    newITem.Title = BMessages.Dont_Have_Insurance_Planty.GetEnumDisplayName();
                    newITem.Price = RouteThisNumberIfConfigExist(foundS1.Price + foundS4.Price + foundS6.Price, objPack.RoundInquery);
                    if (newITem.Price > 0)
                        newQueryItem.GlobalInquiryItems.Add(newITem);
                }
            }
        }

        void InquiryCalceS9(CarThirdPartyInquiryObjects objPack, GlobalInquery newQueryItem, CarThirdPartyInquiryVM input)
        {
            if (input.havePrevInsurance > 0 && !string.IsNullOrEmpty(input.prevEndDate) && input.prevEndDate.ConvertPersianNumberToEnglishNumber().ToEnDate() != null &&
                input.prevEndDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value <= DateTime.Now)
            {
                var endDate = input.prevEndDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value;
                var foundS1 = newQueryItem.GlobalInquiryItems.Where(t => t.CalcKey == "s1").FirstOrDefault();
                var foundS6 = newQueryItem.GlobalInquiryItems.Where(t => t.CalcKey == "s6").FirstOrDefault();
                var defDay = Math.Floor((DateTime.Now - endDate).TotalDays).ToIntReturnZiro();

                if (foundS6 == null)
                    foundS6 = new GlobalInquiryItem();

                if (defDay > 0 && foundS1 != null)
                {
                    decimal v365 = 365;
                    if (defDay > 365)
                        defDay = 365;
                    GlobalInquiryItem newITem = new GlobalInquiryItem();
                    newITem.CalcKey = "s9";
                    newITem.GlobalInquiryId = newQueryItem.Id;
                    newITem.Title = string.Format(BMessages.Delay_Day.GetEnumDisplayName(), defDay);
                    newITem.Price = RouteThisNumberIfConfigExist((Math.Ceiling(Convert.ToDecimal(foundS1.Price + foundS6.Price) / v365) * Convert.ToDecimal(defDay)).ToLongReturnZiro(), objPack.RoundInquery);
                    if (newITem.Price > 0)
                        newQueryItem.GlobalInquiryItems.Add(newITem);
                }
            }
        }

        void InquiryCalceS15(CarThirdPartyInquiryObjects objPack, GlobalInquery newQueryItem)
        {
            var foundS3 = newQueryItem.GlobalInquiryItems.Where(t => t.CalcKey == "s3").FirstOrDefault();
            var foundS8 = newQueryItem.GlobalInquiryItems.Where(t => t.CalcKey == "s8").FirstOrDefault();
            var foundS4_2 = newQueryItem.GlobalInquiryItems.Where(t => t.CalcKey == "s4_2").FirstOrDefault();

            if (foundS3 == null)
                foundS3 = new GlobalInquiryItem() { Price = 0 };
            if (foundS8 == null)
                foundS8 = new GlobalInquiryItem() { Price = 0 };
            if (foundS4_2 == null)
                foundS4_2 = new GlobalInquiryItem() { Price = 0 };

            if (foundS3 != null && foundS8 != null)
            {
                decimal targetPercent = 0;
                if (objPack.ThirdPartyDriverNoDamageDiscountHistory != null)
                    targetPercent = objPack.ThirdPartyDriverNoDamageDiscountHistory.Percent;
                if (objPack.ThirdPartyDriverNoDamageDiscountHistory != null && objPack.ThirdPartyDriverNoDamageDiscountHistory.Percent >= 0 &&
                    (objPack.ThirdPartyDriverHistoryDamagePenalty == null || objPack.ThirdPartyDriverHistoryDamagePenalty.Percent == 0))
                {
                    if (objPack.usMinus5ForNoDamageDiscount == false)
                        targetPercent += 5;
                    if (targetPercent > 70)
                        targetPercent = 70;
                }
                targetPercent = targetPercent -
                    (objPack.ThirdPartyDriverHistoryDamagePenalty == null ? 0 : objPack.ThirdPartyDriverHistoryDamagePenalty.Percent);

                if (targetPercent > 0)
                {
                    GlobalInquiryItem newITem = new GlobalInquiryItem();
                    newITem.CalcKey = "s15";
                    newITem.GlobalInquiryId = newQueryItem.Id;
                    newITem.Title = BMessages.Discount_Accident_Driver.GetEnumDisplayName();
                    newITem.Price = RouteThisNumberIfConfigExist((Math.Ceiling((Convert.ToDecimal(foundS3.Price + foundS8.Price + foundS4_2.Price) * targetPercent) / objPack.v100)).ToLongReturnZiro() * -1, objPack.RoundInquery);
                    if (newITem.Price < 0)
                        newQueryItem.GlobalInquiryItems.Add(newITem);
                }
                else if (targetPercent < 0)
                {
                    GlobalInquiryItem newITem = new GlobalInquiryItem();
                    newITem.CalcKey = "s21";
                    newITem.Title = BMessages.Accident_Driver_Penalty.GetEnumDisplayName();
                    newITem.GlobalInquiryId = newQueryItem.Id;
                    newITem.Price = RouteThisNumberIfConfigExist((Math.Ceiling((Convert.ToDecimal(foundS3.Price + foundS8.Price + foundS4_2.Price) * targetPercent) / objPack.v100)).ToLongReturnZiro() * -1, objPack.RoundInquery);
                    if (newITem.Price > 0)
                        newQueryItem.GlobalInquiryItems.Add(newITem);
                }
            }
        }

        void InquiryCalceS13(CarThirdPartyInquiryObjects objPack, GlobalInquery newQueryItem)
        {
            if (
                (objPack.bodyNoDamagePercent != null && objPack.bodyNoDamagePercent.Percent >= 0) ||
                (objPack.ThirdPartyFinancialAndBodyHistoryDamagePenaltyNotFinancial != null && objPack.ThirdPartyFinancialAndBodyHistoryDamagePenaltyNotFinancial.Percent >= 0) ||
                (objPack.ThirdPartyFinancialAndBodyHistoryDamagePenaltyFinancial != null && objPack.ThirdPartyFinancialAndBodyHistoryDamagePenaltyFinancial.Percent >= 0)
                )
            {
                var foundS1 = newQueryItem.GlobalInquiryItems.Where(t => t.CalcKey == "s1").FirstOrDefault();
                var foundS4 = newQueryItem.GlobalInquiryItems.Where(t => t.CalcKey == "s4").FirstOrDefault();
                var foundS6 = newQueryItem.GlobalInquiryItems.Where(t => t.CalcKey == "s6").FirstOrDefault();
                if (foundS4 == null)
                    foundS4 = new GlobalInquiryItem() { Price = 0 };
                if (foundS6 == null)
                    foundS6 = new GlobalInquiryItem() { Price = 0 };

                decimal targetPercent = 0;

                if (objPack.bodyNoDamagePercent != null)
                    targetPercent = objPack.bodyNoDamagePercent.Percent;
                if (foundS1 != null && foundS4 != null && foundS6 != null)
                {
                    if (
                            objPack.bodyNoDamagePercent != null && objPack.bodyNoDamagePercent.Percent >= 0 &&
                            (objPack.ThirdPartyFinancialAndBodyHistoryDamagePenaltyNotFinancial == null || objPack.ThirdPartyFinancialAndBodyHistoryDamagePenaltyNotFinancial.Percent == 0) &&
                            (objPack.ThirdPartyFinancialAndBodyHistoryDamagePenaltyFinancial == null || objPack.ThirdPartyFinancialAndBodyHistoryDamagePenaltyFinancial.Percent == 0)
                        )
                    {
                        if (objPack.usMinus5ForNoDamageDiscount == false)
                            targetPercent += 5;
                        if (targetPercent > 70)
                            targetPercent = 70;
                    }
                    targetPercent =
                        targetPercent -
                        (objPack.ThirdPartyFinancialAndBodyHistoryDamagePenaltyNotFinancial == null ? 0 : objPack.ThirdPartyFinancialAndBodyHistoryDamagePenaltyNotFinancial.Percent) -
                        (objPack.ThirdPartyFinancialAndBodyHistoryDamagePenaltyFinancial == null ? 0 : objPack.ThirdPartyFinancialAndBodyHistoryDamagePenaltyFinancial.Percent)
                        ;
                    if (targetPercent > 0)
                    {
                        GlobalInquiryItem newITem = new GlobalInquiryItem();
                        newITem.CalcKey = "s13";
                        newITem.GlobalInquiryId = newQueryItem.Id;
                        newITem.Title = BMessages.NoDamage_Discount.GetEnumDisplayName();
                        newITem.Price =
                            RouteThisNumberIfConfigExist(Math.Ceiling((targetPercent / objPack.v100) * Convert.ToDecimal(foundS1.Price + foundS6.Price + foundS4.Price)).ToLongReturnZiro() * -1, objPack.RoundInquery);
                        if (newITem.Price < 0)
                            newQueryItem.GlobalInquiryItems.Add(newITem);
                    }
                    else if (targetPercent < 0)
                    {
                        GlobalInquiryItem newITem = new GlobalInquiryItem();
                        newITem.CalcKey = "s19";
                        newITem.GlobalInquiryId = newQueryItem.Id;
                        newITem.Title = BMessages.Damage_Penalty_Finanical_And_Body.GetEnumDisplayName();
                        newITem.Price = RouteThisNumberIfConfigExist(Math.Ceiling((targetPercent / objPack.v100) * Convert.ToDecimal(foundS1.Price + foundS6.Price + foundS4.Price)).ToLongReturnZiro() * -1, objPack.RoundInquery);
                        if (newITem.Price > 0)
                            newQueryItem.GlobalInquiryItems.Add(newITem);
                    }
                }
            }
        }

        void InquiryCalceS10(CarThirdPartyInquiryObjects objPack, GlobalInquery newQueryItem, bool? hasRoom)
        {
            var foundS1 = newQueryItem.GlobalInquiryItems.Where(t => t.CalcKey == "s1").FirstOrDefault();
            var foundS4 = newQueryItem.GlobalInquiryItems.Where(t => t.CalcKey == "s4").FirstOrDefault();
            if (foundS4 == null)
                foundS4 = new GlobalInquiryItem() { Price = 0 };

            if (foundS1 != null && foundS1.Price > 0 && objPack.CarSpecification != null && hasRoom == true && objPack.CarSpecification.CarRoomRate != null && objPack.CarSpecification.CarRoomRate > 0)
            {
                GlobalInquiryItem newITem = new GlobalInquiryItem();
                newITem.CalcKey = "s10";
                newITem.Title = BMessages.Car_Cargo_Price.GetEnumDisplayName();
                newITem.GlobalInquiryId = newQueryItem.Id;
                newITem.Price = RouteThisNumberIfConfigExist((Math.Ceiling(Convert.ToDecimal(foundS1.Price + foundS4.Price) * objPack.CarSpecification.CarRoomRate.Value)).ToLongReturnZiro(), objPack.RoundInquery);
                if (newITem.Price > 0)
                    newQueryItem.GlobalInquiryItems.Add(newITem);
            }
        }

        void InquiryCalceS8(CarThirdPartyInquiryObjects objPack, GlobalInquery newQueryItem)
        {
            var foundS3 = newQueryItem.GlobalInquiryItems.Where(t => t.CalcKey == "s3").FirstOrDefault();

            decimal v100 = 100;

            if (objPack.VehicleUsage != null && objPack.VehicleUsage.ThirdPartyPercent > 0 && foundS3 != null)
            {
                GlobalInquiryItem newITem = new GlobalInquiryItem();
                newITem.CalcKey = "s8";
                newITem.GlobalInquiryId = newQueryItem.Id;
                newITem.Title = BMessages.Driver_Car_Usage_Price.GetEnumDisplayName();
                newITem.Price = RouteThisNumberIfConfigExist(Math.Ceiling((objPack.VehicleUsage.ThirdPartyPercent.Value * Convert.ToDecimal(foundS3.Price)) / v100).ToLongReturnZiro(), objPack.RoundInquery);
                if (newITem.Price > 0)
                    newQueryItem.GlobalInquiryItems.Add(newITem);
            }
        }

        void InquiryCalceS6(CarThirdPartyInquiryObjects objPack, GlobalInquery newQueryItem)
        {
            var foundS1 = newQueryItem.GlobalInquiryItems.Where(t => t.CalcKey == "s1").FirstOrDefault();

            if (objPack.VehicleUsage != null && objPack.VehicleUsage.ThirdPartyPercent > 0 && foundS1 != null && foundS1.Price > 0)
            {
                GlobalInquiryItem newITem = new GlobalInquiryItem();
                newITem.CalcKey = "s6";
                newITem.GlobalInquiryId = newQueryItem.Id;
                newITem.Title = BMessages.CarUsage_BasePrice.GetEnumDisplayName();
                newITem.Price = RouteThisNumberIfConfigExist(Math.Ceiling((objPack.VehicleUsage.ThirdPartyPercent.Value * Convert.ToDecimal(foundS1.Price)) / objPack.v100).ToLongReturnZiro(), objPack.RoundInquery);
                if (newITem.Price > 0)
                    newQueryItem.GlobalInquiryItems.Add(newITem);
            }
        }

        void InquiryCalceS4_2(CarThirdPartyInquiryObjects objPack, GlobalInquery newQueryItem, int? createYear)
        {
            if (createYear != null && createYear > 0 && objPack.ThirdPartyCarCreateDatePercents != null && objPack.ThirdPartyCarCreateDatePercents.Count > 0)
            {
                var foundS3 = newQueryItem.GlobalInquiryItems.Where(t => t.CalcKey == "s3").FirstOrDefault();
                var defYear = DateTime.Now.Year - createYear.ToIntReturnZiro();
                if (defYear > 0 && foundS3 != null && foundS3.Price > 0)
                {
                    var foundTargetRate = objPack.ThirdPartyCarCreateDatePercents.Where(t => t.FromYear <= defYear && t.ToYear > defYear).FirstOrDefault();
                    if (foundTargetRate != null)
                    {
                        GlobalInquiryItem newITem = new GlobalInquiryItem();
                        newITem.CalcKey = "s4_2";
                        newITem.Title = BMessages.Base_CreateDate_Driver_Accident_Price.GetEnumDisplayName();
                        newITem.GlobalInquiryId = newQueryItem.Id;
                        newITem.Price = RouteThisNumberIfConfigExist(Math.Ceiling((foundTargetRate.Percent * Convert.ToDecimal(foundS3.Price)) / objPack.v100).ToLongReturnZiro(), objPack.RoundInquery);
                        if (newITem.Price > 0)
                            newQueryItem.GlobalInquiryItems.Add(newITem);
                    }
                }
            }
        }

        void InquiryCalceS4(CarThirdPartyInquiryObjects objPack, GlobalInquery newQueryItem, int? createYear)
        {
            if (createYear != null && createYear > 0 && objPack.ThirdPartyCarCreateDatePercents != null && objPack.ThirdPartyCarCreateDatePercents.Count > 0)
            {
                var s1Price = newQueryItem.GlobalInquiryItems.Where(t => t.CalcKey == "s1").FirstOrDefault();
                var defYear = DateTime.Now.Year - createYear.ToIntReturnZiro();
                if (defYear > 0 && s1Price != null)
                {
                    var foundTargetRate = objPack.ThirdPartyCarCreateDatePercents.Where(t => t.FromYear <= defYear && t.ToYear > defYear).FirstOrDefault();
                    if (foundTargetRate != null)
                    {
                        GlobalInquiryItem newITem = new GlobalInquiryItem();
                        newITem.CalcKey = "s4";
                        newITem.Title = BMessages.Base_CreateDate_Price.GetEnumDisplayName();
                        newITem.GlobalInquiryId = newQueryItem.Id;
                        newITem.Price = RouteThisNumberIfConfigExist(Math.Ceiling((foundTargetRate.Percent * Convert.ToDecimal(s1Price.Price)) / objPack.v100).ToLongReturnZiro(), objPack.RoundInquery);
                        if (newITem.Price > 0)
                            newQueryItem.GlobalInquiryItems.Add(newITem);
                    }
                }
            }
        }

        void InquiryCalceS3(CarThirdPartyInquiryObjects carThirdPartyInquiryObjects, GlobalInquery newQueryItem)
        {
            if (carThirdPartyInquiryObjects.ThirdPartyPassengerRates != null && carThirdPartyInquiryObjects.VehicleType != null)
            {
                var foundRate = carThirdPartyInquiryObjects.ThirdPartyPassengerRates.OrderByDescending(t => t.Id)
                .Where(t => t.ThirdPartyPassengerRateCompanies != null && t.ThirdPartyPassengerRateCompanies.Any(tt => tt.CompanyId == newQueryItem.CompanyId))
                .Where(t => t.CarSpecificationId == carThirdPartyInquiryObjects.VehicleType.CarSpecificationId)
                .FirstOrDefault();
                if (foundRate != null && carThirdPartyInquiryObjects.ThirdPartyDriverFinancialCommitment != null)
                {
                    GlobalInquiryItem newITem = new GlobalInquiryItem();
                    newITem.CalcKey = "s3";
                    newITem.GlobalInquiryId = newQueryItem.Id;
                    newITem.Title = BMessages.Driver_Accident_Price.GetEnumDisplayName();
                    newITem.Price = RouteThisNumberIfConfigExist
                        (
                            Math.Ceiling
                            (
                                foundRate.Rate * Convert.ToDecimal(carThirdPartyInquiryObjects.ThirdPartyDriverFinancialCommitment.Price)
                            ).ToLongReturnZiro(),
                            carThirdPartyInquiryObjects.RoundInquery
                        );
                    if (newITem.Price > 0)
                        newQueryItem.GlobalInquiryItems.Add(newITem);
                }
                else
                    newQueryItem.deleteMe = true;
            }
            else
                newQueryItem.deleteMe = true;

        }

        void CreateGlobalInqueryObjectForEachCompanyS1(CarThirdPartyInquiryObjects carThirdPartyInquiryObjects, int? siteSettingId, long globalInputInqueryId, List<GlobalInquery> result)
        {
            if (carThirdPartyInquiryObjects.validCompanies != null && carThirdPartyInquiryObjects.validCompanies.Count > 0 &&
                siteSettingId.ToIntReturnZiro() > 0 && carThirdPartyInquiryObjects.currentProposalForm != null)
            {
                foreach (var company in carThirdPartyInquiryObjects.validCompanies)
                {
                    GlobalInquery newQueryItem = new GlobalInquery()
                    {
                        CompanyId = company.Id,
                        CreateDate = DateTime.Now,
                        GlobalInputInqueryId = globalInputInqueryId,
                        SiteSettingId = siteSettingId.Value,
                        ProposalFormId = carThirdPartyInquiryObjects.currentProposalForm.Id
                    };

                    long basePriceS1 = InquiryCalceS1(company.Id, carThirdPartyInquiryObjects, newQueryItem);

                    if (basePriceS1 > 0)
                        result.Add(newQueryItem);
                }
            }
        }

        long InquiryCalceS1(int companyId, CarThirdPartyInquiryObjects carThirdPartyInquiryObjects, GlobalInquery newQueryItem)
        {
            var foundRate = carThirdPartyInquiryObjects.ThirdPartyRates.OrderByDescending(t => t.Id).Where(t => t.ThirdPartyRateCompanies != null && t.ThirdPartyRateCompanies.Any(tt => tt.CompanyId == companyId)).FirstOrDefault();
            if (foundRate != null && carThirdPartyInquiryObjects.ThirdPartyFinancialCommitment != null && carThirdPartyInquiryObjects.ThirdPartyLifeCommitment != null)
            {
                GlobalInquiryItem newITem = new GlobalInquiryItem();
                newITem.CalcKey = "s1";
                newITem.GlobalInquiryId = newQueryItem.Id;
                newITem.Title = BMessages.BasePrice.GetEnumDisplayName();
                newITem.Price = RouteThisNumberIfConfigExist
                    (Math.Ceiling(
                            foundRate.Rate *
                            Convert.ToDecimal(carThirdPartyInquiryObjects.ThirdPartyFinancialCommitment.Price + carThirdPartyInquiryObjects.ThirdPartyLifeCommitment.Price)).ToLongReturnZiro(),
                            carThirdPartyInquiryObjects.RoundInquery
                    );
                if (newITem.Price > 0)
                    newQueryItem.GlobalInquiryItems.Add(newITem);
                return newITem.Price;
            }
            return 0;
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

        List<InqeryExteraParameter> fillExteraParamters(CarThirdPartyInquiryObjects carThirdPartyInquiryObjects)
        {
            List<InqeryExteraParameter> result = new List<InqeryExteraParameter>();

            if (carThirdPartyInquiryObjects.ThirdPartyDriverFinancialCommitment != null)
                result.Add(new InqeryExteraParameter() { Title = "تعهد مالی سال جاری", Value = carThirdPartyInquiryObjects.ThirdPartyFinancialCommitment.Price.ToString("###,###") });
            if (carThirdPartyInquiryObjects.ThirdPartyLifeCommitment != null)
                result.Add(new InqeryExteraParameter() { Title = "تعهد جانی سال جاری", Value = carThirdPartyInquiryObjects.ThirdPartyLifeCommitment.Price.ToString("###,###") });
            if (carThirdPartyInquiryObjects.ThirdPartyFinancialCommitment != null)
                result.Add(new InqeryExteraParameter() { Title = "تعهد راننده سال جاری", Value = carThirdPartyInquiryObjects.ThirdPartyDriverFinancialCommitment.Price.ToString("###,###") });

            return result;
        }

        CarThirdPartyInquiryObjects fillRequiredObjectsAndValidate(CarThirdPartyInquiryVM input, int? siteSettingId)
        {
            CarThirdPartyInquiryObjects result = new CarThirdPartyInquiryObjects();

            if (input.exteraQuestions != null)
                input.exteraQuestions = input.exteraQuestions.Where(t => t.id.ToIntReturnZiro() > 0 && !string.IsNullOrEmpty(t.value)).ToList();

            result.currentProposalForm = ProposalFormManager.GetByType(ProposalFormType.ThirdParty, siteSettingId);
            if (result.currentProposalForm == null)
                throw BException.GenerateNewException(BMessages.ProposalForm_Not_Founded);

            result.validCompanies = InquiryCompanyLimitManager.GetCompanies(siteSettingId, InquiryCompanyLimitType.ThirdParty);
            if (result.validCompanies != null && result.validCompanies.Count > 0 && input.comIds != null && input.comIds.Count > 0)
                result.validCompanies = result.validCompanies.Where(t => input.comIds.Contains(t.Id)).ToList();


            if (result.validCompanies != null && result.validCompanies.Count > 0)
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

                initVicleTypeObjectAndValidation(result, input);
                initVeicleBrandObjectAndValidation(input);
                initVehicleUsageObjectAndValidation(result, input);
                initCreateYearInputObjectAndValidation(input);
                initRequiredCtrlsObjectAndValidation(result, input);
                initNoDamageDiscountObjAndValidation(result, input);
                initThirdPartyDriverNoDamageDiscountHistoryObjectAndValidation(result, input);
                initThirdPartyFinancialAndBodyHistoryDamagePenaltyObjectAndValidation(result, input);
                initThirdPartyDriverHistoryDamagePenaltyObjectAndValidation(result, input);
                initShowStatusObjectAndValidation(input, siteSettingId, result);
                initThirdPartyRatesObjectAndValidate(result, result.VehicleType?.CarSpecificationId);
                initThirdPartyRequiredFinancialCommitment(result, input);
                initExteraDiscountRightFilterObjectAndValidation(result, input);
                initInsuranceContractDiscountObjectAndValidation(result, input, siteSettingId);
                initInquiryDurationObjectAndValidation(result, input, siteSettingId);
                result.ThirdPartyFinancialCommitment = ThirdPartyFinancialCommitmentManager.GetLastActiveItem();
                result.ThirdPartyLifeCommitment = ThirdPartyLifeCommitmentManager.GetLastActiveItem();
                result.ThirdPartyDriverFinancialCommitment = ThirdPartyDriverFinancialCommitmentManager.GetLastActiveItem();
                result.ThirdPartyPassengerRates = ThirdPartyPassengerRateManager.GetActiveItems();
                result.RoundInquery = RoundInqueryManager.GetBySiteSettingAndProposalForm(siteSettingId, result.currentProposalForm?.Id);
                result.CarSpecification = CarSpecificationManager.GetById(result.VehicleType?.CarSpecificationId);
                result.InqueryDescriptions = InqueryDescriptionManager.GetByProposalFormId(result.currentProposalForm?.Id, siteSettingId);
                result.Tax = TaxManager.GetLastItem();
                result.Duty = DutyManager.GetLastItem();
                result.ThirdPartyCarCreateDatePercents = ThirdPartyCarCreateDatePercentManager.GetActiveList();
                result.InquiryMaxDiscounts = InquiryMaxDiscountManager.GetByFormAndSiteSettingId(result.currentProposalForm?.Id, siteSettingId);
                result.GlobalDiscounts = GlobalDiscountManager.GetAutoDiscounts(result.currentProposalForm?.Id, siteSettingId);
                result.CashPayDiscounts = CashPayDiscountManager.GetBySiteSettingAndProposalFormId(siteSettingId, result.currentProposalForm?.Id);
                result.PaymentMethods = PaymentMethodManager.GetList(siteSettingId, result.currentProposalForm?.Id);
                if (result.ThirdPartyDriverFinancialCommitment != null)
                    result.ThirdPartyDriverFinancialCommitmentLong = result.ThirdPartyDriverFinancialCommitment.Price;
                if (result.ThirdPartyLifeCommitment != null)
                    result.ThirdPartyLifeCommitmentLong = result.ThirdPartyLifeCommitment.Price;
                if (result.ThirdPartyFinancialCommitment != null)
                    result.ThirdPartyFinancialCommitmentLong = result.ThirdPartyFinancialCommitment.Price;
            }
            else
                throw BException.GenerateNewException(BMessages.No_Company_Exist);

            return result;
        }


        void initInquiryDurationObjectAndValidation(CarThirdPartyInquiryObjects result, CarThirdPartyInquiryVM input, int? siteSettingId)
        {
            if (input.dayLimitation.ToIntReturnZiro() > 0)
            {
                result.InquiryDuration = InquiryDurationManager.GetById(siteSettingId, result?.currentProposalForm?.Id, input.dayLimitation);

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

        void initInsuranceContractDiscountObjectAndValidation(CarThirdPartyInquiryObjects result, CarThirdPartyInquiryVM input, int? siteSettingId)
        {
            if (input.discountContractId.ToIntReturnZiro() > 0)
            {
                result.InsuranceContractDiscount = InsuranceContractDiscountManager.GetById(siteSettingId, result.currentProposalForm?.Id, input.discountContractId);
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

        void initExteraDiscountRightFilterObjectAndValidation(CarThirdPartyInquiryObjects result, CarThirdPartyInquiryVM input)
        {
            if (input.exteraQuestions != null && input.exteraQuestions.Count > 0)
            {
                var allExtraDiscountCoverIDs = input.exteraQuestions.Select(t => t.id).ToList();
                result.rightOptionDynamicFilterCTRLs = CarExteraDiscountManager.GetOptionSelectedCtrls(allExtraDiscountCoverIDs, result.currentProposalForm?.Id);


                if (result.rightOptionDynamicFilterCTRLs == null || result.rightOptionDynamicFilterCTRLs.Count == 0)
                    throw BException.GenerateNewException(BMessages.Validation_Error);
                if (result.rightOptionDynamicFilterCTRLs.Count != input.exteraQuestions.Count)
                    throw BException.GenerateNewException(BMessages.Validation_Error);
            }
            else
                input.exteraQuestions = null;
        }

        void initThirdPartyRequiredFinancialCommitment(CarThirdPartyInquiryObjects result, CarThirdPartyInquiryVM input)
        {
            if (input.coverIds != null && input.coverIds.Count > 0)
            {
                result.ThirdPartyRequiredFinancialCommitments = ThirdPartyRequiredFinancialCommitmentManager.GetByIds(input.coverIds);

                if (result.ThirdPartyRequiredFinancialCommitments == null || result.ThirdPartyRequiredFinancialCommitments.Count == 0)
                    throw BException.GenerateNewException(BMessages.Validation_Error);
                if (input.coverIds.Count != result.ThirdPartyRequiredFinancialCommitments.Count)
                    throw BException.GenerateNewException(BMessages.Validation_Error);
                input.coverIds_Title = new List<string>();
                foreach (var ec in result.ThirdPartyRequiredFinancialCommitments)
                    input.coverIds_Title.Add(ec.Title);
            }
            else
            {
                input.coverIds_Title = null;
                input.coverIds = null;
            }
        }

        void initThirdPartyRatesObjectAndValidate(CarThirdPartyInquiryObjects result, int? CarSpecificationId)
        {
            result.ThirdPartyRates = db.ThirdPartyRates.Include(t => t.ThirdPartyRateCompanies).Where(t => t.IsActive == true && t.CarSpecificationId == CarSpecificationId).OrderByDescending(t => t.Year).ToList();
        }

        void initShowStatusObjectAndValidation(CarThirdPartyInquiryVM input, int? siteSettingId, CarThirdPartyInquiryObjects result)
        {
            if (input.showStatus.ToIntReturnZiro() > 0)
            {
                var allValidStatus = EnumManager.GetEnum("PriceDiscountStatus");
                if (!allValidStatus.Any(t => t.id == input.showStatus + ""))
                    throw BException.GenerateNewException(BMessages.Payment_Method_Is_Not_Valid);
                input.showStatus_Title = allValidStatus.Where(t => t.id == input.showStatus + "").Select(t => t.title).FirstOrDefault();
            }
            else
            {
                input.showStatus_Title = null;
                input.showStatus = null;
                if (CashPayDiscountManager.HasAnyDebitPayment(siteSettingId, result.currentProposalForm?.Id))
                    throw BException.GenerateNewException(BMessages.Please_Select_Payment_Method);
            }
        }

        void initThirdPartyDriverHistoryDamagePenaltyObjectAndValidation(CarThirdPartyInquiryObjects result, CarThirdPartyInquiryVM input)
        {
            if (input.driverDamageHistoryId.ToIntReturnZiro() > 0)
            {
                result.ThirdPartyDriverHistoryDamagePenalty = ThirdPartyDriverHistoryDamagePenaltyManager.GetById(input.driverDamageHistoryId);
                if (result.ThirdPartyDriverHistoryDamagePenalty == null)
                    throw BException.GenerateNewException(BMessages.Driver_DamageHistory_Is_Not_Valid);
                input.driverDamageHistoryId_Title = result.ThirdPartyDriverHistoryDamagePenalty.Title;
            }
            else
            {
                input.driverDamageHistoryId_Title = null;
                input.driverDamageHistoryId = null;
            }
        }

        void initThirdPartyFinancialAndBodyHistoryDamagePenaltyObjectAndValidation(CarThirdPartyInquiryObjects result, CarThirdPartyInquiryVM input)
        {

            if (input.bodyDamageHistoryId.ToIntReturnZiro() > 0)
            {
                result.ThirdPartyFinancialAndBodyHistoryDamagePenaltyNotFinancial = ThirdPartyFinancialAndBodyHistoryDamagePenaltyManager.GetById(input.bodyDamageHistoryId, false);
                if (result.ThirdPartyFinancialAndBodyHistoryDamagePenaltyNotFinancial == null)
                    throw BException.GenerateNewException(BMessages.BodyDamageHistory_Not_Valid);
                input.bodyDamageHistoryId_Title = result.ThirdPartyFinancialAndBodyHistoryDamagePenaltyNotFinancial.Title;
            }
            else
            {
                input.bodyDamageHistoryId_Title = null;
                input.bodyDamageHistoryId = null;
            }

            if (input.financialDamageHistoryId.ToIntReturnZiro() > 0)
            {
                result.ThirdPartyFinancialAndBodyHistoryDamagePenaltyFinancial = ThirdPartyFinancialAndBodyHistoryDamagePenaltyManager.GetById(input.financialDamageHistoryId, true);
                if (result.ThirdPartyFinancialAndBodyHistoryDamagePenaltyFinancial == null)
                    throw BException.GenerateNewException(BMessages.FinancialDamageHistory_Not_Valid);
                input.financialDamageHistoryId_Title = result.ThirdPartyFinancialAndBodyHistoryDamagePenaltyFinancial.Title;
            }
            else
            {
                input.financialDamageHistoryId_Title = null;
                input.financialDamageHistoryId = null;
            }
        }

        void initCreateYearInputObjectAndValidation(CarThirdPartyInquiryVM input)
        {
            if (input.createYear != null)
            {
                if (input.createYear.ToIntReturnZiro() > DateTime.Now.Year || input.createYear.ToIntReturnZiro() < DateTime.Now.Year - 40)
                    throw BException.GenerateNewException(BMessages.Invalid_Date);
                var yearList = DateTime.Now.FromYear(40);
                var foundCreateYear = yearList.Where(t => t.id == input.createYear + "").FirstOrDefault();
                input.createYear_Title = foundCreateYear.title;
            }
            else
            {
                input.createYear_Title = null;
                input.createYear = null;
            }
        }

        void initVehicleUsageObjectAndValidation(CarThirdPartyInquiryObjects result, CarThirdPartyInquiryVM input)
        {
            result.VehicleUsage = VehicleUsageManager.GetById(input.cUsage);
            if (result.VehicleUsage == null)
                throw BException.GenerateNewException(BMessages.Invalid_VehicleUsage);
            input.cUsage_Title = result.VehicleUsage.Title;
        }

        void initVeicleBrandObjectAndValidation(CarThirdPartyInquiryVM input)
        {
            string brandTitle = VehicleSystemManager.GetTitleById(input.brandId);
            if (string.IsNullOrEmpty(brandTitle))
                throw BException.GenerateNewException(BMessages.Please_Select_VehicleBrand);
            input.brandId_Title = brandTitle;
        }

        void initVicleTypeObjectAndValidation(CarThirdPartyInquiryObjects result, CarThirdPartyInquiryVM input)
        {
            result.VehicleType = VehicleTypeManager.GetById(input.vType);
            if (result.VehicleType == null)
                throw BException.GenerateNewException(BMessages.Validation_Error);
            input.vType_Title = result.VehicleType.Title;
        }

        void initThirdPartyDriverNoDamageDiscountHistoryObjectAndValidation(CarThirdPartyInquiryObjects result, CarThirdPartyInquiryVM input)
        {
            if (input.driverNoDamageDiscountHistory.ToIntReturnZiro() > 0)
            {
                result.ThirdPartyDriverNoDamageDiscountHistory = ThirdPartyDriverNoDamageDiscountHistoryManager.GetById(input.driverNoDamageDiscountHistory);
                if (result.ThirdPartyDriverNoDamageDiscountHistory == null)
                    throw BException.GenerateNewException(BMessages.Driver_NoDamage_Discount_IsNotValid);
                input.driverNoDamageDiscountHistory_Title = result.ThirdPartyDriverNoDamageDiscountHistory.Title;
            }
            else
            {
                input.driverNoDamageDiscountHistory_Title = null;
                input.driverNoDamageDiscountHistory = null;
            }
        }

        void initNoDamageDiscountObjAndValidation(CarThirdPartyInquiryObjects result, CarThirdPartyInquiryVM input)
        {
            if (input.bodyNoDamagePercentId.ToIntReturnZiro() > 0)
            {
                result.bodyNoDamagePercent = ThirdPartyBodyNoDamageDiscountHistoryManager.GetByIdActived(input.bodyNoDamagePercentId);
                if (result.bodyNoDamagePercent == null)
                    throw BException.GenerateNewException(BMessages.Body_NoDamage_Discount_IsNotValid);
                input.bodyNoDamagePercentId_Title = result.bodyNoDamagePercent.Title;
            }
            else
            {
                input.bodyNoDamagePercentId_Title = null;
                input.bodyNoDamagePercentId = null;
            }
        }

        void initIfHavePrevInsurance(CarThirdPartyInquiryObjects result, CarThirdPartyInquiryVM input)
        {
            result.prevInsuranceCompany = CompanyManager.GetById(input.havePrevInsurance);
            if (result.prevInsuranceCompany == null)
                throw BException.GenerateNewException(BMessages.Selected_Company_Is_Not_Valid);
            input.havePrevInsurance_Title = result.prevInsuranceCompany.Title;
            input.isNewCar = 0;
            if (string.IsNullOrEmpty(input.prevEndDate) || input.prevEndDate.ConvertPersianNumberToEnglishNumber().ToEnDate() == null)
                throw BException.GenerateNewException(BMessages.Please_Enter_Previus_Insurance_Expired_Date);
            if (string.IsNullOrEmpty(input.prevStartDate) || input.prevStartDate.ConvertPersianNumberToEnglishNumber().ToEnDate() == null)
                throw BException.GenerateNewException(BMessages.Please_Enter_Previus_Insurance_Start_Date);
            if (input.bodyNoDamagePercentId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Enter_Body_NoDamage_Discount_Percent);
            if (input.driverNoDamageDiscountHistory.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Enter_Driver_NoDamage_Discount_Percent);
            if (!string.IsNullOrEmpty(input.prevEndDate) && !string.IsNullOrEmpty(input.prevStartDate) &&
                input.prevEndDate.ConvertPersianNumberToEnglishNumber().ToEnDate() != null && input.prevStartDate.ConvertPersianNumberToEnglishNumber().ToEnDate() != null)
            {
                var defDay = (input.prevEndDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value - input.prevStartDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value).TotalDays;
                if (defDay <= 0)
                    throw BException.GenerateNewException(BMessages.StartDate_Should_Be_Less_Then_EndDate);
                if (defDay <= 364)
                    result.usMinus5ForNoDamageDiscount = true;
            }
        }

        void initRequiredCtrlsObjectAndValidation(CarThirdPartyInquiryObjects result, CarThirdPartyInquiryVM input)
        {
            result.requiredValidDynamicCTRLs = CarExteraDiscountManager.GetRequredValidCTRLs(result?.currentProposalForm?.Id, input.brandId.ToIntReturnZiro(), input.havePrevInsurance > 0 ? true : false);
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

        void cleanInputRelatedToDontHavePrevInsurance(CarThirdPartyInquiryVM input)
        {
            input.bodyDamageHistoryId = null;
            input.bodyDamageHistoryId_Title = null;
            input.financialDamageHistoryId = null;
            input.financialDamageHistoryId_Title = null;
            input.driverDamageHistoryId = null;
            input.driverDamageHistoryId_Title = null;
            input.bodyNoDamagePercentId = null;
            input.bodyNoDamagePercentId_Title = null;
            input.driverNoDamageDiscountHistory = null;
            input.driverNoDamageDiscountHistory_Title = null;
            input.prevEndDate = null;
            input.prevStartDate = null;
            input.havePrevInsurance_Title = "ندارم";
        }

        bool IsValidInquiry(CarThirdPartyInquiryVM input)
        {
            return input != null && input.brandId.ToIntReturnZiro() > 0 && input.vType.ToIntReturnZiro() > 0 && input.cUsage.ToIntReturnZiro() > 0;
        }
    }
}
