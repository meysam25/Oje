using Microsoft.AspNetCore.Http;

namespace Oje.Section.WebMain.Models.View
{
    public class LoginBackgroundImageCreateUpdateVM
    {
        public int? id { get; set; }
        public string title { get; set; }
        public IFormFile mainImage { get; set; }
        public bool? isActive { get; set; }
    }
}
