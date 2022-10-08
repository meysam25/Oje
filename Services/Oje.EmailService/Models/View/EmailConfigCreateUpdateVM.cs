using Oje.Infrastructure.Models;

namespace Oje.EmailService.Models.View
{
    public class EmailConfigCreateUpdateVM: GlobalSiteSetting
    {
        public int? id { get; set; }
        public string title { get; set; }
        public string eUsername { get; set; }
        public string ePassword { get; set; }
        public int? smtpPort { get; set; }
        public string smtpHost { get; set; }
        public bool? enableSsl { get; set; }
        public bool? useDefaultCredentials { get; set; }
        public int? timeout { get; set; }
        public bool? isActive { get; set; }
    }
}
