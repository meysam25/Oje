using Oje.Infrastructure.Models;

namespace Oje.Section.RegisterForm.Models.View
{
    public class UserRegisterFormRequiredDocumentTypeCreateUpdateVM: GlobalSiteSetting
    {
        public int? id { get; set; }
        public string title { get; set; }
        public int? userRegisterFormId { get; set; }
        public bool? isActive { get; set; }
    }
}
