using Oje.Infrastructure.Models;

namespace Oje.Section.Secretariat.Models.View
{
    public class SecretariatLetterUserCreateVM: GlobalGridParentLong
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string mobile { get; set; }
    }
}
