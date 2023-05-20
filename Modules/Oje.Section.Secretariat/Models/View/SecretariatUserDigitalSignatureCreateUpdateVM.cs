using Microsoft.AspNetCore.Http;

namespace Oje.Section.Secretariat.Models.View
{
    public class SecretariatUserDigitalSignatureCreateUpdateVM
    {
        public int? id { get; set; }
        public string title { get; set; }
        public string role { get; set; }
        public IFormFile signature { get; set; }
        public bool? isActive { get; set; }
        public long? userId { get; set; }
    }
}
