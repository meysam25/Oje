using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.WebMain.Models.DB
{
    [Table("AutoAnswerOnlineChatMessageLikes")]
    public class AutoAnswerOnlineChatMessageLike
    {
        public int AutoAnswerOnlineChatMessageId { get; set; }
        [ForeignKey("AutoAnswerOnlineChatMessageId"), InverseProperty("AutoAnswerOnlineChatMessageLikes")]
        public AutoAnswerOnlineChatMessage AutoAnswerOnlineChatMessage { get; set; }
        public byte Ip1 { get; set; }
        public byte Ip2 { get; set; }
        public byte Ip3 { get; set; } 
        public byte Ip4 { get; set; }
        public bool IsLike { get; set; }
        public int SiteSettingId { get; set; }
    }
}
