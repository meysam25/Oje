using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.Secretariat.Models.DB
{
    [Table("SecretariatHeaderFooterDescriptions")]
    public class SecretariatHeaderFooterDescription
    {
        public SecretariatHeaderFooterDescription()
        {
            SecretariatLetterCategories = new();
        }

        [Key]
        public int Id { get; set; }
        [Required, MaxLength( 100)]
        public string Title { get; set; }
        [MaxLength(4000)]
        public string Header { get; set; }
        [MaxLength(4000)]
        public string Footer { get; set; }
        public int SiteSettingId { get; set; }

        [InverseProperty(nameof(SecretariatHeaderFooterDescription))]
        public List<SecretariatLetterCategory> SecretariatLetterCategories { get; set; }
    }
}
