using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.Question.Models.DB
{
    [Table("YourQuestions")]
    public class YourQuestion
    {
        [Key]
        public int Id { get; set; }
        [Required,MaxLength(1000)]
        public string Title { get; set; }
        [Required, MinLength(4000)]
        public string Answer { get; set; }
        public bool IsActive { get; set; }
        public int SiteSettingId { get; set; }
    }
}
