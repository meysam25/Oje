using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Security.Models.DB
{
    [Table("ErrorParameters")]
    public class ErrorParameter
    {
        [Key]
        public long ErrorId { get; set; }
        [ForeignKey("ErrorId"), InverseProperty("ErrorParameters")]
        public Error Error { get; set; }
        [Required]
        public string Parameters { get; set; }
    }
}
