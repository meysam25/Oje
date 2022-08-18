namespace Oje.Security.Models.View
{
    public class IpDetectionServiceVM
    {
        public string ip { get; set; }
        public string type { get; set; }
        public string country_code { get; set; }
        public string country_name { get; set; }
        public string region_name { get; set; }
        public string city { get; set; }
        //public decimal? latitude { get; set; }
        //public decimal? longitude { get; set; }
    }
}
