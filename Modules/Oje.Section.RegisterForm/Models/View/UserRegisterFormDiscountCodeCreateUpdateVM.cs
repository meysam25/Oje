using Oje.Infrastructure.Models;

namespace Oje.Section.RegisterForm.Models.View
{
    public class UserRegisterFormDiscountCodeCreateUpdateVM: GlobalSiteSetting
    {
        public int? id { get; set; }
        public int? formId { get; set; }
        public string title { get; set; }
        public string fromDate { get; set; }
        public string toDate { get; set; }
        public string discountCode { get; set; }
        public int? percent { get; set; }
        public long? price { get; set; }
        public long? maxPrice { get; set; }
        public int? countUse { get; set; }
        public bool? isActive { get; set; }
    }
}
