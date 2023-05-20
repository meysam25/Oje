using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.Secretariat.Models.DB
{
    [Table("SecretariatLetterCategories")]
    public class SecretariatLetterCategory
    {
        public SecretariatLetterCategory()
        {
            SecretariatLetters = new();
        }

        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Title { get; set; }
        public bool IsActive { get; set; }
        public int SecretariatHeaderFooterId { get; set; }
        [ForeignKey(nameof(SecretariatHeaderFooterId)), InverseProperty("SecretariatLetterCategories")]
        public SecretariatHeaderFooter SecretariatHeaderFooter { get; set; }
        public int SecretariatHeaderFooterDescriptionId { get; set; }
        [ForeignKey(nameof(SecretariatHeaderFooterDescriptionId)), InverseProperty("SecretariatLetterCategories")]
        public SecretariatHeaderFooterDescription SecretariatHeaderFooterDescription { get; set; }
        [Required, MaxLength(50)]
        public string Code { get; set; }
        public int SiteSettingId { get; set; }

        [InverseProperty(nameof(SecretariatLetterCategory))]
        public List<SecretariatLetter> SecretariatLetters { get; set; }
    }
}
