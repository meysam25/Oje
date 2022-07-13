
namespace Oje.PaymentService.Models.View.TiTec
{
    public class FactorShare
    {
        public byte SharePercent { get; set; }
        public long ShareValue { get; set; }
        public string ShareHolderName { get; set; }
        //Shaba
        public string iban { get; set; }
        public string MobileNo { get; set; }
    }
}
