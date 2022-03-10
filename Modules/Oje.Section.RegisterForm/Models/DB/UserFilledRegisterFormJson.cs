using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.RegisterForm.Models.DB
{
    [Table("UserFilledRegisterFormJsons")]
    public class UserFilledRegisterFormJson
    {
        [Key]
        public long UserFilledRegisterFormId { get; set; }
        [ForeignKey("UserFilledRegisterFormId"), InverseProperty("UserFilledRegisterFormJsons")]
        public UserFilledRegisterForm UserFilledRegisterForm { get; set; }
        [Required]
        public string JsonConfig { get; set; }
    }
}
