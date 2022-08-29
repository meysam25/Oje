using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Security.Models.DB
{
    [Table("DebugInfos")]
    public class DebugInfo
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
