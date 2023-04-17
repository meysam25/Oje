using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Sanab.Interfaces;
using Oje.Sanab.Models.DB;
using Oje.Sanab.Models.View;
using Oje.Sms.Services.EContext;

namespace Oje.Sanab.Services
{
    public class SanabCarThirdPartyPlaqueInquiryService : ISanabCarThirdPartyPlaqueInquiryService
    {
        readonly SanabDBContext db = null;
        readonly ICarInquiry CarInquiry = null;
        readonly ISanabCompanyService SanabCompanyService = null;
        readonly ISanabSystemFieldVehicleSystemService SanabSystemFieldVehicleSystemService = null;
        readonly ISanabTypeFieldVehicleSpecService SanabTypeFieldVehicleSpecService = null;
        readonly ISanabVehicleTypeService SanabVehicleTypeService = null;
        readonly ISanabCarTypeService SanabCarTypeService = null;
        readonly IThirdPartyFinancialAndBodyHistoryDamagePenaltyService ThirdPartyFinancialAndBodyHistoryDamagePenaltyService = null;
        readonly IThirdPartyDriverHistoryDamagePenaltyService ThirdPartyDriverHistoryDamagePenaltyService = null;
        readonly IThirdPartyBodyNoDamageDiscountHistoryService ThirdPartyBodyNoDamageDiscountHistoryService = null;
        readonly IThirdPartyDriverNoDamageDiscountHistoryService ThirdPartyDriverNoDamageDiscountHistoryService = null;

        public SanabCarThirdPartyPlaqueInquiryService
            (
                SanabDBContext db,
                ICarInquiry CarInquiry,
                ISanabCompanyService SanabCompanyService,
                ISanabSystemFieldVehicleSystemService SanabSystemFieldVehicleSystemService,
                ISanabTypeFieldVehicleSpecService SanabTypeFieldVehicleSpecService,
                ISanabVehicleTypeService SanabVehicleTypeService,
                ISanabCarTypeService SanabCarTypeService,
                IThirdPartyFinancialAndBodyHistoryDamagePenaltyService ThirdPartyFinancialAndBodyHistoryDamagePenaltyService,
                IThirdPartyDriverHistoryDamagePenaltyService ThirdPartyDriverHistoryDamagePenaltyService,
                IThirdPartyBodyNoDamageDiscountHistoryService ThirdPartyBodyNoDamageDiscountHistoryService,
                IThirdPartyDriverNoDamageDiscountHistoryService ThirdPartyDriverNoDamageDiscountHistoryService
            )
        {
            this.db = db;
            this.CarInquiry = CarInquiry;
            this.SanabCompanyService = SanabCompanyService;
            this.SanabSystemFieldVehicleSystemService = SanabSystemFieldVehicleSystemService;
            this.SanabTypeFieldVehicleSpecService = SanabTypeFieldVehicleSpecService;
            this.SanabVehicleTypeService = SanabVehicleTypeService;
            this.SanabCarTypeService = SanabCarTypeService;
            this.ThirdPartyFinancialAndBodyHistoryDamagePenaltyService = ThirdPartyFinancialAndBodyHistoryDamagePenaltyService;
            this.ThirdPartyDriverHistoryDamagePenaltyService = ThirdPartyDriverHistoryDamagePenaltyService;
            this.ThirdPartyBodyNoDamageDiscountHistoryService = ThirdPartyBodyNoDamageDiscountHistoryService;
            this.ThirdPartyDriverNoDamageDiscountHistoryService = ThirdPartyDriverNoDamageDiscountHistoryService;
        }

        public async Task<List<KeyValue>> Discount(int? siteSettingId, CarThirdPartyPlaqueVM input, IpSections ipSections)
        {
            discountValidation(siteSettingId, input, ipSections);

            var plaque_1 = input.plaque_1.ToIntReturnZiro();
            var plaque_2 = input.plaque_2.ToIntReturnZiro();
            var plaque_3 = PlaqueService.ConvertStringToPlaque(input.plaque_3);
            var plaque_4 = input.plaque_4.ToIntReturnZiro();
            var prevDayte = DateTime.Now.AddMonths(-5);


            var foundItem = await db.SanabCarThirdPartyPlaqueInquiries
                .Where(t => t.NationalCode == input.nationalCode && t.plaque_1 == plaque_1 && t.plaque_2 == plaque_2 && t.plaque_3 == plaque_3 && t.plaque_4 == plaque_4 && t.CreateDate >= prevDayte)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            if (foundItem != null)
                return convertInquiryDbModelToViewModel(foundItem);

            var tempResult = await CarInquiry.CarDiscount(siteSettingId, input.plaque_1, input?.plaque_4, PlaqueService.ConvertStringToPlaque(input?.plaque_3) + "", input?.plaque_2, input.nationalCode);
            if (tempResult != null)
                return convertInquiryDbModelToViewModel(createFromApi(tempResult, input, ipSections));

            return new List<KeyValue>();
        }

        private List<KeyValue> convertInquiryDbModelToViewModel(SanabCarThirdPartyPlaqueInquiry foundItem)
        {
            List<KeyValue> result = new List<KeyValue>();

            if (foundItem != null)
            {
                var foundBrandItem = SanabSystemFieldVehicleSystemService.GetSystem(foundItem.VehicleSystemCode);
                var foundSpec = SanabTypeFieldVehicleSpecService.GetSpec(foundItem.TypeByCii);
                result.Add(new KeyValue() { key = "vehicleTypeId", value = SanabVehicleTypeService.GetTypeIdBy(foundItem.CarTypCodeByCii.ToIntReturnZiro()) + "" });
                result.Add(new KeyValue() { key = "carTypeId", value = SanabCarTypeService.GetTypeIdBy(foundItem.UsgCodByCii.ToIntReturnZiro()) + "" });
                result.Add(new KeyValue() { key = "brandId", value = foundBrandItem?.Id + "" });
                result.Add(new KeyValue() { key = "brandId_Title", value = foundBrandItem?.Title + "" });
                result.Add(new KeyValue() { key = "specId", value = foundSpec?.Id + "" });
                result.Add(new KeyValue() { key = "specId_Title", value = foundSpec?.Title + "" });
                result.Add(new KeyValue() { key = "createYear", value = foundItem.ModelbyNaja });
                result.Add(new KeyValue() { key = "havePrevInsurance", value = SanabCompanyService.GetCompanyId(foundItem.CompanyId) + "" });
                result.Add(new KeyValue() { key = "prevStartDate", value = foundItem.BeginDate + "" });
                result.Add(new KeyValue() { key = "prevEndDate", value = foundItem.EndDate + "" });
                result.Add(new KeyValue() { key = "bodyDamageHistoryId", value = ThirdPartyFinancialAndBodyHistoryDamagePenaltyService.GetNotFinancialIdBy(foundItem.DiscountLifeYearNumber.ToIntReturnZiro()) });
                result.Add(new KeyValue() { key = "financialDamageHistoryId", value = ThirdPartyFinancialAndBodyHistoryDamagePenaltyService.GetFinancialIdBy(foundItem.DiscountFinancialYearNumber.ToIntReturnZiro()) });
                result.Add(new KeyValue() { key = "driverDamageHistoryId", value = ThirdPartyDriverHistoryDamagePenaltyService.GetIdBy(foundItem.DiscountPersonYearNumber.ToIntReturnZiro()) });
                result.Add(new KeyValue() { key = "bodyNoDamagePercentId", value = ThirdPartyBodyNoDamageDiscountHistoryService.GetIdBy(foundItem.DiscountFinancialYearPercent) });
                result.Add(new KeyValue() { key = "driverNoDamageDiscountHistory", value = ThirdPartyDriverNoDamageDiscountHistoryService.GetIdBy(foundItem.DiscountPersonYearPercent) });
            }

            return result;
        }

        string getCreateDate(string ModelField)
        {
            if (!string.IsNullOrEmpty(ModelField))
            {
                if (ModelField.ToIntReturnZiro() > 1850 && !string.IsNullOrEmpty((ModelField + "/1/1").ToFaDate())) // foreign
                    return ModelField + "-" + (ModelField + "/1/1").ToFaDate().Split('/')[0];
                else if ((ModelField + "/1/1").ToEnDate() != null) //iran
                    return (ModelField + "/1/1").ToEnDate().Value.ToString("yyyy") + "-" + ModelField;
            }

            return "";
        }

        private SanabCarThirdPartyPlaqueInquiry createFromApi(CarDiscountResultVM tempResult, CarThirdPartyPlaqueVM input, IpSections ipSections)
        {
            SanabCarThirdPartyPlaqueInquiry result = null;
            if (tempResult != null && input != null && ipSections != null)
            {
                result = new SanabCarThirdPartyPlaqueInquiry()
                {
                    CreateDate = DateTime.Now,
                    EndDate = tempResult.EndDate,
                    Ip1 = ipSections.Ip1,
                    Ip2 = ipSections.Ip2,
                    Ip3 = ipSections.Ip3,
                    Ip4 = ipSections.Ip4,
                    NationalCode = input.nationalCode,
                    plaque_1 = input.plaque_1.ToIntReturnZiro(),
                    plaque_2 = input.plaque_2.ToIntReturnZiro(),
                    plaque_3 = PlaqueService.ConvertStringToPlaque(input.plaque_3).ToIntReturnZiro(),
                    plaque_4 = input.plaque_4.ToIntReturnZiro(),
                    AxelNo = tempResult.AxelNo,
                    BeginDate = tempResult.BeginDate,
                    Capacity = tempResult.Capacity,
                    CarGroupCode = tempResult.CarGroupCode,
                    CarTypCodeByCii = tempResult.CarTypCodeByCii,
                    CarTypeNameByCii = tempResult.CarTypeNameByCii,
                    ChassisNumber = tempResult.ChassisNumber,
                    CompanyId = tempResult.CompanyId,
                    CompanyTitle = tempResult.CompanyTitle,
                    CylinderNo = tempResult.CylinderNo,
                    DiscountFinancialYearNumber = tempResult.DiscountFinancialYearNumber,
                    DiscountFinancialYearPercent = tempResult.DiscountFinancialYearPercent,
                    DiscountLifeYearNumber = tempResult.DiscountLifeYearNumber,
                    DiscountLifeYearPercent = tempResult.DiscountLifeYearPercent,
                    DiscountPersonYearNumber = tempResult.DiscountPersonYearNumber,
                    DiscountPersonYearPercent = tempResult.DiscountPersonYearPercent,
                    EngineNumber = tempResult.EngineNumber,
                    MainColor = tempResult.MainColor,
                    ModelbyNaja = tempResult.ModelbyNaja,
                    PlateInstallDate = tempResult.PlateInstallDate,
                    PrintNumber = tempResult.PrintNumber,
                    SecondColor = tempResult.SecondColor,
                    SubUsage = tempResult.SubUsage,
                    SystemByCii = tempResult.SystemByCii,
                    SystemByNaja = tempResult.SystemByNaja,
                    TypeByCii = tempResult.TypeByCii,
                    TypeByNaja = tempResult.TypeByNaja,
                    TypeCodeByCii = tempResult.TypeCodeByCii,
                    UniqueCode = tempResult.UniqueCode,
                    UsageByNaja = tempResult.UsageByNaja,
                    UsageCodeByCii = tempResult.UsageCodeByCii,
                    UsageNameByCii = tempResult.UsageNameByCii,
                    UsgCodByCii = tempResult.UsgCodByCii,
                    VehicleSystemCode = tempResult.VehicleSystemCode,
                    Vin = tempResult.Vin,
                    WheelNo = tempResult.WheelNo
                };
                db.Entry(result).State = EntityState.Added;
                db.SaveChanges();
            }

            return result;
        }

        private void discountValidation(int? siteSettingId, CarThirdPartyPlaqueVM input, IpSections ipSections)
        {
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (string.IsNullOrEmpty(input.nationalCode))
                throw BException.GenerateNewException(BMessages.Please_Enter_NationalCode);
            if (!input.nationalCode.IsCodeMeli())
                throw BException.GenerateNewException(BMessages.Invalid_NationaCode);
            if (string.IsNullOrEmpty(input.plaque_1))
                throw BException.GenerateNewException(BMessages.Invalid_Plaque);
            if (string.IsNullOrEmpty(input.plaque_2))
                throw BException.GenerateNewException(BMessages.Invalid_Plaque);
            if (string.IsNullOrEmpty(input.plaque_3))
                throw BException.GenerateNewException(BMessages.Invalid_Plaque);
            if (string.IsNullOrEmpty(input.plaque_4))
                throw BException.GenerateNewException(BMessages.Invalid_Plaque);
            if (input.plaque_4.Length > 2 || input.plaque_4.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Invalid_Plaque);
            if (input.plaque_2.Length > 3 || input.plaque_2.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Invalid_Plaque);
            if (input.plaque_1.Length > 2 || input.plaque_1.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Invalid_Plaque);
            if (PlaqueService.ConvertStringToPlaque(input.plaque_3) == null)
                throw BException.GenerateNewException(BMessages.Invalid_Plaque);
            if (ipSections == null)
                throw BException.GenerateNewException(BMessages.Validation_Error);
        }
    }
}
