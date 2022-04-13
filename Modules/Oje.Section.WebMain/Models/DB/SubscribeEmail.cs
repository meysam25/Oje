using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.WebMain.Models.DB
{
    [Table("SubscribeEmails")]
    public class SubscribeEmail
    {
        [Key]
        public long Id { get; set; }
        [Required, MaxLength(100)]
        public string Email { get; set; }
        public byte Ip1 { get; set; }
        public byte Ip2 { get; set; }
        public byte Ip3 { get; set; }
        public byte Ip4 { get; set; }
        public int SiteSettingId { get; set; }
    }
}
