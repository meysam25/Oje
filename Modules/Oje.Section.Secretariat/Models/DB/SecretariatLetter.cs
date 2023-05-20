using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.Secretariat.Models.DB
{
    [Table("SecretariatLetters")]
    public class SecretariatLetter
    {
        public SecretariatLetter()
        {
            SecretariatLetterUsers = new();
        }

        [Key]
        public long Id { get; set; }
        [Required, MaxLength(200)]
        public string Title { get; set; }
        [Required, MaxLength(200)]
        public string SubTitle { get; set; }
        [Required, MaxLength(200)]
        public string Subject { get; set; }
        [Required]
        public string Body { get; set; }
        public int SecretariatUserDigitalSignatureId { get; set; }
        [ForeignKey(nameof(SecretariatUserDigitalSignatureId)), InverseProperty("SecretariatLetters")]
        public SecretariatUserDigitalSignature SecretariatUserDigitalSignature { get; set; }
        public int SecretariatLetterCategoryId { get; set; }
        [ForeignKey(nameof(SecretariatLetterCategoryId)), InverseProperty("SecretariatLetters")]
        public SecretariatLetterCategory SecretariatLetterCategory { get; set; }
        public DateTime CreateDate { get; set; }
        public long UserId { get; set; }
        [ForeignKey(nameof(UserId)), InverseProperty("SecretariatLetters")]
        public User User { get; set; }
        public bool IsConfirm { get; set; }
        public int SiteSettingId { get; set; }

        [InverseProperty(nameof(SecretariatLetter))]
        public List<SecretariatLetterUser> SecretariatLetterUsers { get; set; }
    }
}
