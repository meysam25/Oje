using Microsoft.AspNetCore.Http;
using Oje.Infrastructure.Models;

namespace Oje.Section.WebMain.Models.View
{
    public class LoginBackgroundImageCreateUpdateVM: GlobalSiteSetting
    {
        public int? id { get; set; }
        public string title { get; set; }
        public IFormFile mainImage { get; set; }
        public bool? isActive { get; set; }
    }
}
