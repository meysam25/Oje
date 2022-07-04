using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Security.Models.DB
{
    [Table("DebugEmailReceivers")]
    public class DebugEmailReceiver
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Email { get; set; }
        public bool IsActive { get; set; }
    }
}
