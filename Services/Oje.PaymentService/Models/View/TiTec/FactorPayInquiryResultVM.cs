
namespace Oje.PaymentService.Models.View.TiTec
{
    public class FactorPayInquiryResultVM
    {
        public long? amount { get; set; }
        public string customerName { get; set; }
        public string mobileNumber { get; set; }
        public string factorId { get; set; }
        public string factorNumber { get; set; }
        //Persian
        public string payDate { get; set; }
        public DateTime? payDateTime { get; set; }
        public bool? status { get; set; }
    }
}
