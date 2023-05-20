using Microsoft.AspNetCore.Http;

namespace Oje.Section.Secretariat.Models.View
{
    public class SecretariatHeaderFooterCreateUpdateVM
    {
        public int? id { get; set; }
        public string title { get; set; }
        public IFormFile header { get; set; }
        public IFormFile footer { get; set; }
    }
}
