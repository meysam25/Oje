using Oje.Infrastructure.Models;

namespace Oje.ValidatedSignature.Models.View
{
    public class UserRegisterFormPriceMainGrid: GlobalGrid
    {
        public int? id { get; set; }
        public string title { get; set; }
        public long? price { get; set; }
        public bool? isActive { get; set; }
    }
}
