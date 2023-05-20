using Oje.Infrastructure.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;


namespace Oje.Section.Secretariat.Models.DB
{
    [Table("SecretariatLetterUsers")]
    public class SecretariatLetterUser
    {
        [Key]
        public long Id { get; set; }
        public long SecretariatLetterId { get; set; }
        [ForeignKey(nameof(SecretariatLetterId)), InverseProperty("SecretariatLetterUsers")]
        public SecretariatLetter SecretariatLetter { get; set; }
        public long UserId { get; set; }
        [ForeignKey(nameof(UserId)), InverseProperty("SecretariatLetterUsers")]
        public User User { get; set; }
        public SecretariatLetterUserType Type { get; set; }
        public DateTime CreateDate { get; set; }
        public long? ByUserId { get; set; }
        [ForeignKey(nameof(ByUserId)), InverseProperty("BySecretariatLetterUsers")]
        public User ByUser { get; set; }
    }
}
