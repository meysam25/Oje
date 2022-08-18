namespace Oje.PaymentService.Models.View.Sep
{
    public class VerifyResultData
    {
        public VerifyInfo TransactionDetail { get; set; }
        public int? ResultCode { get; set; }
        public string ResultDescription { get; set; }
        public bool Success { get; set; }
    }
}
