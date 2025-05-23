﻿using Oje.Infrastructure.Interfac;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.Question.Models.DB
{
    [Table("YourQuestions")]
    public class YourQuestion: IEntityWithSiteSettingId
    {
        [Key]
        public int Id { get; set; }
        [Required,MaxLength(1000)]
        public string Title { get; set; }
        [Required, MinLength(4000)]
        public string Answer { get; set; }
        public bool IsActive { get; set; }
        public int SiteSettingId { get; set; }
        [ForeignKey("SiteSettingId"), InverseProperty("YourQuestions")]
        public SiteSetting SiteSetting { get; set; }
    }
}
