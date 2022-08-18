using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Security.Models.DB
{
    [Table("InValidRangeIps")]
    public class InValidRangeIp
    {
        public byte Ip1 { get; set; }
        public byte Ip2 { get; set; }
        public byte Ip3 { get; set; }
        public byte Ip4 { get; set; }
        public DateTime? LastTryDate { get; set; }
        public int? CountTry { get; set; }
        public bool? IsSuccess { get; set; }
        public string LastEmailErrorMessage { get; set; }
    }
}
