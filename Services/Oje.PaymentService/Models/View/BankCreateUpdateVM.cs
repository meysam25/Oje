using Microsoft.AspNetCore.Http;

namespace Oje.PaymentService.Models.View
{
    public class BankCreateUpdateVM
    {
        public int? id { get; set; }
        public string title { get; set; }
        public int? code { get; set; }
        public bool? isActive { get; set; }
        public IFormFile minPic { get; set; }
    }
}
