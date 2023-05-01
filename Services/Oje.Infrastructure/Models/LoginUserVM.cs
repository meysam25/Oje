using System.Collections.Generic;

namespace Oje.Infrastructure.Models
{
    public class LoginUserVM
    {
        public long UserId { get; set; }
        public string Username { get; set; }
        public string Fullname { get; set; }
        public string Ip { get; set; }
        public int? siteSettingId { get; set; }
        public string sessionFileName { get; set; }
        public List<string> roles { get; set; }
        public string browserName { get; set; }
        public bool? hasAutoRefres { get; set; }
        public bool? canSeeOtherWebsites { get; set; }
        public string nationalCode { get; set; }
    }
}
