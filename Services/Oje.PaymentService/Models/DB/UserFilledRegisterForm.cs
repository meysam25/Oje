using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.PaymentService.Models.DB
{
    [Table("UserFilledRegisterForms")]
    public class UserFilledRegisterForm
    {
        [Key]
        public long Id { get; set; }
        [MaxLength(50)]
        public string PaymentTraceCode { get; set; }
        public int SiteSettingId { get; set; }
    }
}
