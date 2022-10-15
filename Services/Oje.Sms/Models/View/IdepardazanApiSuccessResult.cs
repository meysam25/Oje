namespace Oje.Sms.Models.View
{
    public class IdepardazanApiSuccessResult
    {
        public long? status { get; set; }
        public string message { get; set; }
        public IdepardazanApiSuccessResultData data { get; set; }

    }

    public class IdepardazanApiSuccessResultData
    {
        public string packId { get; set; }
        public List<int> messageIds { get; set; }
        public decimal? cost { get; set; }
    }
}
