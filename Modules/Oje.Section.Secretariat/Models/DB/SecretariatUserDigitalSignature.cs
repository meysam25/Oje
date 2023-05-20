using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.Secretariat.Models.DB
{
    [Table("SecretariatUserDigitalSignatures")]
    public class SecretariatUserDigitalSignature
    {
        public SecretariatUserDigitalSignature()
        {
            SecretariatLetters = new();
        }

        [Key]
        public int Id { get; set; }
        [MaxLength(100)]
        public string Title { get; set; }
        [Required, MaxLength(100)]
        public string Role { get; set; }
        [Required, MaxLength(100)]
        public string SignatureSamle { get; set; }
        public bool IsActive { get; set; }
        public long UserId { get; set; }
        [ForeignKey(nameof(UserId)), InverseProperty("SecretariatUserDigitalSignatures")]
        public User User { get; set; }
        public int SiteSettingId { get; set; }

        [InverseProperty(nameof(SecretariatUserDigitalSignature))]
        public List<SecretariatLetter> SecretariatLetters { get; set; }
    }
}
