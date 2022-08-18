using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Security.Models.DB
{
    [Table("ValidRangeIps")]
    public class ValidRangeIp
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(200)]
        public string Title { get; set; }
        public byte FromIp1 { get; set; }
        public byte FromIp2 { get; set; }
        public byte FromIp3 { get; set; }
        public byte FromIp4 { get; set; }
        public byte ToIp1 { get; set; }
        public byte ToIp2 { get; set; }
        public byte ToIp3 { get; set; }
        public byte ToIp4 { get; set; }
        public bool IsActive { get; set; }
    }
}
