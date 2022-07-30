using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;

namespace Oje.Section.RegisterForm.Models.View
{
    public class UserRegisterFormPrintDescrptionCreateUpdateVM
    {
        public long? id { get; set; }
        public int? pfid { get; set; }
        public ProposalFormPrintDescrptionType? type { get; set; }
        [IgnoreStringEncode]
        public MyHtmlString description { get; set; }
        public bool? isActive { get; set; }
    }
}
