namespace Oje.Security.Models.View
{
    public class ValidRangeIpCreateUpdateVM
    {
        public int? id { get; set; }
        public string title { get; set; }
        public string fromIp { get; set; }
        public string toIp { get; set; }
        public bool? isActive { get; set; }
    }
}
