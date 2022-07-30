namespace Oje.PaymentService.Models.View.Sadad
{
    public class PurchaseResult
    {
        public string OrderId { get; set; }
        public string HashedCardNo { get; set; }
        public string PrimaryAccNo { get; set; }
        public string SwitchResCode { get; set; }
        public string ResCode { get; set; }
        public string Token { get; set; }
    }
}
