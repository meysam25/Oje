using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.RegisterForm.Models.DB
{
    [Table("UserRegisterFormRequiredDocuments")]
    public class UserRegisterFormRequiredDocument
    {
        [Key]
        public int Id { get; set; }
        public int UserRegisterFormRequiredDocumentTypeId { get; set; }
        [ForeignKey("UserRegisterFormRequiredDocumentTypeId"), InverseProperty("UserRegisterFormRequiredDocuments")]
        public UserRegisterFormRequiredDocumentType UserRegisterFormRequiredDocumentType { get; set; }
        [Required, MaxLength(100)]
        public string Title { get; set; }
        public bool IsActive { get; set; }
        public bool IsRequired { get; set; }
        [MaxLength(100)]
        public string DownloadFile { get; set; }
        public int SiteSettingId { get; set; }
    }
}
