namespace Oje.PaymentService.Models.View.Sep
{
    public class VerifyInfo
    {
        public string RRN { get; set; }
        public string RefNum { get; set; }
        public string MaskedPan { get; set; }
        public string HashedPan { get; set; }
        public long? TerminalNumber { get; set; }
        public long? OrginalAmount { get; set; }
        public long? AffectiveAmount { get; set; }
        public string StraceDate { get; set; }
        public string StraceNo { get; set; }
    }
}
