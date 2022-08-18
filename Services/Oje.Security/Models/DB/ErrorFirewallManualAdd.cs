using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Security.Models.DB
{
    [Table("ErrorFirewallManualAdds")]
    public class ErrorFirewallManualAdd
    {
        public byte Ip1 { get; set; }
        public byte Ip2 { get; set; }   
        public byte Ip3 { get; set; }   
        public byte Ip4 { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
