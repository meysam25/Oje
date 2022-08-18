using System.ComponentModel.DataAnnotations;

namespace Oje.Infrastructure.Enums
{
    public enum ProposalFilledFormUserType
    {
        [Display(Name = "کاربر ثبت کننده")]
        CreateUser = 1,
        [Display(Name = "مارکتر")]
        Marketer = 2,
        [Display(Name = "نماینده")]
        Agent = 3,
        [Display(Name = "ارجاع")]
        Refer = 4,
        [Display(Name = "مالک")]
        OwnerUser = 5
    }
}
