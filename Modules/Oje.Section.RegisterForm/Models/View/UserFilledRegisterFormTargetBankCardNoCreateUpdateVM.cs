using System.ComponentModel.DataAnnotations;

namespace Oje.Section.RegisterForm.Models.View
{
    public class UserFilledRegisterFormTargetBankCardNoCreateUpdateVM
    {
        [Display(Name = "شماره کارت")]
        public string targetBankCardNo { get; set; }
    }
}
