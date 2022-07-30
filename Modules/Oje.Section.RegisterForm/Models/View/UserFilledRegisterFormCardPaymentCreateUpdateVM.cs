using Microsoft.AspNetCore.Http;

namespace Oje.Section.RegisterForm.Models.View
{
    public class UserFilledRegisterFormCardPaymentCreateUpdateVM
    {
        public long? fid { get; set; }
        public string card { get; set; }
        public string refferCode { get; set; }
        public long? price { get; set; }
        public string pDate { get; set; }
        public IFormFile mainImage { get; set; }
    }
}
