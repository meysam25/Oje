using Oje.Infrastructure.Enums;

namespace Oje.Section.InsuranceContractBaseData.Models.View
{
    public class MyFilledContractAddressVM
    {
        public long? id { get; set; }
        public ContractLocation? reciveLocation { get; set; }
        public string reciveDate { get; set; }
        public string reciveTime { get; set; }
        public string tell { get; set; }
        public string myAddressLocation { get; set; }
        public string address { get; set; }
        public decimal? mapLatRecivePlace { get; set; }
        public decimal? mapLonRecivePlace { get; set; }
        public byte? mapZoomRecivePlace { get; set; }
    }
}
