using Microsoft.AspNetCore.Http;
using Oje.Infrastructure.Models;

namespace Oje.Section.RegisterForm.Models.View
{
    public class UserRegisterFormRequiredDocumentCreateUpdateVM: GlobalSiteSetting
    {
        public int? id { get; set; }
        public string title { get; set; }
        public int? typeId { get; set; }
        public bool? isRequird { get; set; }
        public bool? isActive { get; set; }
        public IFormFile downloadFile { get; set; }
    }
}
