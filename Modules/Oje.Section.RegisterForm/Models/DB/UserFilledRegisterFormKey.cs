using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.RegisterForm.Models.DB
{
    [Table("UserFilledRegisterFormKeys")]
    public class UserFilledRegisterFormKey
    {
        public UserFilledRegisterFormKey()
        {
            UserFilledRegisterFormValues = new();
        }

        [Key]
        public long Id { get; set; }
        [Required,MaxLength(100)]
        public string Key { get; set; }

        [InverseProperty("UserFilledRegisterFormKey")]
        public List<UserFilledRegisterFormValue> UserFilledRegisterFormValues { get; set; }
    }
}
