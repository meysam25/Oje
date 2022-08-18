namespace Oje.PaymentService.Models.View.Sep
{
    public class SepCallBack
    {
        public long? MID { get; set; }
        public string State { get; set; }
        public int? Status { get; set; }
        public string RRN { get; set; }
        public string RefNum { get; set; }
        public string ResNum { get; set; }
        public string TerminalId { get; set; }
        public string TraceNo { get; set; }
        public long? Amount { get; set; }
        public string SecurePan { get; set; }
        public string HashedCardNumber { get; set; }
    }
}
