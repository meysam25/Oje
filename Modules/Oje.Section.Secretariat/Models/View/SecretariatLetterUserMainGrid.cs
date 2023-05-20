using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;

namespace Oje.Section.Secretariat.Models.View
{
    public class SecretariatLetterUserMainGrid: GlobalGridParentLong
    {
        public string userFullname { get; set; }
        public SecretariatLetterUserType? type { get; set; }
        public string createDate { get; set; }
        public string mobile { get; set; }
    }
}
