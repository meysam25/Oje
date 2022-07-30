using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;

namespace Oje.Section.RegisterForm.Models.View
{
    public class UserRegisterFormPrintDescrptionMainGrid: GlobalGrid
    {
        public string fTitle { get; set; }
        public ProposalFormPrintDescrptionType? type { get; set; }
        public bool? isActive { get; set; }
    }
}
