using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.WebMain.Models.DB
{
    [Table("AutoAnswerOnlineChatMessages")]
    public class AutoAnswerOnlineChatMessage
    {
        public AutoAnswerOnlineChatMessage()
        {
            Childs = new();
            AutoAnswerOnlineChatMessageLikes = new();
        }

        [Key]
        public int Id { get; set; }
        public int? ParentId { get; set; }
        [ForeignKey("ParentId"), InverseProperty("Childs")]
        public AutoAnswerOnlineChatMessage Parent { get; set; }
        [MaxLength(50)]
        public string Title { get; set; }
        [Required, MaxLength(4000)]
        public string Description { get; set; }
        public int Order { get; set; }
        [MaxLength(100)]
        public string Link { get; set; }
        public bool IsActive { get; set; }
        public bool IsMessage { get; set; }
        public bool? HasLike { get; set; }
        public int SiteSettingId { get; set; }

        [InverseProperty("Parent")]
        public List<AutoAnswerOnlineChatMessage> Childs { get; set; }
        [InverseProperty("AutoAnswerOnlineChatMessage")]
        public List<AutoAnswerOnlineChatMessageLike> AutoAnswerOnlineChatMessageLikes { get; set; }
    }
}
