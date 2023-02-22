using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.Tender.Models.DB
{
    [Table("UserRegisterForms")]
    public class UserRegisterForm
    {
        public UserRegisterForm()
        {
            TenderFiles = new();
        }

        [Key]
        public int Id { get; set;  }
        [Required, MaxLength(300)]
        public string Title { get; set; }
        public int SiteSettingId { get; set; }

        [InverseProperty("UserRegisterForm")]
        public List<TenderFile> TenderFiles { get; set; }
    }
}
