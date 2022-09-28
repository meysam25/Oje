using Oje.Infrastructure.Interfac;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.RegisterForm.Models.DB
{
    [Table("UserRegisterFormRequiredDocumentTypes")]
    public class UserRegisterFormRequiredDocumentType: IEntityWithSiteSettingId
    {
        public UserRegisterFormRequiredDocumentType()
        {
            UserRegisterFormRequiredDocuments = new();
        }

        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Title { get; set; }
        public int UserRegisterFormId { get; set; }
        [ForeignKey("UserRegisterFormId"), InverseProperty("UserRegisterFormRequiredDocumentTypes")]
        public UserRegisterForm UserRegisterForm { get; set; }
        public bool IsActive { get; set; }
        public int SiteSettingId { get; set; }

        [InverseProperty("UserRegisterFormRequiredDocumentType")]
        public List<UserRegisterFormRequiredDocument> UserRegisterFormRequiredDocuments { get; set; }
    }
}
