using Oje.Infrastructure.Models;

namespace Oje.Section.RegisterForm.Models.View
{
    public class UserRegisterFormPriceMainGrid: GlobalGrid
    {
        public string formTitle { get; set; }
        public string title { get; set; }
        public long? price { get; set; }
        public bool? isActive { get; set; }
    }
}
