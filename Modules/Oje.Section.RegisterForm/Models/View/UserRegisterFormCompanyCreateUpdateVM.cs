using Oje.Infrastructure.Models;

namespace Oje.Section.RegisterForm.Models.View
{
    public class UserRegisterFormCompanyCreateUpdateVM: GlobalSiteSetting
    {
        public int? id { get; set; }
        public int? cid { get; set; }
        public int? fid { get; set; }
        public bool? isActive { get; set; }
    }
}
