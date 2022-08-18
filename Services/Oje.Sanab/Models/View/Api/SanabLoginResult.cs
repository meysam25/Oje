namespace Oje.Sanab.Models.View.Api
{
    public class SanabLoginResult
    {
        public string UserId { get; set; }
        public long? CompanyId { get; set; }
        public bool? IsServiceUser { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public string UserName { get; set; }
        public bool? IsSucceed { get; set; }
        public long? LoginStatus { get; set; }
    }
}
