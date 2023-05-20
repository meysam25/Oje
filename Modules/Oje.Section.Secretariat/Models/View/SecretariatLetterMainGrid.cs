using Oje.Infrastructure.Models;

namespace Oje.Section.Secretariat.Models.View
{
    public class SecretariatLetterMainGrid: GlobalGrid
    {
        public string title { get; set; }
        public string subTitle { get; set; }
        public string subject { get; set; }
        public string user { get; set; }
        public string sUser { get; set; }
        public string createDate { get; set; }
    }
}
