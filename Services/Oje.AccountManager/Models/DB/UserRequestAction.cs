using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.AccountService.Models.DB
{
    [Table("UserRequestActions")]
    public class UserRequestAction
    {
        public long UserId { get; set; }
        [ForeignKey("UserId"), InverseProperty("UserRequestActions")]
        public User User { get; set; }
        public long ActionId { get; set; }
        [ForeignKey("ActionId"), InverseProperty("UserRequestActions")]
        public Action Action { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
