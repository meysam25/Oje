using Oje.Infrastructure.Models;

namespace Oje.Section.RegisterForm.Models.View
{
    public class UserRegisterFormDiscountCodeMainGrid: GlobalGrid
    {
        public string ppfTitle { get; set; }
        public string createDate { get; set; }
        public string title { get; set; }
        public string fromDate { get; set; }
        public string toDate { get; set; }
        public bool? isActive { get; set; }
    }
}
