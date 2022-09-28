using Oje.Infrastructure.Interfac;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.Question.Models.DB
{
    [Table("UserRegisterFormYourQuestions")]
    public class UserRegisterFormYourQuestion: IEntityWithSiteSettingId
    {
        [Key]
        public int Id { get; set; }
        public int UserRegisterFormId { get; set; }
        [ForeignKey("UserRegisterFormId"), InverseProperty("UserRegisterFormYourQuestions")]
        public UserRegisterForm UserRegisterForm { get; set; }
        [Required, MaxLength(1000)]
        public string Title { get; set; }
        [Required, MaxLength(4000)]
        public string Answer { get; set; }
        public bool IsActive { get; set; }
        public int SiteSettingId { get; set; }
    }
}
