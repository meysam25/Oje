using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.Secretariat.Models.DB
{
    [Table("SecretariatHeaderFooters")]
    public class SecretariatHeaderFooter
    {
        public SecretariatHeaderFooter()
        {
            SecretariatLetterCategories = new();
        }

        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Title { get; set; }
        [Required, MaxLength(100)]
        public string HeaderUrl { get; set; }
        [Required, MaxLength(100)]
        public string FooterUrl { get; set; }
        public int SiteSettingId { get; set; }

        [InverseProperty(nameof(SecretariatHeaderFooter))]
        public List<SecretariatLetterCategory> SecretariatLetterCategories { get; set; }
    }
}
