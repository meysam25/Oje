using Oje.Infrastructure.Models;

namespace Oje.Section.RegisterForm.Models.View
{
    public class UserRegisterFormCompanyMainGrid: GlobalGrid
    {
        public string company { get; set; }
        public string form { get; set; }
        public bool? isActive { get; set; }
    }
}
