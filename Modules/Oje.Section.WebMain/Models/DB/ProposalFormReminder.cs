using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.WebMain.Models.DB
{
    [Table("ProposalFormReminders")]
    public class ProposalFormReminder
    {
        [Column(TypeName = "datetime")]
        public DateTime CreateDate { get; set; }
        public byte Ip1 { get; set; }
        public byte Ip2 { get; set; }
        public byte Ip3 { get; set; }
        public byte Ip4 { get; set; }
        public int ProposalFormId { get; set; }
        public long Mobile { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime TargetDate { get; set; }
        [MaxLength(4000)]
        public string Description { get; set; }
        public int SiteSettingId { get; set; }
    }
}
