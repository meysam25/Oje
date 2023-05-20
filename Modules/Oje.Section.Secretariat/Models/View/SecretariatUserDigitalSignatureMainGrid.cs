using Oje.Infrastructure.Models;

namespace Oje.Section.Secretariat.Models.View
{
    public class SecretariatUserDigitalSignatureMainGrid: GlobalGrid
    {
        public string role { get; set; }
        public bool? isActive { get; set; }
        public string user { get; set; }
    }
}
