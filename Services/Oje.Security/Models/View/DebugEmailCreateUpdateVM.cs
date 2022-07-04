
namespace Oje.Security.Models.View
{
    public class DebugEmailCreateUpdateVM
    {
        public string username { get; set; }
        public string password { get; set; }
        public int? smtpPort { get; set; }
        public string smtpHost { get; set; }
        public bool? isSSL { get; set; }
        public bool? isDefaultCredentials { get; set; }
        public int? timeOut { get; set; }
        public bool? isActive { get; set; }
    }
}
