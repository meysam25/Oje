using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Sanab.Models.DB
{
    [Table("SanabCarThirdPartyPlaqueInquiries")]
    public class SanabCarThirdPartyPlaqueInquiry
    {
        [Key]
        public long Id { get; set; }
        public DateTime CreateDate { get; set; }
        public byte Ip1 { get; set; }
        public byte Ip2 { get; set; }
        public byte Ip3 { get; set; }
        public byte Ip4 { get; set; }
        [Required, MaxLength(15)]
        public string NationalCode { get; set; }
        public int plaque_1 { get; set; }
        public int plaque_2 { get; set; }
        public int plaque_3 { get; set; }
        public int plaque_4 { get; set; }
        [MaxLength(100)]
        public string LastCompanyDocumentNumber { get; set; }
        public long? UsageCode { get; set; }
        public long? MapUsageCode { get; set; }
        [MaxLength(100)]
        public string MapUsageName { get; set; }
        public long? Plk1 { get; set; }
        public long? Plk2 { get; set; }
        public long? Plk3 { get; set; }
        public long? PlkSrl { get; set; }
        [MaxLength(100)]
        public string PrintEndorsCompanyDocumentNumber { get; set; }
        [MaxLength(100)]
        public string InsuranceFullName { get; set; }
        [MaxLength(100)]
        public string EndorseDate { get; set; }
        [MaxLength(100)]
        public string SystemField { get; set; }
        [MaxLength(100)]
        public string TypeField { get; set; }
        [MaxLength(100)]
        public string UsageField { get; set; }
        [MaxLength(100)]
        public string MainColorField { get; set; }
        [MaxLength(100)]
        public string SecondColorField { get; set; }
        [MaxLength(100)]
        public string ModelField { get; set; }
        [MaxLength(100)]
        public string CapacityField { get; set; }
        [MaxLength(100)]
        public string CylinderNumberField { get; set; }
        [MaxLength(100)]
        public string EngineNumberField { get; set; }
        [MaxLength(100)]
        public string ChassisNumberField { get; set; }
        [MaxLength(100)]
        public string VinNumberField { get; set; }
        [MaxLength(100)]
        public string InstallDateField { get; set; }
        [MaxLength(100)]
        public string AxelNumberField { get; set; }
        [MaxLength(100)]
        public string wheelNumberField { get; set; }
        [MaxLength(100)]
        public string CompanyName { get; set; }
        public int? CompanyCode { get; set; }
        [MaxLength(100)]
        public string IssueDate { get; set; }
        [MaxLength(100)]
        public string SatrtDate { get; set; }
        [MaxLength(100)]
        public string EndDate { get; set; }
        [MaxLength(100)]
        public string Thrname { get; set; }
        [MaxLength(100)]
        public string MapUsgCod { get; set; }
        [MaxLength(100)]
        public string MapUsgName { get; set; }
        [MaxLength(100)]
        public string CylinderCount { get; set; }
        [MaxLength(100)]
        public string EndorseText { get; set; }
        public int? PolicyHealthLossCount { get; set; }
        public int? PolicyFinancialLossCount { get; set; }
        public int? PolicyPersonLossCount { get; set; }
        public decimal? Tonage { get; set; }
        public long? ThirdPolicyCode { get; set; }
        public long? DiscountPersonPercent { get; set; }
        public long? DiscountThirdPercent { get; set; }
        [MaxLength(100)]
        public string PrntPlcyCmpDocNo { get; set; }
        [MaxLength(100)]
        public string plk { get; set; }
        [MaxLength(100)]
        public string MapTypNam { get; set; }
        [MaxLength(100)]
        public string MtrNum { get; set; }
        [MaxLength(100)]
        public string ShsNum { get; set; }
        [MaxLength(100)]
        public string HBgnDte { get; set; }
        [MaxLength(100)]
        public string HEndDte { get; set; }
        [MaxLength(100)]
        public string DisFnYrNum { get; set; }
        [MaxLength(100)]
        public string DisLfYrNum { get; set; }
        [MaxLength(100)]
        public string DisPrsnYrNum { get; set; }
        [MaxLength(100)]
        public string DisPrsnYrPrcnt { get; set; }
        [MaxLength(100)]
        public string DisFnYrPrcnt { get; set; }
        public long? PrsnCvrCptl { get; set; }
        public long? LfCvrCptl { get; set; }
        public long? FnCvrCptl { get; set; }
        [MaxLength(100)]
        public string LstCmpDocNo { get; set; }
        [MaxLength(100)]
        public string vin { get; set; }
        public int? UsgCod { get; set; }
        [MaxLength(100)]
        public string MapUsgNam { get; set; }
        public int? CylCnt { get; set; }
    }
}
