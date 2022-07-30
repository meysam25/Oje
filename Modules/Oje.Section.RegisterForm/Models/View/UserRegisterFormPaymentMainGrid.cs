using Oje.Infrastructure.Models;

namespace Oje.Section.RegisterForm.Models.View
{
    public class UserRegisterFormPaymentMainGrid: GlobalGridParentLong
    {
        public string card { get; set; }
        public string refferCode { get; set; }
        public long? price { get; set; }
        public string pDate { get; set; }
    }
}
